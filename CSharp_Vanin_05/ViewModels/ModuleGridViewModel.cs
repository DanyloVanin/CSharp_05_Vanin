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
    internal class ModuleGridViewModel : BaseViewModel
    {
        private readonly ObservableCollection<ModuleHolder> _modules;
        private RelayCommand<object> _goBack;
        public string ProcessName
        {
            get;
        }

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

        public ObservableCollection<ModuleHolder> ModulesCollection => _modules;

        internal ModuleGridViewModel()
        {
            ProcessName = StationManager.SelectedProcess.Name;
            _modules = new ObservableCollection<ModuleHolder>();
            foreach (ProcessModule module in StationManager.SelectedProcess.ModulesCollection)
            {
                _modules.Add(new ModuleHolder(module));
            }
        }
    }
}