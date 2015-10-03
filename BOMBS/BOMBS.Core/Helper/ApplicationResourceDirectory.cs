using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Core.Helper
{
    public class ApplicationResourceDirectory
    {
        public static string DefaultFolderName
        {
            get { return "BOMBS"; }
        }

        private static string localApplicationData;
        public static string LocalApplicationData
        {
            get
            {
                if (string.IsNullOrEmpty(localApplicationData))
                    localApplicationData = string.Format(@"{0}\BOMBS", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
                return localApplicationData;
            }
        }
    }
}
