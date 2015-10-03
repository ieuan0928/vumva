using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Client.Communicator
{
    namespace Server
    {
        public class TestConnectionVariablesArgs : EventArgs
        {
            public bool IsTestConnectionSuccessful { get; set; }

            public BombsHost.ServerInformation ServerInformation { get; set; }
        }
    }
}
