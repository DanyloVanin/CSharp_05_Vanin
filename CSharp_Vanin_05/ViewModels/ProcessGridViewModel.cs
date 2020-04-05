using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CSharp_Vanin_05.Models;
using CSharp_Vanin_05.Tools.Interfaces;
using CSharp_Vanin_05.Tools.Managers;
using CSharp_Vanin_05.Tools.MVVW;
using CSharp_Vanin_05.Tools.ViewModel;

namespace CSharp_Vanin_05.ViewModels
{
    internal class ProcessGridViewModel : BaseViewModel
    {
        #region Fields

        private ObservableCollection<ProcessHolder> _processCollection;

        private bool _isControlEnabled = true;
        private ProcessHolder _selectedProcess;

        private Thread _workingThread;
        private readonly CancellationToken _token;


        #region Commands
        private RelayCommand<object> _endTask;
        private RelayCommand<object> _openFolder;
        private RelayCommand<object> _showThreads;
        private RelayCommand<object> _showModules;
        #endregion

        #region SortCommands
        private RelayCommand<object> _sortById;
        private RelayCommand<object> _sortByName;
        private RelayCommand<object> _sortByIsActive;
        private RelayCommand<object> _sortByUsageCpu;
        private RelayCommand<object> _sortByUsageMemory;
        private RelayCommand<object> _sortByAmountMemory;
        private RelayCommand<object> _sortByThreadsNumber;
        private RelayCommand<object> _sortByUser;
        private RelayCommand<object> _sortByFilePath;
        private RelayCommand<object> _sortByStartTime;
        #endregion
        #endregion

        #region Properties

        #region Sort Commands
        public RelayCommand<object> SortById
        {
            get
            {
                return _sortById ??= new RelayCommand<object>(o =>
                    SortImplementation(SortTypeEnum.Id));
            }
        }
        public RelayCommand<object> SortByName
        {
            get
            {
                return _sortByName ??= new RelayCommand<object>(o =>
                    SortImplementation(SortTypeEnum.Name));
            }
        }
        public RelayCommand<object> SortByIsActive
        {
            get
            {
                return _sortByIsActive ??= new RelayCommand<object>(o =>
                    SortImplementation(SortTypeEnum.IsActive));
            }
        }
        public RelayCommand<object> SortByUsageCpu
        {
            get
            {
                return _sortByUsageCpu ??= new RelayCommand<object>(o =>
                    SortImplementation(SortTypeEnum.UsageCpu));
            }
        }
        public RelayCommand<object> SortByUsageMemory
        {
            get
            {
                return _sortByUsageMemory ??= new RelayCommand<object>(o =>
                    SortImplementation(SortTypeEnum.UsageMemory));
            }
        }
        public RelayCommand<object> SortByAmountMemory
        {
            get
            {
                return _sortByAmountMemory ??= new RelayCommand<object>(o =>
                    SortImplementation(SortTypeEnum.AmountMemory));
            }
        }
        public RelayCommand<object> SortByThreadsNumber
        {
            get
            {
                return _sortByThreadsNumber ??= new RelayCommand<object>(o =>
                    SortImplementation(SortTypeEnum.ThreadsNumber));
            }
        }
        public RelayCommand<object> SortByUser
        {
            get
            {
                return _sortByUser ??= new RelayCommand<object>(o =>
                    SortImplementation(SortTypeEnum.User));
            }
        }
        public RelayCommand<object> SortByFilePath
        {
            get
            {
                return _sortByFilePath ??= new RelayCommand<object>(o =>
                    SortImplementation(SortTypeEnum.FilePath));
            }
        }
        public RelayCommand<object> SortByStartTime
        {
            get
            {
                return _sortByStartTime ??= new RelayCommand<object>(o =>
                    SortImplementation(SortTypeEnum.StartTime));
            }
        }

        #region Sort Implementation

        private async void SortImplementation(SortTypeEnum param)
        {
            await Task.Run(() =>
            {
                try
                {
                    StationManager.SortingType = param;
                    StationManager.UpdateProcessList();
                    ProcessCollection = new ObservableCollection<ProcessHolder>(StationManager.ProcessList);
                }
                catch (Exception)
                {
                    MessageBox.Show("ERROR: cannot sort");
                }
            }, _token);
        }

