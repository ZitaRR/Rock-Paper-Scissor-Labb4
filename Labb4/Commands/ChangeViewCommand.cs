using Labb4.ViewModels;
using System;
using System.Windows.Input;

namespace Labb4.Commands
{
    internal class ChangeViewCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private MainViewModel main;

        public ChangeViewCommand(MainViewModel main)
        {
            this.main = main;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "Menu":
                    main.CurrentView = new MenuViewModel();
                    break;
                case "Game":
                    main.CurrentView = new GameViewModel();
                    break;
                case "Leaderboards":
                    main.CurrentView = new LeaderboardsViewModel();
                    break;
            }
        }
    }
}
