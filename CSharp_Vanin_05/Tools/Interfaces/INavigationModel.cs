namespace CSharp_Vanin_05.Tools.Interfaces
{
    internal enum ViewType
    {
        ProcessGridView = 0,
        ModuleGridView = 1,
        ThreadGridView = 2
    }

    interface INavigationModel
    {
        void Navigate(ViewType viewType);
    }
}