using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using BOMBS.Core.Log;

namespace BOMBS.Core.Resources
{
    public class Builder
    {
        public Builder(Property property)
        {
            this.property = property;

            Initialize();
        }

        public Builder(string path, string fileName)
        {
            property = new Property()
            {
                Path = path,
                FileName = fileName
            };

            Initialize();
        }

        private bool useDefaultLogger = true;

        private LogSource logSource;
        public LogSource LogSource
        {
            get { return logSource; }
            set
            {
                logSource = value;

                useDefaultLogger = logSource != null;
            }
        }

        private object sender;
        public object Sender
        {
            get { return sender; }
            set { sender = value; }
        }

        private Property property;
        public Property Property
        {
            get { return property; }
            set { property = value; }
        }

        private Dictionary<string, string> properties = new Dictionary<string, string>();
        public string this[string propertyName]
        {
            get
            {
                string result = null;
                if (properties.ContainsKey(propertyName)) result = properties[propertyName];
                return result;
            }
            set
            {
                if (properties.ContainsKey(propertyName)) properties[propertyName] = value;
                else properties.Add(propertyName, value);
            }
        }

        public Dictionary<string, string>.KeyCollection Properties
        {
            get { return properties.Keys; }
        }

        private bool isFileAvailable;
        public bool IsFileAvailable
        {
            get { return isFileAvailable; }
            set { isFileAvailable = value; }
        }

        private void Initialize()
        {
            logSource = LogController.Instance.DefaultSource;
            isFileAvailable = File.Exists(this.property.FullFilePath);

            if (isFileAvailable) Fill();
        }

        private void WriteLog(string logMessage, EventLogEntryType logEntryType = EventLogEntryType.Information)
        {
            object sender = this.sender == null ? this : this.sender;

            if (useDefaultLogger) LogController.Instance.GetDefaultLogger().LogNow(sender, logMessage, logEntryType);
            else LogController.Instance.GetLogger(logSource).LogNow(sender, logMessage, logEntryType);
        }

        public void Save(string attemptMessage, string successMessage)
        {
            WriteLog(attemptMessage);

            if (!System.IO.Directory.Exists(property.Path)) System.IO.Directory.CreateDirectory(property.Path);

            ResourceWriter writer = new ResourceWriter(property.FullFilePath);

            foreach (string key in properties.Keys) writer.AddResource(key, properties[key]);

            writer.Generate();
            writer.Close();

            WriteLog(successMessage);
        }

        public void Save()
        {
            Save(string.Format("Writing {0}...", property.DisplayName), string.Format("{0} has been written successfully...", property.DisplayName));
        }

        public bool Fill(string attemptMessage, string successMessage)
        {
            WriteLog(attemptMessage);

            if (!System.IO.File.Exists(property.FullFilePath))
            {
                WriteLog(string.Format("There is no Resource file for {0}..."), EventLogEntryType.Error);
                return false;
            }

            ResourceReader reader = new ResourceReader(property.FullFilePath);
            IDictionaryEnumerator enumerator = reader.GetEnumerator();

            while (enumerator.MoveNext())
            {
                string key = enumerator.Key.ToString();
                string value = string.IsNullOrEmpty(enumerator.Value as string) ? string.Empty : enumerator.Value as string;

                if (properties.ContainsKey(key)) properties[key] = value;
                else properties.Add(enumerator.Key.ToString(), value);
            }

            reader.Close();

            WriteLog(successMessage);

            return true;
        }

        public bool Fill()
        {
            return Fill(string.Format("Retreiving {0}...", property.DisplayName), string.Format("{0} has been retrieved successfully...", property.DisplayName));
        }
    }
}
