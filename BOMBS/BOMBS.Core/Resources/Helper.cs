using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Core
{
    namespace Resources
    {
        public class Helper
        {
            private Helper()
            {
                collection = new Dictionary<Property, Builder>();
            }

            private static Helper instance;
            public static Helper Instance
            {
                get
                {
                    if (instance == null) instance = new Helper();

                    return instance;
                }
            }

            private Dictionary<Property, Builder> collection;

            public Builder this[string path, string fileName]
            {
                get
                {
                    string fullFilePath = string.Format(@"{0}\{1}", path, fileName);
                    Property key = collection.Keys.FirstOrDefault(itm => itm.FullFilePath.Trim() == fullFilePath.Trim());

                    if (key == null)
                    {
                        key = new Property() { FileName = fileName, Path = path };

                        collection.Add(key, new Builder(key));
                    }

                    return collection[key];
                }
            }

            public string this[string path, string fileName, string propertyName]
            {
                get
                {
                    return this[path, fileName][propertyName];
                }
                set
                {
                    this[path, fileName][propertyName] = value;
                }
            }
        }
    }
}
