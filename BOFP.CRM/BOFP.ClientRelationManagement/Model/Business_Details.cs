using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOFP.ClientRelationManagement.Controls;
namespace BOFP.ClientRelationManagement.Model
{
    public class Business_Details : ModelBase
    {
        private DateTime _DateApplication;
        public DateTime DateApplication
        {
            get
            {
                return _DateApplication;
            }
            set
            {
                _DateApplication = value;
                OnPropertyChanged("DateApplication");
            }
        }
        private const string _Address = "Address";
        public string Address
        {
            get
            {
                return (string)base[_Address];
            }
            set
            {
                base[_Address] = value;
                OnPropertyChanged("Address");
            }
        }
        private const string _ORnumber = "ORNumber";
        public int ORNumber
        {
            get
            {
                return (int)base[_ORnumber];
            }
            set
            {
                base[_ORnumber] = value;
                OnPropertyChanged("ORNumber");
            }
        }

        private const string _PaymentFireCode = "PaymentFireCode";
        public double PaymentFireCode
        {
            get
            {
                return (double)base[_PaymentFireCode];
            }
            set
            {
                base[_PaymentFireCode] = value;
                OnPropertyChanged("PaymentFireCode");
            }
        }

        private DateTime _DateOR;
        public DateTime DateOR
        {
            get
            {
                return _DateOR;
            }
            set
            {
                _DateOR = value;
                OnPropertyChanged("DateOR");
            }
        }

        private const string _HeightofBuilding = "HeightofBuilding";
        public double HeightofBuilding
        {
            get
            {
                return (double)base[_HeightofBuilding];
            }
            set
            {
                base[_HeightofBuilding] = value;
                OnPropertyChanged("HeightofBuilding");
            }
        }
        private const string _FloorArea = "FloorArea";
        public double FloorArea
        {
            get
            {
                return (double)base[_FloorArea];
            }
            set
            {
                base[_FloorArea] = value;
                OnPropertyChanged("FloorArea");
            }
        }
        private const string _ClassificationOccupancy = "ClassificationOccupacy";
        public string ClassificationOccupancy
        {
            get
            {
                return (string)base[_ClassificationOccupancy];
            }
            set
            {
                base[_ClassificationOccupancy] = value;
            }
        }

        private const string _InspectionOrderNumber = "InspectionOrderNumber";
        public int InspectionOrderNumber
        {
            get
            {
                return (int)base[_InspectionOrderNumber];
            }

            set
            {
                base[_InspectionOrderNumber] = value;
                OnPropertyChanged("InspectionOrderNumber");
            }
        }

        private DateTime _DateOfInspection;
        public DateTime DateOfInspection
        {
            get
            {
                return _DateOfReleased;
            }
            set
            {
                _DateOfReleased = value;
                OnPropertyChanged("DateOR");
            }
        }

        private const string _Remark = "Remarks";
        public string Remarks
        {
            get
            {
                return (string)base[_Remark];
            }
            set
            {
                base[_Remark] = value;
                OnPropertyChanged("Remarks");
            }
        }

        private const string _FSICNumber = "FSICNumber";
        public int FSICNumber
        {
            get
            {
                return (int)base[_FSICNumber];
            }
            set
            {
                base[_FSICNumber] = value;
                OnPropertyChanged("FSICNumber");
            }
        }


        private DateTime _DateOfReleased;
        public DateTime DateOfReleased
        {
            get
            {
                return _DateOfReleased;
            }
            set
            {
                _DateOfReleased = value;
                OnPropertyChanged("DateOfReleased");
            }
        }

        private const string _BusinessName = "BusinessName";
        public string BusinessName 
        {
            get
            {
                return (string)base[_BusinessName];
            }
            set
            {
                base[_BusinessName] = value;
            }
        }
    }
}
