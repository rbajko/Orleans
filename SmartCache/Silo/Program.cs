using System;
using Orleans.Runtime.Host;

namespace Silo
{
    public class Program
    {
        static void Main(string[] args)
        {
            var silo = new SiloHost("Silo");
            silo.ConfigFileName = "SiloConfiguration.xml";

            silo.InitializeOrleansSilo();
            var startSuccessful = silo.StartOrleansSilo();

            if (!startSuccessful)
            {
                throw new SystemException(String.Format("Failed to start Orleans silo '{0}' as a {1} node", silo.Name, silo.Type));
            }

            Console.WriteLine("\nPress Enter to terminate...");
            Console.ReadLine();
    
            silo.ShutdownOrleansSilo();     
        }
    }
}
