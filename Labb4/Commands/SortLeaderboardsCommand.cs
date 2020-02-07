using Labb4.ViewModels;
using Labb4ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Labb4.Commands
{
    internal class SortLeaderboardsCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private LeaderboardsViewModel leaderboards;

        public SortLeaderboardsCommand(LeaderboardsViewModel leaderboards)
        {
            this.leaderboards = leaderboards;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            List<MatchResult> sorted = null;
            switch ((int)parameter)
            {
                case 0:
                    sorted = leaderboards.Matches.OrderBy(x => x.Name).ToList();
                    break;
                case 1:
                    sorted = leaderboards.Matches.OrderBy(x => x.Result).Reverse().ToList();
                    break;
                case 2:
                    sorted = leaderboards.Matches.OrderBy(x => x.Length).ToList();
                    break;
                case 3:
                    sorted = leaderboards.Matches.OrderBy(x => x.Date).ToList();
                    break;
            }
            if (sorted != null)
            {
                leaderboards.Matches = sorted;
            }
        }
    }
}
