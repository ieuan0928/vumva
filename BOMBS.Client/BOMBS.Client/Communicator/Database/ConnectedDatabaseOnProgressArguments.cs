using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Client.Communicator.Database
{
    public delegate void ConnectDatabaseOnProgressHandler(object sender, ConnectedDatabaseOnProgressArguments e);

    public class ConnectedDatabaseOnProgressArguments : System.EventArgs
    {
        public BombsHost.DatabaseInformation CurrentDatabaseInformation { get; set; }

        public BombsHost.DatabaseStatus OldStatus { get; set; }
        public BombsHost.DatabaseStatus NewStatus { get; set; }
    }
}
