using Labb4ClassLibrary;
using System.Net.Sockets;
using System.Threading;
using System.ComponentModel;
using System.Windows;
using Labb4.ViewModels;

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

                    if (GameData.ShouldDisconnect)
                    {
                        bool won = GameData.PlayerScore > GameData.OpponentScore;
                        var result = won ? $"You won against {GameData.OpponentName} by {GameData.PlayerScore - GameData.OpponentScore} point(s)." :
                        $"You lost against {GameData.OpponentName} by {GameData.OpponentScore - GameData.PlayerScore} point(s).";
                        MessageBox.Show(result);
                        break;
                    }
                }

                MainViewModel.Instance.CurrentView = new MenuViewModel();
            }).Start();
        }

        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
