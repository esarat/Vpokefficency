using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VpokefficencyDomain;
using VpokefficencyFramework;
using VpokefficencyGateway;

namespace VpokefficencyService
{
    public class PokemonService : IPokemonService
    {
        private readonly ILogger _logger;
        private IApiGateway _apiGateway;
        public PokemonService(ILogger<PokemonService> logger , IApiGateway apiGateway)
        {
              _logger = logger; 
              _apiGateway = apiGateway;
        
        }
        private async Task<List<string>> GetPokemonTypes(string pokemonName)
        {
            
            if (_logger != null)
               _logger.LogInformation("V GetPokemonTypes is called");

            //check for null
            if (string.IsNullOrEmpty(pokemonName))
                 return null;

            //Init setup
            List<string> pokemonTypes = new List<string>();
            string url = $"https://pokeapi.co/api/v2/pokemon/{pokemonName}";

            try
            {
                //Get pokemon types Json from API
                ResponseBody responseBody = await _apiGateway.GetJsonResponse(url);
                
                //check status before opening the payload
                if (responseBody.status == "Exception")
                {
                    //log and throw ex
                    Console.WriteLine($"Exception occured when getting pokemon  types for  {pokemonName} " + responseBody.PayLoad.ToString());
                    throw new Exception(responseBody.PayLoad.ToString());
                }
                else if (responseBody.status == "Error")
                {
                    //log and return null
                    Console.WriteLine($"Error occured when getting pokemon  types for  {pokemonName} " + responseBody.PayLoad.ToString());
                    return null;
                }
                else
                {
                    //Success 
                    dynamic pokemonData = JsonConvert.DeserializeObject(responseBody.PayLoad.ToString());

                    //get types
                    var pktypes = pokemonData.types;

                    //loop through types and make a list
                    foreach (var pktype in pktypes)
                    {
                        string typeName = pktype.type.name;
                        pokemonTypes.Add(typeName);
                    }
                }
                return pokemonTypes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return null;
            }
            
        }
        private async Task<List<TypeEffectiveness>> GetDamageRelationsForType(string typeName)
        {
            //check for null
            if (string.IsNullOrEmpty(typeName))
                return null;

            //Init setup
            List<TypeEffectiveness> typeEffectivenessList = new List<TypeEffectiveness>() { };
            string url = $"https://pokeapi.co/api/v2/type/{typeName}";


            try
            {
                //Get relations Json from API
                ResponseBody responseBody = await _apiGateway.GetJsonResponse(url);

                //check status before opening the payload
                if (responseBody.status == "Exception")
                {
                    //log and throw ex
                    Console.WriteLine($"Exception occured when getting relations for  {typeName} " + responseBody.PayLoad.ToString());
                    throw new Exception(responseBody.PayLoad.ToString());
                }
                else if (responseBody.status == "Error")
                {
                    //log and return null
                    Console.WriteLine($"Error occured when getting Damage Relations  for  {typeName} " + responseBody.PayLoad.ToString());
                    return null;
                }
                else
                {
                    //success
                    dynamic typeData = JsonConvert.DeserializeObject(responseBody.PayLoad.ToString());

                    //get relations
                    var damageRelations = typeData.damage_relations;

                    //loop through relations
                    foreach (var relation in damageRelations)
                    {
                        //loop through types in a relation amd make a list of TypeEffectiveness objects
                        foreach (var ptype in relation.Value)
                        {
                            TypeEffectiveness te = null;
                            if (relation.Name == "double_damage_to" || relation.Name == "half_damage_from" || relation.Name == "no_damage_from")
                            {
                                //Create a TypeEffectiveness object -- strong
                                te = new TypeEffectiveness { IsStrong = true, Name = ((JContainer)ptype).First.First.ToString() };
                            }
                            if (relation.Name == "double_damage_from" || relation.Name == "half_damage_to" || relation.Name == "no_damage_to")
                            {
                                //Create a TypeEffectiveness object -- weak
                                te = new TypeEffectiveness { IsWeak = true, Name = ((JContainer)ptype).First.First.ToString() };
                            }

                            //finally add to list
                            if (te != null)
                                typeEffectivenessList.Add(te);
                        }
                    }

                    return typeEffectivenessList;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        
        private static List<TypeEffectiveness> Compaction(List<TypeEffectiveness> typeEffectivenesslist)
        {
            //This helps compress objects that are ambigous ( both strong and weak ) 
            //Objects that are ambiguous have same name but they have different strong and is weak properties
            //for instance { Name = poke-dubious , isStrong = True } and { Name = poke-dubious ,  }
            //so they are not duplicates they appear twice in the list
            //we need to combine them into one object that is both strong and weak
            //{ Name = poke-dubious , isStrong = True , isWeak = True}
            //we can accomplish that by grouping them by name and picking strong and weak flags by OR ing all items in group

            return typeEffectivenesslist
                        .GroupBy(x => x.Name)
                        .Select(g => new TypeEffectiveness
                        {
                            Name = g.Key,
                            IsStrong = g.Any(x => x.IsStrong),
                            IsWeak = g.Any(x => x.IsWeak)
                        })
                        .ToList();
        }
        public async Task<List<TypeEffectiveness>> GetTypeEffectiveness(string pokemonName)
        {
            List<TypeEffectiveness> typeEffectivenesslist = new List<TypeEffectiveness>() { };

            //Gwt types for the input pokemon
            List<string> pokemonTypes = await GetPokemonTypes(pokemonName);

            //Get DamageRelations For all Types
            if (pokemonTypes != null)
            {
                var tasks = pokemonTypes.Select(async typeName =>
                {
                    //Get DamageRelations For the current Type
                    var result = await GetDamageRelationsForType(typeName);

                    
                    lock (typeEffectivenesslist)
                    {
                        //add to list while removing duplicates from previous types
                        if (typeEffectivenesslist.Count > 0)
                        {
                            typeEffectivenesslist.Union(result);
                        }
                        else
                        {
                            typeEffectivenesslist.AddRange(result);
                        }
                    }
                });

                //wait for all tasks to finish before compressing
                await Task.WhenAll(tasks);

                //Compress and merge 
                List<TypeEffectiveness> compressedList = Compaction(typeEffectivenesslist);
                return compressedList;
            }
            else
            {
                //pokemon types is null
                return null;
            }
        }
    }
}
