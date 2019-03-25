using System;
using System.IO;
using Akka.Actor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Extensions.Configuration;
using Steeltoe.Extensions.Configuration.ConfigServer;

namespace akka.cluster
{
    class Program
    {
        private static IConfiguration Config { get; set; }

        static void BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true)
                .AddConfigServer()
                .AddEnvironmentVariables();

            Config = builder.Build();
        }
        static void Main(string[] args)
        {
            BuildConfiguration();
            var clusterName = Config["clusterName"] ?? "localCluster";
            var hocon = Config["hocon"];

            var config = Akka.Configuration.ConfigurationFactory.ParseString(hocon);

            using (var system = ActorSystem.Create(clusterName, config))
            {
                Console.ReadLine();
            }
        }
    }
}
