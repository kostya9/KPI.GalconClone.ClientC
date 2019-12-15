using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Assets.Scripts.Client
{
    /*public class ServerClient : IDisposable
    {
        private readonly TcpClient _client;
        private readonly NetworkStream _stream;

        private ServerClient(TcpClient client)
        {
            _client = client;
            _stream = _client.GetStream();
            Task.Run(ReadData);
        }

        private async Task ReadData()
        {
            using(var reader = new BinaryReader(_stream))
            while (!_client.Connected)
            {
                if (!_stream.DataAvailable)
                {
                    await Task.Delay(100);
                    continue;
                }

                var s = reader.ReadString();
                Debug.WriteLine(s);
            }
        }

        public static async Task<ServerClient> Init(string address, int port)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            await socket.ConnectAsync(IPAddress.Parse(address), port);

            var tcpClient = new TcpClient(address, port);

            await tcpClient.ConnectAsync(address, port);

            return new ServerClient(tcpClient);
        }

        public void Dispose()
        {
            _stream?.Dispose();
            _client?.Dispose();
        }
    }*/
}
