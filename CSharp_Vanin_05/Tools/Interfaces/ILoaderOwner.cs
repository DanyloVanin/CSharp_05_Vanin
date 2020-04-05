using System.ComponentModel;
using System.Windows;

namespace CSharp_Vanin_05.Tools.Interfaces
{
    internal interface ILoaderOwner : INotifyPropertyChanged
    {
        Visibility LoaderVisibility { get; set; }
        bool IsUserInteractionAllowed { get; set; }
    }
}