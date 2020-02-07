using Labb4.ViewModels;
using System;
using System.Windows.Input;

namespace Labb4.Commands
{
    internal class SendMoveCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private GameViewModel game;

        public SendMoveCommand(GameViewModel game)
        {
            this.game = game;
        }

        public bool CanExecute(object parameter)
        {
            return (game.Game != null) && game.Game.GameData.CanPlay && 
                game.Game.GameData.SelectedMove > 0 && game.Game.GameData.HasFoundGame;
        }

        public void Execute(object parameter)
        {
            game.SendMove();
        }
    }
}
