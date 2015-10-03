using System;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Net;

using Database = BOMBS.Service.Database;

namespace BOMBS.Service.Server
{
    [DataContract(Name = "ServerInformation")]
    public class Information
    {
        public Information()
        {
            computerName = Environment.MachineName;
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();

            hostName = Dns.GetHostName();
            domainName = ipProperties.DomainName;

            if (string.IsNullOrEmpty(domainName)) fullComputerName = hostName;
            else fullComputerName = string.Format("{0}.{1}", hostName, domainName);

            ipAddress = Dns.GetHostEntry(hostName).AddressList[1].ToString();
        }

        private string IpAdresses(string hostName)
        {
            IPAddress[] ipAddressCollection = Dns.GetHostEntry(hostName).AddressList;

            string result = ipAddressCollection[0].ToString().Trim();

            for (int i = 1; i < ipAddressCollection.Length; i++)
            {
                if (result.Length > 0)
                    result = string.Format(", {0}", ipAddressCollection[i].ToString().Trim());
                else result = ipAddressCollection[i].ToString().Trim();
            }

            return result;
        }

        private string computerName = string.Empty;
        private string fullComputerName = string.Empty;
        private string hostName = string.Empty;
        private string domainName = string.Empty;
        private string ipAddress = string.Empty;
        private Database.Information databaseInformation = new Database.Information();
        private InitializationActivityStatusEnum initializationActivityStatus = InitializationActivityStatusEnum.Ready;

        [DataMember]
        public string ComputerName
        {
            get { return computerName; }
            set { computerName = value; }
        }

        [DataMember]
        public string FullComputerName
        {
            get { return fullComputerName; }
            set { fullComputerName = value; }
        }

        [DataMember]
        public string HostName
        {
            get { return hostName; }
            set { hostName = value; }
        }

        [DataMember]
        public string DomainName
        {
            get { return domainName; }
            set { domainName = value; }
        }

        [DataMember]
        public string IPAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        [DataMember]
        public Database.Information DatabaseInformation
        {
            get { return databaseInformation; }
            set { databaseInformation = value; }
        }

        [DataMember]
        public InitializationActivityStatusEnum InitializationActivityStatus
        {
            get { return initializationActivityStatus; }
            set { initializationActivityStatus = value; }
        }


    }
}
