namespace MatrixRain
{
    class Program
    {
        static void Main(string[] args)
        {
            bool direction = false;         
            var color = ConsoleColor.Green; 
            int delay = 1;                  
            char characters = 'x';          
            bool randomChars = false;       

            /*
             * Prejdenie vsetkych argumentov a nastavenie atributov programu
             */
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Contains("--direction-up"))
                {
                    direction = true;
                }
                if (args[i].Contains("--color"))
                {
                    var chosen_color = args[i + 1];
                    switch (chosen_color)
                    {
                        case "Red": color = ConsoleColor.Red; break;
                        case "Blue": color = ConsoleColor.Blue; break;
                        case "Magenta": color = ConsoleColor.Magenta; break;
                        case "Cyan": color = ConsoleColor.Cyan; break;
                        case "Yellow": color = ConsoleColor.Yellow; break;
                        case "Green": color = ConsoleColor.Green; break;
                        default: break;
                    }
                }
                if (args[i].Contains("--delay-speed"))
                {
                    int value = Int32.Parse(args[i + 1]);
                    if (value < 0) // Ak je zadana hodnota zaporna, automaticky sa nastavi default 1
                    {
                        continue;
                    }
                    delay = value;
                }
                if (args[i].Contains("--characters"))
                {
                    var chosen_characters = args[i + 1];
                    switch (chosen_characters)
                    {
                        case "Alpha":
                            characters = 'a';
                            break;
                        case "Numeric":
                            characters = 'n';
                            break;
                        default:
                            break;
                    }
                }
                if (args[i].Contains("--random-colors"))
                {
                    color = ConsoleColor.Gray;  // Vyuzita gray farba pre jednoduchsi prenos do triedy MatrixRain
                }
                if (args[i].Contains("--random-chars"))
                {
                    randomChars = true;
                }
                if (args[i].Contains("--help") || args[i].Contains("-h") || args[i].Contains("-?"))
                {
                    Console.WriteLine("Description:");
                    Console.WriteLine("\tMatrix digital rain - a simplified version of the falling code of letters representing " +
                        "the activity of the simulated reality environment from the Matrix movie.");
                    Console.WriteLine();
                    Console.WriteLine("Usage:");
                    Console.WriteLine("\tMatrixRain [options]");
                    Console.WriteLine();
                    Console.WriteLine("Options:");
                    Console.WriteLine("\t--direction-up\t\t\tDirection of falling code up [default: False]");
                    Console.WriteLine("\t--color\t\t\t\tColor of falling code up [default: Green]");
                    Console.WriteLine("\t    <Red|Blue|Magenta|Cyan|Yellow|Green>");
                    Console.WriteLine("\t--delay-speed <delay-speed>\tDelay speed of falling code in miliseconds [default: 1]");
                    Console.WriteLine("\t--characters <Alpha|AlphaNumeric|Numeric>\tThe set of characters from which the falling code will be generated [default: AlphaNumeric]");
                    Console.WriteLine("\t--random-chars\t\t\tEach drop will have random chars for every iteration.");
                    Console.WriteLine("\t--random-colors\t\t\tEach drop will have random color for every iteration.");
                    Console.WriteLine("\t--help, -h, -?\t\t\tShow help and usage information");
                    break;
                }
            }
            MatrixRain rain = new(direction, color, delay, characters, randomChars);
            rain.Matrix();
        }
    }
}