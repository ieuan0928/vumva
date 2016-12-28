using BOMBS.Core.Framework.EntityTypes;
using BOMBS.Core.Helper;
using BOMBS.Service.Framework;
using Microsoft.SqlServer.Management.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ServiceFramework = BOMBS.Service.Framework;

namespace BOMBS.Experiments.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            //string encryptString = SecureString.Encrypt("MyPassword");
            //string decryptString = SecureString.Decrypt(encryptString);

            using (SqlConnection connection = new SqlConnection(@"Data Source=localhost\MSSQL2008E;Initial Catalog=master;Integrated Security=True"))
            {
                try
                {
                    connection.Open();
                   // result = ConfigurationSteps.InstanceConnectionSuccess;
                    connection.Close();
                }
                catch (Exception ex)
                {
                    //result = ConfigurationSteps.InstanceFailure;
                }
            }
        }
    }
}
