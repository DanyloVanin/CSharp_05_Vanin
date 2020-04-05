using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CSharp_Vanin_05.Models;
using CSharp_Vanin_05.ViewModels;

namespace CSharp_Vanin_05.Tools.Managers
{
    internal class StationManager
    {
        #region Fields

        private static List<ProcessHolder> _processList;
        private static ProcessHolder _selectedProcess;

        private static readonly object Locker = new object();
        private static StationManager _instance;
        #endregion

        #region Properties

        internal static StationManager Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                lock (Locker)
                {
                    return _instance ??= new StationManager();
                }
            }
        }
        internal static List<ProcessHolder> ProcessList => _processList;
        internal static ProcessHolder SelectedProcess
        {
            get => _selectedProcess;
            set => _selectedProcess = value;
        }
        internal static ProcessGridViewModel.SortTypeEnum SortingType { get; set; }
        internal void Initialize()
        {
            SortingType = 0;
            _processList = new List<ProcessHolder>();
            UpdateProcessList();
        }

        internal static void UpdateProcessList()
        {
            AddMissingProcesses();
            SortProcessList();
        }
        internal static void DeleteProcess(ref ProcessHolder p)
        {
            _processList.Remove(p);
        }

        internal static void SortProcessList()
        {
            _processList = SortingType switch
            {
                ProcessGridViewModel.SortTypeEnum.Id => (from p in _processList orderby p.Id select p).ToList(),
                ProcessGridViewModel.SortTypeEnum.Name => (from p in _processList orderby p.Name select p).ToList(),
                ProcessGridViewModel.SortTypeEnum.IsActive => (from p in _processList orderby p.IsActive select p).ToList(),
                ProcessGridViewModel.SortTypeEnum.UsageCpu => (from p in _processList orderby p.UsageCpu descending select p).ToList(),
                ProcessGridViewModel.SortTypeEnum.UsageMemory => (from p in _processList orderby p.UsageMemory descending select p).ToList(),
                ProcessGridViewModel.SortTypeEnum.AmountMemory => (from p in _processList orderby p.AmountMemory descending select p).ToList(),
                ProcessGridViewModel.SortTypeEnum.ThreadsNumber => (from p in _processList orderby p.ThreadsNumber descending select p).ToList(),
                ProcessGridViewModel.SortTypeEnum.User => (from p in _processList orderby p.User select p).ToList(),
                ProcessGridViewModel.SortTypeEnum.FilePath => (from p in _processList orderby p.FilePath select p).ToList(),
                ProcessGridViewModel.SortTypeEnum.StartTime => (from p in _processList orderby p.StartTime select p).ToList(),
                _ => (from p in _processList orderby p.Name select p).ToList()
            };
        }
        
        private static void AddMissingProcesses()
        {
            foreach (var process in Process.GetProcesses())
            {
                if (process == null) continue;
                if(!AlreadyInList(process.Id))
                    _processList.Add(new ProcessHolder(process));
            }
        }

        private static bool AlreadyInList(int processId)
        {
            return _processList.Any(process => processId == process.Id);
        }



        internal void CloseApp()
        {
            Environment.Exit(0);
        }
        #endregion
    }
}
