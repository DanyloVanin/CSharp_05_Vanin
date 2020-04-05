using System;
using System.Diagnostics;

namespace CSharp_Vanin_05.Models
{
    internal class ThreadHolder
    {
        #region Fields
        private readonly ProcessThread _thread;
        #endregion

        #region Properties
        public int Id => _thread.Id;

        public ThreadState State => _thread.ThreadState;

        public string StartTime
        {
            get
            {
                try
                {
                    return _thread.StartTime.ToString("HH:mm:ss dd/MM/yyyy");
                }
                catch (Exception)
                {
                    return "ACCESS DENIED";
                }
            }
        }

        #endregion

        #region Constructor
        internal ThreadHolder(ProcessThread thread)
        {
            _thread = thread;
        }
        #endregion
       
    }
}