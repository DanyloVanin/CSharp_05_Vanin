using System.Windows.Controls;
using CSharp_Vanin_05.Tools.Interfaces;
using CSharp_Vanin_05.ViewModels;

namespace CSharp_Vanin_05.Views
{
    public partial class ThreadsGridView : UserControl, INavigatable
    {
        public ThreadsGridView()
        {
            InitializeComponent();
            DataContext = new ThreadGridViewModel();
        }
    }
}