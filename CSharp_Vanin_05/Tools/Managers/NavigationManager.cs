using CSharp_Vanin_05.Tools.Interfaces;

namespace CSharp_Vanin_05.Tools.Managers
{
    internal class NavigationManager
    {
        #region Fields

        private static readonly object Locker = new object();
        private static NavigationManager _instance;
        private INavigationModel _navigationModel;

        #endregion

        #region Properties

        internal static NavigationManager Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                lock (Locker)
                {
                    return _instance ??= new NavigationManager();
                }
            }
        }

        #endregion

        #region Constructors

        private NavigationManager()
        {
        }

        #endregion

        #region Methods

        internal void Initialize(INavigationModel navigationModel)
        {
            _navigationModel = navigationModel;
        }

        internal void Navigate(ViewType viewType)
        {
            _navigationModel.Navigate(viewType);
        }

        #endregion
    }
}