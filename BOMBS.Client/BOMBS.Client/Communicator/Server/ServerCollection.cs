using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BOMBS.Core.Helper;

using BResources = BOMBS.Core.Resources;
using BSerialization = BOMBS.Core.Serialization;
using System.Collections.ObjectModel;

namespace BOMBS.Client.Communicator.Server
{
    public class ServerCollection : ObservableCollection<KeyValuePair<string, ServerVariables>>
    {
        private const string resRegsteredServerFile = @"BOMBS.Client.Properties.Local.Registered.Servers.Resources";

        public ServerCollection()
        {
            localPath = ApplicationResourceDirectory.LocalApplicationData;

            rscBuilder = BResources.Helper.Instance[localPath, resRegsteredServerFile];

            ReadResources();
        }

        private string localPath;
        private BResources.Builder rscBuilder;

        private List<string> keys = new List<string>();
        public IList<string> Keys
        {
            get
            {
                return keys;
            }
        }


        public ServerVariables this[string key]
        {
            get
            {
                return this.FirstOrDefault(itm => itm.Key == key).Value;
            }
            set
            {
                KeyValuePair<string, ServerVariables> result = this.FirstOrDefault(itm => itm.Key == key);

                if (result.Key == null)
                {
                    keys.Add(key);
                    this.Add(new KeyValuePair<string, ServerVariables>(key, value));
                    return;
                }
                if (result.Value == null)
                {
                    this.Remove(result);
                    this.Add(new KeyValuePair<string, ServerVariables>(key, value));
                    return;
                }

                result.Value.Address = value.Address;
                result.Value.Information = value.Information;
                result.Value.Port = value.Port;
                //result.Value.ServerConfigurationStatus = value.ServerConfigurationStatus;
            }
        }

        public bool ReadResources()
        {
            if (!rscBuilder.IsFileAvailable) return false;

            var enumerator = rscBuilder.Properties.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var key = enumerator.Current;
                this[key] = BSerialization.XMLHelper.DeserializeFromXMLString<ServerVariables>(rscBuilder[key]);
            }

            return true;
        }


        public void WriteResources()
        {
            var enumerator = keys.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var svar = enumerator.Current;
                string result = BSerialization.XMLHelper.SerializeToXMLString<ServerVariables>(this[svar]);
                rscBuilder[svar] = result;
            }

            rscBuilder.Save();
        }

    }
}
