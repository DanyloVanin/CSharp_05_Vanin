using System.Windows;
using CSharp_Vanin_05.Tools.Interfaces;
using CSharp_Vanin_05.Tools.Managers;
using CSharp_Vanin_05.Tools.Navigation;
using CSharp_Vanin_05.Tools.ViewModel;

namespace CSharp_Vanin_05.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel, ILoaderOwner, IContentOwner
    {
        #region Fields

        private Visibility _loaderVisibility = Visibility.Hidden;
        private bool _isUserInteractionAllowed = true;
        private INavigatable _content;

        #endregion

        #region Properties

        public INavigatable Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged();
            }
        }


        public Visibility LoaderVisibility
        {
            get => _loaderVisibility;
            set
            {
                _loaderVisibility = value;
                OnPropertyChanged();
            }
        }

        public bool IsUserInteractionAllowed
        {
            get => _isUserInteractionAllowed;
            set
            {
                _isUserInteractionAllowed = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructor

        internal MainWindowViewModel()
        {
            StationManager.Instance.Initialize();
            LoaderManager.Instance.Initialize(this);
            NavigationManager.Instance.Initialize(new NavigationModel(this));
            NavigationManager.Instance.Navigate(ViewType.ProcessGridView);
        }

        #endregion
    }
}