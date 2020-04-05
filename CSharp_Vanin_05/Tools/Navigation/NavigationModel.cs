using System;
using CSharp_Vanin_05.Tools.Interfaces;
using CSharp_Vanin_05.Views;

namespace CSharp_Vanin_05.Tools.Navigation
{
    internal class NavigationModel : INavigationModel
    {
        #region Fields

        private readonly IContentOwner _contentOwner;
        private readonly INavigatable _processView;

        #endregion

        #region Constructor

        internal NavigationModel(IContentOwner contentOwner)
        {
            _contentOwner = contentOwner;
            _processView = new ProcessGridView();
        }

        #endregion

        #region Methods

        public void Navigate(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.ThreadGridView:
                    _contentOwner.Content = new ThreadsGridView();
                    break;
                case ViewType.ModuleGridView:
                    _contentOwner.Content = new ModulesGridView();
                    break;
                case ViewType.ProcessGridView:
                    _contentOwner.Content = _processView;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }
        }
    }

    #endregion
}