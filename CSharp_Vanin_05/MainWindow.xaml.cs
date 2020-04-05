using System.ComponentModel;
using System.Windows;
using CSharp_Vanin_05.Tools.Managers;
using CSharp_Vanin_05.ViewModels;

namespace CSharp_Vanin_05
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            StationManager.Instance.CloseApp();
        }
    }
}
