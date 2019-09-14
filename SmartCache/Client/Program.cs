using System;
using GrainInterfaces;
using Orleans;

namespace Client
{ 
    public class Program
    {
        static void Main(string[] args)
        {
            //var clientConfig = ClientConfiguration.LocalhostSilo(30000);

            var client = new ClientBuilder().LoadConfiguration("ClientConfiguration.xml").Build();//.UseConfiguration(clientConfig).Build();

            client.Connect().Wait();

            Console.WriteLine("Client connected.");

            //GrainClient.Initialize(clientConfig);

            GrainClient.Initialize();

            var nomnioGrain = GrainClient.GrainFactory.GetGrain<ICacheGrain>("nomnio.com");

            Console.WriteLine("\n\n{0}\n\n", nomnioGrain.AddEmail("matej@nomnio.com").Result);          

            Console.WriteLine("\n\n{0}\n\n", nomnioGrain.GetEmail("matej@nomnio.com").Result);

            Console.WriteLine("\nPress Enter to terminate...");
            Console.ReadLine();
            
            client.Close();
        }
    }
}
