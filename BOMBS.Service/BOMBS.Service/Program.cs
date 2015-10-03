using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Service...");
            Console.WriteLine();
            Console.WriteLine();
            Controller.Communicator.Start();
            Console.WriteLine("Service Started Successfully...");
            Console.WriteLine();
            Console.Write("Press any key to stop service..");
            Console.ReadKey();

            Console.WriteLine();
            Console.WriteLine("Stopping Service...");
            Controller.Communicator.Stop();
            Console.WriteLine("Service Stopped successfully...");

            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
