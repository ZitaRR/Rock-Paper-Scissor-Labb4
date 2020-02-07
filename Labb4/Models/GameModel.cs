using Labb4ClassLibrary;
using System.Net.Sockets;
using System.Threading;
using System.ComponentModel;
using System.Windows;

namespace Labb4.Models
{
    internal class GameModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Socket Socket { get; }

        private GameData gameData;
        public GameData GameData
        {
            get => gameData;
            set
            {
                gameData = value;
                OnPropertyChanged(nameof(GameData));
            }
        }

        public GameModel(string name)
        {
            Socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            Socket.Connect(Server.IP, Server.PORT);
            GameData = new GameData { PlayerName = name };
            Socket.Send(GameData.ToBytes());

            new Thread(() =>
            {
                byte[] data = new byte[1024];
                Socket.Receive(data);
                GameData = GameData.GetObject(data);
                MessageBox.Show($"You have found an opponent.\n" +
                    $"You may now make your move!", 
                    $"Your opponent: {GameData.OpponentName}");

                while (true)
                {
                    data = new byte[1024];
                    Socket.Receive(data);
                    GameData = GameData.GetObject(data);
                }
            }).Start();
        }

        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
