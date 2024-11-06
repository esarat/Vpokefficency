using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VpokefficencyGateway;
using VpokefficencyService;
using Newtonsoft.Json;
using System.ComponentModel;
using VpokefficencyFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VpokefficencyService;
using System.Threading.Tasks;
using VpokefficencyDomain;
using System.Collections.Generic;
using System;
using VpokefficencyGateway;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration;

namespace VpokefficencyBootstrap
{
    public static  class Bootstrap
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static ServiceProvider ServiceProvider { get; set; }
        public static void Configure() {
            //setup logging and  DI
            ServiceProvider = new ServiceCollection()
                 .AddLogging(builder => builder.AddConsole())
                 .AddSingleton<IPokemonService, PokemonService>()
                 .AddSingleton<IApiGateway, RestApiGateway>()
                 .BuildServiceProvider();

          
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
           
        }

    }
}
