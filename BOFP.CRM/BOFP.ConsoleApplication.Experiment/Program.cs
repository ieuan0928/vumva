using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using AmountInWords = BOFP.ClientRelationManagement.Custom.AmountInWords;

namespace BOFP.ConsoleApplication.Experiment
{
    class Program
    {
        static void Main(string[] args)
        {
            decimal shit = 40456789021.88M;

            Console.WriteLine(shit.ToString("#,##0.00##############"));
            Console.WriteLine("Result ---------------------------");

            Console.Write(AmountInWords.Helper.Generate(shit));
            Console.ReadKey();
        }
    }
}
