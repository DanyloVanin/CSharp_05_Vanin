using System.Windows;
using CSharp_Vanin_05.Tools.Interfaces;

namespace CSharp_Vanin_05.Tools.Managers
{
    internal class LoaderManager
    {
        #region Fields

        private static readonly object Locker = new object();
        private static LoaderManager _instance;
        private ILoaderOwner _loaderOwner;

        #endregion

        #region Properties

        internal static LoaderManager Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                lock (Locker)
                {
                    return _instance ??= new LoaderManager();
                }
            }
        }

        #endregion

        #region Constructors

        private LoaderManager()
        {
        }

        #endregion

        #region Methods

        internal void Initialize(ILoaderOwner loaderOwner)
        {
            _loaderOwner = loaderOwner;
        }

        internal void ShowLoader()
        {
            _loaderOwner.LoaderVisibility = Visibility.Visible;
            BlockInterface();
        }

        internal void HideLoader()
        {
            _loaderOwner.LoaderVisibility = Visibility.Hidden;
            UnblockInterface();
        }

        private void BlockInterface()
        {
            _loaderOwner.IsUserInteractionAllowed = false;
        }

        private void UnblockInterface()
        {
            _loaderOwner.IsUserInteractionAllowed = true;
        }

        #endregion
    }
}