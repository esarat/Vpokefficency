using Microsoft.Extensions.Logging;
using VpokefficencyService;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace VpokefficencyTest
{
    public class PokemonServiceTest
    {
        private IPokemonService _service;
        int normalPokemonTypeEffectivnessListLength = 4;
        //private ILogger<PokemonService> _logger;
         public PokemonServiceTest(IPokemonService service)
        {
            //setup service
         
            _service = service;
            
            //other setup and mocks init goes here
        }
        [Fact]
        public async void GetPokemonTypes_NullTest()
        {
           //null test
           var ptypes = await _service.GetTypeEffectiveness("");
           Assert.Null(ptypes);
        }

        [Fact]
        public async void GetPokemonTypes_normalPokemonTypeValidTest()
        {
            //Known input time tested output
            var telist = await _service.GetTypeEffectiveness("linoone");
            Assert.Equal(telist.Count, normalPokemonTypeEffectivnessListLength);
        }
    }
}