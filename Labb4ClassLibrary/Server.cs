using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Labb4ClassLibrary
{
    public class Server
    {
        public static string IP { get; private set; }
        public static readonly int PORT = 2001;

        public static Cosmos Cosmos { get; private set; }

        private IPEndPoint endPoint;
        private TcpListener listener;
        private List<Client> clients = new List<Client>();

        public Server()
        {
            Cosmos = new Cosmos();
            Cosmos.Database.EnsureCreated();

            foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    IP = ip.ToString();
                }
            }

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

                if (string.IsNullOrEmpty(game.PlayerName))
                {
                    int size = Cosmos.Users.Count();
                    client.Send(BitConverter.GetBytes(size));

                    for (int i = 0; i < size; i++)
                    {
                        var result = Cosmos.Users.Find(i + 1);
                        client.Send(result.ToBytes());
                    }

                    continue;
                }

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
                        new Thread(() =>
                        {
                            new Game(players[0], players[1]);

                            clients.Remove(players[0]);
                            clients.Remove(players[1]);
                        }).Start();
                    }
                }
            }
        }
    }
}
