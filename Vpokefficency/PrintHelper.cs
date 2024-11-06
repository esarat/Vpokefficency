using Microsoft.Extensions.Configuration;


namespace Vpokefficency
{
    public static class PrintHelper
    {
        static int tableWidth ;
        

        public static void Init(IConfigurationRoot configuration)
        {
            tableWidth = int.Parse(configuration["tableWidth"]);
        }

        public static void PrintHeader(string pokemonName , string headerType)
        {
            //print a header line at top
            PrintHelper.PrintLine();

            //Print header column names
            
            Console.WriteLine(String.Format("|{0,-10}|{1,10}|", pokemonName, headerType));

            //print a header line at bottom
            PrintHelper.PrintLine();
        }
        public static void PrintLine()
        {
            //prints a line
            Console.WriteLine(new string('-', tableWidth));
        }

        public static void PrintRow(ConsoleColor consoleColor, string typename, string catgegory)
        {
            //set text color 
            Console.ForegroundColor = consoleColor;

            //print row data
            Console.WriteLine(String.Format("|{0,-10}|{1,10}|", typename, catgegory));

            //print a line with same colr
            PrintLine();
        }
    }
}
