using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VpokefficencyDomain;

namespace VpokefficencyService
{
    public interface IPokemonService
    {
         public Task<List<TypeEffectiveness>> GetTypeEffectiveness(string pokemonName);
    }


}
