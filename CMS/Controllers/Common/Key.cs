using System;
namespace CMS.Controllers.Common
{
    /// <summary>
    /// Summary description for Config
    /// </summary>
    public sealed class Key
    {
        private static readonly Key instance = new Key();
        private readonly string _sKey;
        public static string sKey
        { 
            get { 
                return instance._sKey; 
            }
        }
        Key()
        {
            _sKey = "support" + DateTime.Now.Month;
        }
        public static Key Instance
        {
            get { return instance; }
        }
    }
}