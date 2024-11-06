using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VpokefficencyBootstrap;
using VpokefficencyDomain;
using VpokefficencyFramework;
using VpokefficencyService;



namespace Vpokefficency

{

    public class Program
    {        
        static async Task Main(string[] args)
        {
            Console.WriteLine("VVV!");
           
            //Setup 
            Console.ForegroundColor = ConsoleColor.White;


            //Bootstrap
            Bootstrap.Configure();
            var serviceProvider = Bootstrap.ServiceProvider;
            PrintHelper.Init(Bootstrap.Configuration);



            //Get valid pokeman name from user
            string pokemonName = AcceptOnlyValidInput();

            while (pokemonName != "quit")
            {
                Console.WriteLine("You entered a Pokemon name: " + pokemonName);
                
                //create a pokemon service
                var pokemonService = serviceProvider.GetService<IPokemonService>();

                //Get type effectiveness for the pokemon
                List<TypeEffectiveness> compressedList = await pokemonService.GetTypeEffectiveness(pokemonName);

                //Display the results to user
                if (compressedList != null) 
                    DisplayResults(compressedList, pokemonName);                

                //Rinse and repeat
                pokemonName = "";
                compressedList = null;
                pokemonName = AcceptOnlyValidInput();
            }

        }

        public static string AcceptOnlyValidInput()
        {
            Console.Write("Please Enter a Pokemon name: ");

            string pokemonName = Console.ReadLine();

            //check for empty string
            if (string.IsNullOrEmpty(pokemonName))
            {
                Console.WriteLine("You entered a null or empty Pokemon name: ");
                return AcceptOnlyValidInput();
            }

            //trimming
            pokemonName = pokemonName.TrimAndRemoveWhiteSpace();

            //make sure it has just letters
            if (!pokemonName.HasJustletters())
            {
                Console.WriteLine("You entered an invalid Pokemon name: Only letters are allowed  ");
                return AcceptOnlyValidInput();
            }

            // change it to lower case to avoid errors when making API calls
            return pokemonName.ToLower();
        }
       
        public static void DisplayResults(List<TypeEffectiveness> compressedList, string pokemonName)
        {
            //print header
            PrintHelper.PrintHeader(pokemonName, "Efficency");
           
            //print rows
            foreach (var item in compressedList.Where(x => x.IsStrong && !x.IsWeak))
            {
                PrintHelper.PrintRow(ConsoleColor.Green, item.Name, "Strong");               
            }
            
            foreach (var item in compressedList.Where(x => x.IsWeak && !x.IsStrong))
            {
                PrintHelper.PrintRow(ConsoleColor.Red, item.Name, "Weak");                
            }
            
            foreach (var item in compressedList.Where(x => x.IsStrong && x.IsWeak))
            {
                PrintHelper.PrintRow(ConsoleColor.Yellow, item.Name, "Ambigous");  
              
            }

            //Set console color back to white
            Console.ForegroundColor = ConsoleColor.White;            

        } 

    }
}
