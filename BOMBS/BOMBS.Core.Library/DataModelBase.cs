using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using BOMBS.Core.Framework;

namespace BOMBS.Core.Library
{
    public abstract class DataModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged == null) return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private int baseId = 0;
        [IdProperty(DisplayName = "Base Id", FieldName = "BaseID")]
        public int BaseID
        {
            get { return baseId; }
        }

        private string displayName = null;
        [VarcharProperty(DisplayName = "Display Name", FieldName = "DisplayName")]
        public string DisplayName
        {
            get { return displayName; }
            set
            {
                if (displayName == value) return;

                displayName = value;
                FirePropertyChanged("DisplayName");
            }
        }

        private DateTime? dateCreated = null;
        [DateTimeProperty(DisplayName = "Date Created", Description = null, FieldName = "DateCreated")]
        public DateTime? DateCreated
        {
            get { return dateCreated; }
            set
            {
                if (dateCreated == value) return;

                dateCreated = value;
                FirePropertyChanged("DateCreated");
            }
        }

        private DateTime? dateLastUpdated = null;
        [DateTimeProperty(DisplayName = "Date Last Updated", Description = null, FieldName = "DateLastUpdated")]
        public DateTime? DateLastUpdated
        {
            get { return dateLastUpdated; }
            set
            {
                if (dateLastUpdated == value) return;

                dateLastUpdated = value;
                FirePropertyChanged("DateLastUpdated");
            }
        }
    }
}
