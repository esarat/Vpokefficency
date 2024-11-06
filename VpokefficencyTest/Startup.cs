using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VpokefficencyGateway;
using VpokefficencyService;

namespace VpokefficencyTest
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IPokemonService, PokemonService>();
            services.AddSingleton<IApiGateway,RestApiGateway>();
        }
    }
}
