using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using BOMBS.Core.Common;
using BOMBS.Core.Entities;

namespace BOMBS.HRMS.Entites
{
   public class Employee : Person
    {
        public const string _EmployeeID = "EmployeeId";
        public int EmployeeId
        {
            get
            {
                return (int)base[_EmployeeID];
            }
            set
            {
                base[_EmployeeID] = value;
                OnPropertyChanged("EmployeeId");
            }
        }

        public const string _RequirementStatus = "Requirements";
        public string RequirementsStatus
        {
            get
            {
                return (string)base[_RequirementStatus];
            }
            set
            {
                base[_RequirementStatus] = value;
                OnPropertyChanged("RequirementsStatus");
            }
        }
    }
}
