using NUnitLite;
using System;

namespace CodingArchitect.Spikes.NH
{
    public class Program
    {
        /// <summary>
        /// The main program executes the tests. Output may be routed to
        /// various locations, depending on the arguments passed.
        /// </summary>
        /// <remarks>Run with --help for a full list of arguments supported</remarks>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            new AutoRun().Execute(args);
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}