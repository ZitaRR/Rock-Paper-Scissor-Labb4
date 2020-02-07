using System;
using System.Net.Sockets;
using System.Linq;

namespace Labb4ClassLibrary
{
    internal class Client
    {
        public Socket Socket { get; }
        public GameData GameData { get; internal set; }
        public bool Connected { get; private set; } = true;

        public Client(Socket socket, GameData data)
        {
            GameData = data;
            Socket = socket;
            Console.WriteLine($"{GameData.PlayerName} connected and is now matchmaking.");
        }

        internal void SendData()
        {
            Socket.Send(GameData.ToBytes());
        }

        internal void SaveMatchResult(int id ,bool won, TimeSpan time)
        {
            Server.Cosmos.Users.Add(new MatchResult
            {
                Id = id,
                Name = GameData.PlayerName,
                Result = won ? "Win" : "Lost",
                Length = time.ToString(),
                Date = DateTime.UtcNow.ToString()
            });
            Server.Cosmos.SaveChanges();

            Console.WriteLine("Data saved to database.");
        }
    }
}
