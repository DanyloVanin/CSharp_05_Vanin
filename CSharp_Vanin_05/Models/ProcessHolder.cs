using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using CSharp_Vanin_05.Tools;

namespace CSharp_Vanin_05.Models
{
    public class ProcessHolder
    {
        #region Fields

        private readonly Process _process;
        private readonly PerformanceCounter _cpuCounter;
        #endregion

        #region Constructor

        internal ProcessHolder(Process process)
        {
            _process = process;
            _cpuCounter = new PerformanceCounter("Process", "% Processor Time", _process.ProcessName);
            _cpuCounter.NextValue();
        }


        #endregion

        #region Properties

        public Process ProcessInstance => _process;

        public string Name => _process.ProcessName;

        public int Id => _process.Id;

        public bool IsActive => _process.Responding;

        public double UsageCpu => Math.Round((double) _cpuCounter.NextValue()/Environment.ProcessorCount, 2);
        public double AmountMemory => Math.Round(((double)(_process.WorkingSet64) / 1024 / 1024), 1);

        public double UsageMemory
        {
            get
            {
                PerformanceInfo.PerformanceInformation pi = new PerformanceInfo.PerformanceInformation();
                Int64 totalRam;
                if (PerformanceInfo.GetPerformanceInfo(out pi, Marshal.SizeOf(pi)))
                {
                    totalRam = pi.PhysicalTotal.ToInt64() * pi.PageSize.ToInt64() / 1048576;
                }
                else totalRam = Convert.ToInt64(1);
                long appRam = (_process.WorkingSet64 + _process.PrivateMemorySize64) / 1048576;
                double RAM = Math.Round((Double)appRam, 2);
                return Math.Round((Double)RAM / totalRam * 100, 2);
            }
        }


        public int ThreadsNumber => _process.Threads.Count;


        public string User
        {
            get
            {
                var processHandle = IntPtr.Zero;
                try
                {
                    OpenProcessToken(_process.Handle, 8, out processHandle);
                    var wi = new WindowsIdentity(processHandle);
                    var user = wi.Name;
                    return user.Contains(@"\") ? user.Substring(user.IndexOf(@"\", StringComparison.Ordinal) + 1) : user;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    if (processHandle != IntPtr.Zero)
                    {
                        CloseHandle(processHandle);
                    }
                }
            }

        }

        public ProcessModuleCollection ModulesCollection => _process.Modules;

        public ProcessThreadCollection ThreadsCollection => _process.Threads;

        public string FilePath
        {
            get
            {
                try
                {
                    return _process.MainModule?.FileName;
                }
                catch (Exception)
                {
                    return "Access denied";
                }
            }
        }

        public string StartTime
        {
            get
            {
                try
                {
                    return _process.StartTime.ToString("HH:mm:ss dd/MM/yyyy");
                }
                catch (Exception)
                {
                    return "Access denied";
                }
            }
        }

        public bool IsAvaliable()
        {
            return (StartTime != "Access denied");
        }
        #endregion

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);
    }
}