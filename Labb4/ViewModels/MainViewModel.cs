using Labb4.Commands;
using System.Collections.Generic;

namespace Labb4.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        public ChangeViewCommand ViewCommand { get; }

        private BaseViewModel currentView;
        public BaseViewModel CurrentView
        {
            get => currentView;
            set
            {
                currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public static Dictionary<string, BaseViewModel> Views = new Dictionary<string, BaseViewModel>
        {
            ["Menu"] = new MenuViewModel(),
            ["Game"] = new GameViewModel()
        };

        public MainViewModel()
        {
            ViewCommand = new ChangeViewCommand(this);
        }
    }
}
