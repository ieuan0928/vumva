using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace BOMBS.Core.Common
{
    public class ModelBase : IModelBase, INotifyPropertyChanged
    {
        protected ModelBase()
        {

        }
        public event PropertyChangedEventHandler PropertyChanged;
        private Dictionary<string, object> properties = new Dictionary<string, object>();
        protected object this[string key]
        {
            get
            {
                if (properties.ContainsKey(key) == false) return null;
                else return properties[key];
            }
            set
            {
                string kt = key.Trim();
                if (!properties.ContainsKey(kt)) properties.Add(kt, value);

                else
                {
                    object previousValue = properties[kt];

                    properties[kt] = value;

                    if (previousValue != value) OnPropertyChanged(kt);
                }
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));
        }

        public const string IdKey = "Id";
        public int Id
        {
            get
            {
                return (int)this[IdKey];
            }
            set
            {
                this[IdKey] = value;
            }
        }

        public const string DisplayNameKey = "DisplayName";
        public string DisplayName
        {
            get
            {
                return (string)this[DisplayNameKey];
            }
            set
            {
                this[DisplayNameKey] = value;
            }
        }

        public const string NameKey = "Name";
        public string Name
        {
            get
            {
                return (string)this[NameKey];
            }
            set
            {
                this[NameKey] = value;
                OnPropertyChanged("Name");
            }
        }




        public IEnumerable<string> Getproperties()
        {
            return properties.Keys;
        }
    }
}
