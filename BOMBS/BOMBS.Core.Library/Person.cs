using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BOMBS.Core.Framework;

namespace BOMBS.Core.Library
{
    [Entity(TableName = "Core_Library_Person", DisplayName = "Person")]
    public class Person : DataModelBase
    {
        public enum Sex { Male, Female }

        private string lastName = null;
        [VarcharProperty(AllowNull = false, FieldName = "LastName", DisplayName = "Last Name", Description = "Person's Last Name", MaxLength = 255)]
        public string LastName
        {
            get { return lastName; }
            set
            {
                if (lastName == value) return;
                lastName = value;
                FirePropertyChanged("LastName");
            }
        }

        private string firstName = null;
        [VarcharProperty(AllowNull = false, FieldName = "FirstName", DisplayName = "First Name", Description = "Person's First Name", MaxLength = 255)]
        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (firstName == value) return;
                firstName = value;
                FirePropertyChanged("FirstName");
            }
        }

        private string middleName = null;
        [VarcharProperty(AllowNull = true, FieldName = "MiddleName", DisplayName = "Middle Name", Description = "Person's Middle Name", MaxLength = 255)]
        public string MiddleName
        {
            get { return middleName; }
            set
            {
                if (middleName == value) return;
                middleName = value;
                FirePropertyChanged("MiddleName");
            }
        }

        private string salutation = null;
        [VarcharProperty(AllowNull = true, FieldName = "Salutation", DisplayName = "Salutation", Description = "Person's Salutation", MaxLength = 255)]
        public string Salutation
        {
            get { return salutation; }
            set
            {
                if (salutation == value) return;
                salutation = value;
                FirePropertyChanged("Salutation");
            }
        }

        private Sex? gender = null;
        [VarcharProperty(AllowNull = true, FieldName = "Gender", DisplayName = "Gender", Description = "Person's Gender")]
        public Sex? Gender
        {
            get { return gender; }
            set
            {
                if (gender == value) return;
                gender = value;
                FirePropertyChanged("Gender");
            }
        }

        [DateTimeProperty(AllowNull = true, FieldName = "BirthDate", DisplayName = "BirthDate", Description = "Person's Date of Birth")]
        public DateTime? BirthDate { get; set; }
    }
}
