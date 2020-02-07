using Labb4.Commands;
using Labb4.Models;
using System.IO;
using System.Windows;

namespace Labb4.ViewModels
{
    internal class GameViewModel : BaseViewModel
    {
        public GameSelectionCommand SelectionCommand { get; }
        public MatchmakeCommand MatchmakeCommand { get; }
        public SendMoveCommand MoveCommand { get; }
        public string Scissors { get; } = @"\Resources\Images\icons8-hand-scissors-100.png";
        public string Stone { get; } = @"\Resources\Images\icons8-clenched-fist-100.png";
        public string Paper { get; } = @"\Resources\Images\icons8-hand-100.png";

        private GameModel game;
        public GameModel Game
        {
            get => game;
            set
            {
                game = value;
                OnPropertyChanged(nameof(Game));
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public GameViewModel()
        {
            string root = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            Scissors = root + Scissors;
            Stone = root + Stone;
            Paper = root + Paper;

            SelectionCommand = new GameSelectionCommand(this);
            MatchmakeCommand = new MatchmakeCommand(this);
            MoveCommand = new SendMoveCommand(this);
        }

        public void Matchmake()
        {
            Game = new GameModel(Name);
        }

        public void SendMove()
        {
            Game.GameData.CanPlay = false;
            game.Socket.Send(Game.GameData.ToBytes());
        }
    }
}
