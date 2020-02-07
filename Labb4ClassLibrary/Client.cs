using System;
using System.Net.Sockets;

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
    }
}
