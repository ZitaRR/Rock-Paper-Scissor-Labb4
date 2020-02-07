using Labb4.ViewModels;
using System;
using System.Windows.Input;

namespace Labb4.Commands
{
    internal class MatchmakeCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private GameViewModel game;

        public MatchmakeCommand(GameViewModel game)
        {
            this.game = game;
        }

        public bool CanExecute(object parameter)
        {
            return game.Game == null && !string.IsNullOrEmpty(game.Name);
        }

        public void Execute(object parameter)
        {
            game.Matchmake();
        }
    }
}
