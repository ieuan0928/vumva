using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


using BOMBS.Core.Helper;
using BOMBS.Core.Common.Handlers;
using Resources = BOMBS.Core.Resources;
using System.ServiceModel;


namespace BOMBS.Service.Database
{
    [DataContract(Name = "DatabaseInformation")]
    public class Information
    {
        public enum ConnectionStringType
        {
            Default,
            UseMaster,
            EmptyCatalog
        }

        private const string resFile = "BOMBS.Database.Configuration.Settings.Resources";
        public Information()
        {
            resourceBuilder = Resources.Helper.Instance[ApplicationResourceDirectory.DefaultFolderName, resFile];

            resourceBuilder.Sender = this;

            if (resourceBuilder.IsFileAvailable)
            {
                ReadResources();
                status = Database.Status.ConfigurationRequiresValidation;
            }
            else status = Database.Status.RequiresConfiguration;
        }

        private Resources.Builder resourceBuilder;

        public event NewValueHandler StatusOnChanged;
        private void OnStatusChanged(Status newValue, Status oldValue)
        {
            if (StatusOnChanged != null)
                StatusOnChanged(this, new NewValueArgs("Status")
                {
                    NewValue = newValue,
                    OldValue = oldValue
                });
        }

        private void ReadResources()
        {
            resourceBuilder.Fill("Retreiving Database Configuration File.", "Database Configuration File has been retrieved successfully.");

            sqlServerOption = (SQLServerOption)Enum.Parse(typeof(SQLServerOption), resourceBuilder["SQLServerOption"], true);
            databaseName = resourceBuilder["DatabaseName"];
            serverInstanceName = resourceBuilder["ServerInstanceName"];
            sqlServerPortString = resourceBuilder["SQLServerPortString"];
            serverAddress = resourceBuilder["ServerAddress"];
            databaseAuthentication = (Authentication)Enum.Parse(typeof(Authentication), resourceBuilder["DatabaseAuthentication"], true);
            userName = resourceBuilder["UserName"];
            password = SecureString.Decrypt(resourceBuilder["Password"]);
            serverConnectionConfiguration = resourceBuilder["ServerConnectionConfiguration"];
            status = (Status)Enum.Parse(typeof(Status), resourceBuilder["Status"], true);

        }

        public void WriteResources()
        {
            resourceBuilder["SQLServerOption"] = sqlServerOption.ToString();
            resourceBuilder["DatabaseName"] = databaseName;
            resourceBuilder["ServerInstanceName"] = serverInstanceName;
            resourceBuilder["SQLServerPortString"] = sqlServerPortString;
            resourceBuilder["ServerAddress"] = serverAddress;
            resourceBuilder["DatabaseAuthentication"] = databaseAuthentication.ToString();
            resourceBuilder["UserName"] = userName;
            resourceBuilder["Password"] = SecureString.Encrypt(password);
            resourceBuilder["ServerConnectionConfiguration"] = serverConnectionConfiguration;
            resourceBuilder["Status"] = status.ToString();

            resourceBuilder.Save("Creating Database Configuration File.", "Database Configuration File has been created.");
        }

        internal void Populate(Information targetObject)
        {
            targetObject.DatabaseAuthentication = this.DatabaseAuthentication;
            targetObject.DatabaseName = this.DatabaseName;
            targetObject.Password = this.Password;
            targetObject.ServerAddress = this.ServerAddress;
            targetObject.ServerInstanceName = this.ServerInstanceName;
            targetObject.Status = this.status;
            targetObject.UserName = this.UserName;
        }

        private SQLServerOption sqlServerOption = SQLServerOption.UseLocalhost;
        [DataMember]
        public SQLServerOption SqlServerOption
        {
            get { return sqlServerOption; }
            set { sqlServerOption = value; }
        }

        private string databaseName = string.Empty;
        [DataMember]
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        private string serverInstanceName = string.Empty;
        [DataMember]
        public string ServerInstanceName
        {
            get { return serverInstanceName; }
            set { serverInstanceName = value; }
        }

        private string sqlServerPortString = string.Empty;
        [DataMember]
        public string SQLServerPortString
        {
            get { return sqlServerPortString; }
            set { sqlServerPortString = value; }
        }

        private string serverAddress = string.Empty;
        [DataMember]
        public string ServerAddress
        {
            get { return serverAddress; }
            set { serverAddress = value; }
        }

        private Authentication databaseAuthentication = Authentication.Windows;
        [DataMember]
        public Authentication DatabaseAuthentication
        {
            get { return databaseAuthentication; }
            set { databaseAuthentication = value; }
        }

        private string userName = string.Empty;
        [DataMember]
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string password = string.Empty;
        [DataMember]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string serverConnectionConfiguration = string.Empty;
        [DataMember]
        public string ServerConnectionConfiguration
        {
            get { return serverConnectionConfiguration; }
            set { serverConnectionConfiguration = value; }
        }

        private Status status = Status.Ready;
        [DataMember]
        public Status Status
        {
            get { return status; }
            set
            {
                Status oldValue = status;
                Status newValue = value;

                if (oldValue != newValue)
                {
                    status = newValue;
                    OnStatusChanged(newValue, oldValue);
                }
            }
        }

        public string GenerateConnectionString(ConnectionStringType connectionStringType = ConnectionStringType.Default)
        {
            string server = string.Format(@"Data Source={0}{1}\{2}", serverAddress,
                sqlServerPortString.Trim().Length > 0 ? string.Format(",{0}", sqlServerPortString.Trim()) : string.Empty,
                serverInstanceName.Trim().Length > 0 ? serverInstanceName.Trim() : string.Empty);

            string database = string.Empty;

            if (connectionStringType != ConnectionStringType.EmptyCatalog)
                database = string.Format(@"Initial Catalog={0}", connectionStringType == ConnectionStringType.UseMaster ? "master" : databaseName);

            string authentication = string.Empty;

            if (databaseAuthentication == Authentication.Windows) authentication = "Integrated Security=True";
            else authentication = string.Format(@"User Id={0}; Password={1}", userName, password);

            return string.Format("{0};{1};{2};", server, database, authentication);
        }

        public Information Duplicate()
        {
            Information result = new Information()
            {
                databaseAuthentication = databaseAuthentication,
                databaseName = databaseName,
                password = password,
                serverAddress = serverAddress,
                serverConnectionConfiguration = serverConnectionConfiguration,
                serverInstanceName = serverInstanceName,
                sqlServerOption = sqlServerOption,
                status = status,
                userName = userName
            };


            return result;
        }
    }
}