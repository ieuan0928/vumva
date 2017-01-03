using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using BOMBS.Core.Framework;
using System.Data.SqlClient;

namespace BOMBS.Service.Framework
{
    public class Operations
    {
        private Operations() { }

        private static Operations instance = null;
        public static Operations Instance
        {
            get
            {
                if (instance == null) instance = new Operations();
                return instance;
            }
        }

        internal bool ImportAssembly(Assembly assembly, string dbConnectionString)
        {
            bool result = false;

            IEnumerable<Type> entityTypes = assembly.GetTypes().Where<Type>(itm => itm.GetCustomAttributes(typeof(CoreTypeAttribute), false).Length > 0);

            if (entityTypes.Count() > 0)
            {
                using (SqlConnection connection = new SqlConnection(dbConnectionString))
                {
                    connection.Open();

                    SqlTransaction transaction = connection.BeginTransaction();
                    SqlCommand command = connection.CreateCommand();
                    command.Transaction = transaction;

                    IEnumerator<Type> enumerator = entityTypes.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        Type entity = enumerator.Current;
                        QueryBuilders.CreateTableBuilder builder = new QueryBuilders.CreateTableBuilder(entity);
                        command.CommandText = builder.GenerateTSQLString();
                        command.ExecuteNonQuery();
                    }

                    try
                    {
                        transaction.Commit();
                        result = true;
                    }
                    catch { transaction.Rollback(); }

                    connection.Close();
                }
            }

            return result;
        }

        internal bool DiscoverAssembly(Assembly assembly, string dbConnectionString)
        {
            bool result = false;

            IEnumerable<Type> entityTypes = assembly.GetTypes().Where(itm => itm.GetCustomAttributes(typeof(CoreTypeAttribute), false).Length > 0);

            if (entityTypes.Count() > 0)
            {
                using (SqlConnection connection = new SqlConnection(dbConnectionString))
                {
                    connection.Open();

                    SqlTransaction transaction = connection.BeginTransaction();
                    SqlCommand command = connection.CreateCommand();
                    command.Transaction = transaction;

                    foreach (Type entityType in entityTypes)
                    {
                    }

                }

            }

            return result;
        }
    }
}
