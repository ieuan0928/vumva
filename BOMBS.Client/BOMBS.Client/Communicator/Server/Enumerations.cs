using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Client.Communicator.Server
{
    public enum ServerConfigurationStatus : byte
    {
        Unset = 0,
        Active = 1,
        NoConnection = 2
    }
}
