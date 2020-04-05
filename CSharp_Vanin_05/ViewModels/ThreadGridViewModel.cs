using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using CSharp_Vanin_05.Models;
using CSharp_Vanin_05.Tools.Interfaces;
using CSharp_Vanin_05.Tools.Managers;
using CSharp_Vanin_05.Tools.MVVW;
using CSharp_Vanin_05.Tools.ViewModel;

namespace CSharp_Vanin_05.ViewModels
{
    internal class ThreadGridViewModel : BaseViewModel
    {
        private readonly ObservableCollection<ThreadHolder> _threads;
        private RelayCommand<object> _goBack;

        public string ProcessName { get; }

        public ObservableCollection<ThreadHolder> ThreadsCollection => _threads;
        public RelayCommand<object> GoBackCommand
        {
            get
            {
                return _goBack ??= new RelayCommand<object>(o => GoBackImplementation());
            }
        }

        private void GoBackImplementation()
        {
            try
            {
                NavigationManager.Instance.Navigate(ViewType.ProcessGridView);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        internal ThreadGridViewModel()
        {
            ProcessName = StationManager.SelectedProcess.Name;
            _threads = new ObservableCollection<ThreadHolder>();
            foreach (ProcessThread thread in StationManager.SelectedProcess.ThreadsCollection)
            {
                _threads.Add(new ThreadHolder(thread));
            }
        }
    }
}