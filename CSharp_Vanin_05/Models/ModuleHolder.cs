using System.Diagnostics;

namespace CSharp_Vanin_05.Models
{
    internal class ModuleHolder
    {
        #region Fields

        private readonly ProcessModule _module;

        #endregion

        #region Constructor
        internal ModuleHolder(ProcessModule module)
        {
            _module = module;
        }
        #endregion

        #region Regions
        public string Name => _module.ModuleName;

        public string FilePath
        {

            get
            {
                try
                {
                    return _module.FileName;
                }
                catch
                {
                    return "ACCESS DENIED";
                }
            }

        }
        #endregion

      
    }
}