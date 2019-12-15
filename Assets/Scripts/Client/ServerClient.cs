using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.Client
{
    public class ServerClient : IDisposable
    {
        [Inject] public PlayerConnected PlayerConnectedSignal { get; set; }
        [Inject] public MapGenerated MapGeneratedSignal { get; set; }
        [Inject] public GameStarted GameStartedSignal { get; set; }

        private int? _curClientId;

        private readonly TcpClient _client;
        private readonly NetworkStream _stream;

        private ServerClient(TcpClient client)
        {
            _client = client;
            _stream = _client.GetStream();
        }

        public void StartDispatchingEvents()
        {
            ThreadPool.QueueUserWorkItem((_) => ReadData());
        }

        private void ReadData()
        {
            Debug.Log("Started receiving data from server...");
            while (_client.Connected)
            {
                if (_stream == null || !_stream.DataAvailable)
                {
                    continue;
                }

                var reader = new BinaryReader(_stream);
                var size = reader.ReadInt32();
                var message = reader.ReadBytes(size);
                var received = FromBytes(message);
                LogReceived(received);

                var jobject = JObject.Parse(received);
                HandleReceived(jobject);
            }
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
                    MapGeneratedSignal.Dispatch(args);
                } break;
                case "game_started":
                    GameStartedSignal.Dispatch();
                    break;
                case "connect":
                {
                    var args = jobject.ToObject<PlayerConnectedArgs>();

                    if (!_curClientId.HasValue)
                    {
                        _curClientId = args.Player.Id;
                    }

                    PlayerConnectedSignal.Dispatch(args);
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
            var writer = new BinaryWriter(_stream);
            var serialized = JsonConvert.SerializeObject(o);
            var bytes = ToBytes(serialized);
            writer.Write(bytes.Length);
            writer.Write(bytes);
            writer.Flush();
            LogSent(serialized);
        }
            
        private byte[] ToBytes(string input)
            => System.Text.Encoding.UTF8.GetBytes(input);

        private string FromBytes(byte[] input)
            => System.Text.Encoding.UTF8.GetString(input);

        private void LogReceived<T>(T received)
        {
            Debug.Log($"[client {_curClientId}] Received '{received}' from server.");
        }

        private void LogSent<T>(T sent)
        {
            Debug.Log($"[client {_curClientId}] Sent '{sent}' to server.");
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
