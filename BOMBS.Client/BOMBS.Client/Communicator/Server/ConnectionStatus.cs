using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Client.Communicator
{
    namespace Server
    {
        public enum ConnectionStatus
        {
            NotFetched,
            Success,
            ResourceFileNotFound,
            ConnectionFailure
        }
    }
}
