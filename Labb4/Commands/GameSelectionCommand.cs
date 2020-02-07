using Labb4.ViewModels;
using Labb4ClassLibrary;
using System;
using System.Windows.Input;

namespace Labb4.Commands
{
    internal class GameSelectionCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private GameViewModel game;

        public GameSelectionCommand(GameViewModel game)
        {
            this.game = game;
        }

        public bool CanExecute(object parameter)
        {
            return game.Game != null;
        }

        public void Execute(object parameter)
        {
            var gameData = game.Game.GameData;
            gameData.SelectedMove = (Move)int.Parse((string)parameter);
            game.Game.GameData = gameData;
        }
    }
}
