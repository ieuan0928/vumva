using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Core.Resources
{
    public class Property
    {
        private string fileName = string.Empty;
        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                if (string.IsNullOrEmpty(displayName)) displayName = fileName;
            }
        }

        private string path = string.Empty;
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public string FullFilePath
        {
            get { return string.Format(@"{0}\{1}", this.Path, this.FileName); }
        }

        private string displayName = string.Empty;
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }


    }
}
