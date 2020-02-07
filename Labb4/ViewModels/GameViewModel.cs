using Labb4.Commands;
using Labb4.Models;

namespace Labb4.ViewModels
{
    internal class GameViewModel : BaseViewModel
    {
        public GameSelectionCommand SelectionCommand { get; }
        public MatchmakeCommand MatchmakeCommand { get; }
        public SendMoveCommand MoveCommand { get; }
        public string Scissors { get; } = @"C:\Users\Nicklas\Desktop\Code\Databas_Kurs\Labb4\Labb4\Resources\Images\icons8-hand-scissors-100.png";
        public string Stone { get; } = @"C:\Users\Nicklas\Desktop\Code\Databas_Kurs\Labb4\Labb4\Resources\Images\icons8-clenched-fist-100.png";
        public string Paper { get; } = @"C:\Users\Nicklas\Desktop\Code\Databas_Kurs\Labb4\Labb4\Resources\Images\icons8-hand-100.png";

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