        #endregion

        #endregion
        
        public ProcessHolder SelectedProcess
        {
            get => _selectedProcess;
            set
            {
                _selectedProcess = value;
                OnPropertyChanged();
            }
        }

        public bool IsControlEnabled
        {
            get => _isControlEnabled;
            set
            {
                _isControlEnabled = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ProcessHolder> ProcessCollection
        {
            get => _processCollection;
            private set
            {
                _processCollection = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<object> EndTask
        {
            get
            {
                return _endTask ??= new RelayCommand<object>(
                    EndTaskImplementation, o => CanExecuteCommand());
            }
        }
        public RelayCommand<object> OpenFolder
        {
            get
            {
                return _openFolder ??= new RelayCommand<object>(
                    OpenFolderImplementation, o => CanExecuteCommand());
            }
        }
        public RelayCommand<object> ShowThreads
        {
            get
            {
                return _showThreads ??= new RelayCommand<object>(
                    ShowThreadsImplementation, o => CanExecuteCommand());
            }
        }
        public RelayCommand<object> ShowModules
        {
            get
            {
                return _showModules ??= new RelayCommand<object>(
                    ShowModulesImplementation, o => CanExecuteCommand());
            }
        }

        #endregion

        #region Constructor
        internal ProcessGridViewModel()
        {
            _processCollection = new ObservableCollection<ProcessHolder>(StationManager.ProcessList);
            var tokenSource = new CancellationTokenSource();
            _token = tokenSource.Token;
            StartWorkingThread();
        }
        #endregion

        #region Threads

        private void StartWorkingThread()
        {
            _workingThread = new Thread(WorkingThreadProcess) {IsBackground = true};
            _workingThread.Start();
        }

        private void WorkingThreadProcess()
        {
            while (!_token.IsCancellationRequested)
            {
                StationManager.UpdateProcessList();
                ProcessCollection = new ObservableCollection<ProcessHolder>(StationManager.ProcessList);

                if (SelectedProcess != null)
                {
                    var temp = SelectedProcess.Id;
                    foreach (var p in ProcessCollection)
                    {
                        if (p.Id != temp) continue;
                        SelectedProcess = p;
                        break;
                    }
                }
                Thread.Sleep(2000);
            }
        }
        #endregion

        #region Commands Implementation

        private async void EndTaskImplementation(object obj)
        {
            LoaderManager.Instance.ShowLoader();
            await Task.Run(() =>
            {
                if (_selectedProcess.IsAvaliable())
                {
                    SelectedProcess?.ProcessInstance?.Kill();
                    StationManager.DeleteProcess(ref _selectedProcess);
                    StationManager.UpdateProcessList();
                    SelectedProcess = null;
                    ProcessCollection = new ObservableCollection<ProcessHolder>(StationManager.ProcessList);
                }
                else
                {
                    MessageBox.Show("ACCESS DENIED: cannot end process");
                }
            }, _token).ConfigureAwait(false);
            LoaderManager.Instance.HideLoader();
        }

        private void OpenFolderImplementation(object obj)
        {
            try
            {
                if (File.Exists(_selectedProcess.FilePath))
                {
                    Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + _selectedProcess.FilePath));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR: cannot open file explorer");
            }
        }

        private void ShowModulesImplementation(object obj)
        {
            StationManager.SelectedProcess = SelectedProcess;
            try
            {
                NavigationManager.Instance.Navigate(ViewType.ModuleGridView);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void ShowThreadsImplementation(object obj)
        {
            StationManager.SelectedProcess = SelectedProcess;
            try
            {
                NavigationManager.Instance.Navigate(ViewType.ThreadGridView);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        
        private bool CanExecuteCommand()
        {
            return SelectedProcess != null;
        }

        #endregion

        #region Enum

        internal enum SortTypeEnum
        {
            Id=0,
            Name,
            IsActive,
            UsageCpu,
            UsageMemory,
            AmountMemory,
            ThreadsNumber,
            User,
            FilePath,
            StartTime
        }

        #endregion
    }
}
