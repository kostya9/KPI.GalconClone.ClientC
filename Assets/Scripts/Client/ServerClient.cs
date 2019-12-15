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
        [Inject] MapGenerated MapGeneratedSignal { get; set; }

        private readonly TcpClient _client;
        private readonly NetworkStream _stream;

        private ServerClient(TcpClient client)
        {
            _client = client;
            _stream = _client.GetStream();
            ThreadPool.QueueUserWorkItem((_) => ReadData());
        }

        private void ReadData()
        {
            Debug.Log("Started receiving data from server...");
            var reader = new BinaryReader(_stream);
            while (_client.Connected)
            {
                if (!_stream.DataAvailable)
                {
                    continue;
                }

                var size = reader.ReadInt32();
                var message = reader.ReadBytes(size);
                var received = FromBytes(message);
                var jobject = JObject.Parse(received);
                LogReceived(jobject);
            }
        }

        public void SendReady()
        {
            var msg = new { name = "ready", ready = true };
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
            Debug.Log($"Received '{received}' from server.");
        }

        private void LogSent<T>(T sent)
        {
            Debug.Log($"Sent '{sent}' to server.");
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
