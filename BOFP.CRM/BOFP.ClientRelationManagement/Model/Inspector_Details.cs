using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOFP.ClientRelationManagement.Controls;
namespace BOFP.ClientRelationManagement.Model
{
    public class Inspector_Details : Business_Details
    {
        private const string _FirstName = "FirstName";
        public string FirstName
        {
            get
            {
                return (string)base[_FirstName];
            }

            set
            {
                base[_FirstName] = value;
                OnPropertyChanged("FirstName");
            }
        }

        private const string _MiddleName = "MiddleName";
        public string MiddleName
        {
            get
            {
                return (string)base[_MiddleName];
            }
            set
            {
                base[_MiddleName] = value;
                OnPropertyChanged("MiddleName");
            }
        }
        private const string _LastName = "LastName";
        public string LastName
        {
            get
            {
                return (string)base[_LastName];
            }
            set
            {
                base[_LastName] = value;
                OnPropertyChanged("LastName");
            }
        }
    }
}
