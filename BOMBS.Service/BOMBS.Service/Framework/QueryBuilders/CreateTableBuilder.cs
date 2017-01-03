using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOMBS.Core.Framework;
using System.Reflection;

namespace BOMBS.Service.Framework.QueryBuilders
{
    public class CreateTableBuilder : BuilderBase
    {
        public CreateTableBuilder(Type targetType) : base(targetType) { }

        private string bodyString = null;
        private bool hasBody = false;
        private IdPropertyAttribute primaryKey = null;

        private string commaString(string content)
        {
            string comma = hasBody ? ", " : string.Empty;
            hasBody = true;
            return string.Format("{0}{1}", comma, content);
        }

        private string AllowNullString(bool allowNull)
        {
            return allowNull ? "NULL" : "NOT NULL";
        }

        private void GenerateFieldNameAndAllowNull(AllowNullPropertyAttribute attribute, string dbFieldType)
        {
            bodyString += commaString(string.Format("{0} {1} {2}", attribute.FieldName, dbFieldType, AllowNullString(attribute.AllowNull)));
        }

        private void GenerateIdBody(IdPropertyAttribute attribute)
        {
            bodyString += commaString(string.Format("{0} bigint NOT NULL", attribute.FieldName));
            primaryKey = attribute;
        }

        private void GenerateUniqueNameBody(UniqueNamePropertyAttribute attribute)
        {
            bodyString += commaString(string.Format("{0} varchar(max) not null", attribute.FieldName));
        }

        private void GenerateNumericBody(NumericPropertyAttribute attribute)
        {
            bodyString += commaString(string.Format("{0} numeric({1}, {2}) {3}", attribute.FieldName, attribute.Precision, attribute.Scale, AllowNullString(attribute.AllowNull)));
        }

        private void GenerateVarcharBody(VarcharPropertyAttribute attribute)
        {
            bodyString += commaString(string.Format("{0} varchar({1}) {2}", attribute.FieldName, attribute.MaxLength.ToString(), AllowNullString(attribute.AllowNull)));
        }

        public override string GenerateTSQLString()
        {
            string openString = string.Format("CREATE TABLE {0} (", coreTypeAttribute.TableName);
            string closeString = null;
            bodyString = string.Empty;

            var enumerator = propertyInfoCollection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var propertyInfo = enumerator.Current;
                FieldPropertyAttribute fieldPropertyAttribute = propertyInfo.GetCustomAttributes(typeof(FieldPropertyAttribute), false)[0] as FieldPropertyAttribute;

                if (fieldPropertyAttribute is BitPropertyAttribute) GenerateFieldNameAndAllowNull((AllowNullPropertyAttribute)fieldPropertyAttribute, "bit");
                else if (fieldPropertyAttribute is DateTimePropertyAttribute) GenerateFieldNameAndAllowNull((AllowNullPropertyAttribute)fieldPropertyAttribute, "datetime");
                else if (fieldPropertyAttribute is IdPropertyAttribute) GenerateIdBody((IdPropertyAttribute)fieldPropertyAttribute);
                else if (fieldPropertyAttribute is ForeignTypePropertyAttribute) GenerateFieldNameAndAllowNull((AllowNullPropertyAttribute)fieldPropertyAttribute, "bigint");
                else if (fieldPropertyAttribute is NumericPropertyAttribute) GenerateNumericBody((NumericPropertyAttribute)fieldPropertyAttribute);
                else if (fieldPropertyAttribute is TextPropertyAttribute) GenerateFieldNameAndAllowNull((AllowNullPropertyAttribute)fieldPropertyAttribute, "text");
                else if (fieldPropertyAttribute is UniqueNamePropertyAttribute) GenerateUniqueNameBody((UniqueNamePropertyAttribute)fieldPropertyAttribute);
                else if (fieldPropertyAttribute is VarcharPropertyAttribute) GenerateVarcharBody((VarcharPropertyAttribute)fieldPropertyAttribute);
            }

            if (primaryKey != null)
            {
                string constraintName = string.Format("{0}_{1}_PK", coreTypeAttribute.TableName, primaryKey.FieldName);
                bodyString += commaString(string.Format("CONSTRAINT {0} PRIMARY KEY CLUSTERED ( {1} ASC ) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]", constraintName, primaryKey.FieldName));
                closeString = ") ON [PRIMARY]";
            }
            else closeString = ")";

            return string.Format("{0}{1}{2}", openString, bodyString, closeString);
        }
    }
}
