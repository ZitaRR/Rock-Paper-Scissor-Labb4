using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Labb4ClassLibrary
{
    public class Server
    {
        public static readonly string IP = "192.168.3.221";
        public static readonly int PORT = 2001;

        private IPEndPoint endPoint;
        private TcpListener listener;
        private List<Client> clients = new List<Client>();

        public Server()
        {
            endPoint = new IPEndPoint(IPAddress.Parse(IP), PORT);
            listener = new TcpListener(endPoint);
            listener.Start();

            Console.WriteLine($"Server is running on port {endPoint.Port}.\n" +
                $"The local end-point is: {listener.LocalEndpoint}.");

            ListenForConnections();
        }

        private void ListenForConnections()
        {
            while (true)
            {
                Socket client = listener.AcceptSocket();

                byte[] data = new byte[1024];
                client.Receive(data);
                var game = GameData.GetObject(data);

                clients.Add(new Client(client, game));

                if (clients.Count > 1)
                {
                    List<Client> players = new List<Client>();
                    foreach (var _client in clients)
                    {
                        if (_client.GameData.HasFoundGame == false)
                        {
                            players.Add(_client);
                            if (players.Count >= 2)
                                break;
                        }
                    }

                    if (players.Count >= 2)
                    {
                        new Thread(() => new Game(players[0], players[1])).Start();
                    }
                }
            }
        }
    }
}
