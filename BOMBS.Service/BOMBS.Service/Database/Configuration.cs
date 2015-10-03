using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Resources = BOMBS.Core.Resources;

namespace BOMBS.Service.Database
{
    public class Configuration
    {
        private string resourcePath = "BOMBS";
        private string resourceFileName = "BOMBS.Service.Config.Database.Resources";
        private Resources.Builder resourceBuilder;

        private Configuration()
        {
            resourceBuilder = Resources.Helper.Instance[resourcePath, resourceFileName];
        }

        private static Configuration instance;
        public static Configuration Instance
        {
            get
            {
                if (instance == null) instance = new Configuration();

                return instance;
            }
        }

        public Status Validate(ref Information databaseInformation)
        {
            if (!resourceBuilder.IsFileAvailable)
            {
                databaseInformation.Status = Status.RequiresConfiguration;
                return databaseInformation.Status;
            }

            return databaseInformation.Status;
        }

    }
}
