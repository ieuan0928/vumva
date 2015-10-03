using BOMBS.Core.Framework.EntityTypes;
using BOMBS.Core.Helper;
using BOMBS.Service.Framework;
using Microsoft.SqlServer.Management.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFramework = BOMBS.Service.Framework;

namespace BOMBS.Experiments.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string encryptString = SecureString.Encrypt("MyPassword");
            string decryptString = SecureString.Decrypt(encryptString);
        }
    }
}
