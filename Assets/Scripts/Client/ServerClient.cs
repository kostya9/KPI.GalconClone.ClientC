using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using strange.extensions.signal.impl;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.Client
{
    public class ServerClient : IDisposable
    {
        // Add [Inject] as soon as signal should be used
        public PlayerConnected PlayerConnectedSignal { get; set; }
        [Inject] public MapGenerated MapGeneratedSignal { get; set; }
        public GameStarted GameStartedSignal { get; set; }
        public PlayerReady PlayerReadySignal { get; set; }
        [Inject] public PlanetSelected PlanetSelectedSignal { get; set; }
        [Inject] public UnitMoved UnitMovedSignal { get; set; }
        [Inject] public HpAdded HpAddedSignal { get; set; }
        [Inject] public DamageDone DamageDoneSignal { get; set; }
        public GameOver GameOverSignal { get; set; }
        public PlayerInitialized PlayerInitializedSignal { get; set; }

        private int? _curClientId;

        private readonly TcpClient _client;
        private readonly NetworkStream _stream;
        private readonly BinaryWriter _writer;
        private readonly BinaryReader _reader;

        public ServerClient(TcpClient client)
        {
            _client = client;
            _stream = _client.GetStream();
            _writer = new BinaryWriter(_stream);
            _reader = new BinaryReader(_stream);
        }

        public void StartDispatchingEvents()
        {
            ThreadPool.QueueUserWorkItem((_) => ReadAndHandleEvents());
        }

        private void ReadAndHandleEvents()
        {
            Debug.Log("Started receiving data from server...");
            while (_client.Connected)
            {
                if (!_stream.DataAvailable)
                {
                    continue;
                }

                try
                {
                    var size = _reader.ReadInt32();
                    var message = _reader.ReadBytes(size);
                    var received = FromBytes(message);
                    LogReceived(received);

                    var jobject = JObject.Parse(received);
                    HandleReceived(jobject);
                }
                catch(Exception e)
                {
                    Debug.LogError($"Failed to process server message due to exception: {e}");
                }
            }
            Debug.Log("Connection closed. Stopped receiving.");
        }

        private void HandleReceived(JObject jobject)
        {
            switch(jobject.Value<string>("name"))
            {
                //case "player_init"
                // No player init event in server. (bug?)
                case "mapinit":
                {
                    var args = jobject.ToObject<MapContent>();
                    MapGeneratedSignal?.Dispatch(args);
                } break;
                case "game_started":
                    GameStartedSignal?.Dispatch();
                    break;
                case "connect":
                {
                    var args = jobject.ToObject<PlayerConnectedArgs>();
                    PlayerConnectedSignal?.Dispatch(args);
                } break;
                case "player_init":
                {
                    var args = jobject.ToObject<PlayerInitializedArgs>();
                    _curClientId = args.Id;
                    Constants.CURRENT_PLAYER_ID = args.Id;
                    PlayerInitializedSignal?.Dispatch(args);
                } break;
                case "ready":
                {
                    var args = jobject.ToObject<PlayerReadyArgs>();
                    PlayerReadySignal?.Dispatch(args);
                } break;
                case "select":
                {
                    var args = jobject.ToObject<PlanetSelectedArgs>();
                    PlanetSelectedSignal?.Dispatch(args);
                } break;
                case "move":
                {
                    var args = jobject.ToObject<UnitMovedArgs>();
                    UnitMovedSignal?.Dispatch(args);
                } break;
                case "add_hp":
                {
                    var args = jobject.ToObject<HpAddedArgs>();
                    HpAddedSignal?.Dispatch(args);
                } break;
                case "damage":
                {
                    var args = jobject.ToObject<DamageDoneArgs>();
                    DamageDoneSignal?.Dispatch(args);
                } break;
                case "gameover":
                {
                    var args = jobject.ToObject<GameOverArgs>();
                    GameOverSignal?.Dispatch(args);
                } break;
            }
        }

        public void SendReady()
        {
            var msg = new { name = "ready", ready = true };
            Send(msg);
        }

        public void SendRendered()
        {
            var msg = new { name = "rendered" };
            Send(msg);
        }

        public void SendSelect(int[] planetIds, int percentage)
        {
            var msg = new { name = "select", from = planetIds, percentage = percentage};
            Send(msg);
        }

        public void SendMove(int unitId, double x, double y)
        {
            var msg = new { name = "move", unit_id = unitId, x = x, y = y };
            Send(msg);
        }

        public void SendAddHp(int planetId, int hpCount = 1)
        {
            var msg = new { name = "add_hp", planet_id = planetId, hp_count = hpCount };
            Send(msg);
        }

        public void SendDamage(int planetId, int unitId, int hpCount = 1)
        {
            var msg = new { name = "damage", planet_id = planetId, unit_id = unitId, hp_count = hpCount };
            Send(msg);
        }

        private void Send(object o)
        {
            var serialized = JsonConvert.SerializeObject(o, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            var bytes = ToBytes(serialized);
            lock (_writer)
            {
                _writer.Write(bytes.Length);
                _writer.Write(bytes);
            }
            LogSent(serialized);
        }
            
        private byte[] ToBytes(string input)
            => System.Text.Encoding.UTF8.GetBytes(input);

        private string FromBytes(byte[] input)
            => System.Text.Encoding.UTF8.GetString(input);

        private void LogReceived<T>(T received)
        {
            Debug.Log($"[{DateTime.Now}][client {_curClientId}] Received '{received}' from server.");
        }

        private void LogSent<T>(T sent)
        {
            Debug.Log($"[{DateTime.Now}][client {_curClientId}] Sent '{sent}' to server.");
        }

        public static ServerClient Init(string address, int port)
        {
            var tcpClient = new TcpClient(address, port);

            return new ServerClient(tcpClient);
        }

        public void Dispose()
        {
            _client.Close();

            _stream?.Dispose();
            _client?.Dispose();
        }
    }
}
