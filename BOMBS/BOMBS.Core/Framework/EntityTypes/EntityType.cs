using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOMBS.Core.Framework;
using BOMBS.Core.Framework.DataObjects;

namespace BOMBS.Core.Framework.EntityTypes
{
    public abstract class EntityType : IDataCommunicator
    {
        public EntityType()
        {
            Type myType = this.GetType();

            className = myType.FullName;
            this[frameworkNameProperty] = myType.FullName;
            assemblyName = myType.Assembly.FullName;
        }

        public EntityType(Dictionary<string, object> senderPropertyMap)
            : this()
        {
            propertyMap = senderPropertyMap;
        }

        private Dictionary<string, object> propertyMap = null;
        protected object this[string key]
        {
            get
            {
                if (propertyMap == null) return null;

                return propertyMap[key];
            }
            set
            {
                if (propertyMap == null) propertyMap = new Dictionary<string, object>();

                if (propertyMap.ContainsKey(key))
                    propertyMap[key] = value;
                else propertyMap.Add(key, value);
            }
        }

        private const string idProperty = "Id";
        [IdProperty(FieldName = idProperty, DisplayName = "ID", Description = "Framework ID")]
        public int Id
        {
            get { return (int)this[idProperty]; }
            protected set { this[idProperty] = value; }
        }

        private const string frameworkNameProperty = "FrameworkName";
        [UniqueNameProperty(FieldName = frameworkNameProperty, DisplayName = "BOMBS Identifier", Description = "BOMBS Unique Framework Name")]
        public string FrameworkName
        {
            get { return (string)this[frameworkNameProperty]; }
            protected set { this[FrameworkName] = value; }
        }

        private const string dateCreatedProperty = "DateCreated";
        [DateTimeProperty(AllowNull = false, DisplayName = "Date Created", FieldName = dateCreatedProperty)]
        public DateTime? DateCreated
        {
            get { return (DateTime?)this[dateCreatedProperty]; }
            set { this[dateCreatedProperty] = value; }
        }

        private string assemblyName = null;
        public string AssemblyName
        {
            get { return assemblyName; }

        }

        private string className = null;
        public string ClassName
        {
            get { return className; }
            protected set { className = value; }
        }
    }
}
