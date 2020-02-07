using Labb4.Commands;
using Labb4ClassLibrary;
using System.Collections.Generic;
using System.Threading;

namespace Labb4.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        public static MainViewModel Instance { get; private set; }
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

        public MainViewModel()
        {
            Instance = this;
            ViewCommand = new ChangeViewCommand(this);
        }
    }
}
