using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace BOMBS.ServiceServer
{
    static class Program
    {
        static void Main(string[] args)
        {
            ServiceBase[] ServicesToRun;

            ServicesToRun = new ServiceBase[] 
            { 
                new BombsService() 
            };


            ServiceBase.Run(ServicesToRun);
        }
    }
}
