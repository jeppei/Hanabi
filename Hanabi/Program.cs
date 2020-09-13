using System;
using System.Linq;
using static Hanabi.Game;

namespace Hanabi {

    class Program {
        
        static readonly int[] results = new int[26];
        public static Game game;
        
        static void Main() {
 
            Console.Write("[1                      50                      100]\n ");

            for (int i = 0; i < iterations; i++) {

                PrintProgress(i);

                game = new Game();
                game.Play();

                results[score]++;
            }

            game.history.PrintToConsole(); // Print the last game
            PrintStatistics();
        }

        private static void PrintStatistics() {
            Console.WriteLine($"#### STATISTICS ####");

            int div = (iterations <= 200) ? 1 : iterations / 100;

            int total = 0;

            for (int i = 0; i < results.Count(); i++) {

                if (results[i] == 0) continue;
                Console.Write($"{i}: ");

                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.DarkBlue;

                for (var j = 0; j < results[i]/div; j++) Console.Write("#");
                
                Console.ResetColor();
                Console.WriteLine($" {results[i]}");// ({((100 * results[i]) / (float)iterations).ToString().WithMaxLenght(4)}%)");
                //Console.WriteLine($" {results[i]} ({((100 * results[i]) / (float)iterations).ToString().WithMaxLenght(4)}%)");
                total += i * results[i];
            }
            Console.WriteLine($"Mean value: {total/(float)iterations}");
        }

        private static void PrintProgress(int iteration) {
            if (iteration % (iterations / 50) == 0) {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write("#");
                Console.ResetColor();
            }
            if (iteration == iterations - 1) Console.WriteLine("");
        }
    }
}
