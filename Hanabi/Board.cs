using System;
using System.IO;
using System.Linq;
using System.Text;
using static Hanabi.Brick;
using static Hanabi.GlobalVariables;

namespace Hanabi {

    class Board {
        
        static readonly int[] results = new int[26];
        static StringBuilder history;
        
        static void Main() {
 
            Console.Write("[1                      50                      100]\n ");
            for (int i = 0; i < iterations; i++) {

                history = new StringBuilder();
                if (i % (iterations/50) == 0) {
                    PrintProgress();
                }

                printInConsole = (i == iterations - 1);
                if (printInConsole) Print("");

                ResetValues();
                PrintData();
                DealBricks();
                PrintData();

                while (true) {

                    if (IsTheGameOver()) break;

                    Player currentPlayer = players[currentPlayerIndex];

                    string oldHand = currentPlayer.ToStringWithClues();
                    foreach (Brick brick in currentPlayer.hand) {
                        currentPlayer.CalculateBrickPlayability(brick);
                        currentPlayer.CalculateBrickTrashability(brick);
                        brick.HandAge++;
                        if (brick.gotColorClue || brick.gotNumberClue) brick.ClueAge++;
                    }
                    foreach (Brick brick in table) brick.TableAge++;

                    string playabilities = string.Join(", ", currentPlayer.hand.Select(b => b.brickPlayability.ToString().Replace(",", ".").WithMaxLenght(4)));
                    string trashabilities = string.Join(", ", currentPlayer.hand.Select(b => b.brickTrashability.ToString().Replace(",", ".").WithMaxLenght(4)));

                    Strategies.PriotitiesSingleBrickClues(currentPlayer);
                    VerifyThatThePlayerMadeAMove();

                    Print($"Turn {turn}, " +
                          $"\nPlayer {currentPlayerIndex}: {oldHand}" +
                          $"\nPlayability:  {playabilities}" +
                          $"\nTrashability: {trashabilities}" +
                          $"\nMove: {lastMoveDetails}, " +
                          $"Score={score}, " +
                          $"Lifes={lifes}, " +
                          $"Clues={clues}, " +
                          $"PileSize={drawPile.Count}, ");
                    PrintPlayersHands();
                    Print($"Trash ({trashPile.Count}): {trashPile.BricksToString()}");
                    PrintTableBricks();
                    Print("");

                    GoToNextPlayer();
                }

                Print("THE GAME IS OVER");
                score = (lifes == 0) ? 0 : score;

                results[score]++;
                PrintData();
                //if (score == 24) {
                //    ForcePrint();
                //    return;
                //}
                SaveResult();
            }
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

        private static void SaveResult() {
            string result = $"Score = {score}, Cheat = {cheat}";
        //    //string path = 
        //    //    Directory.GetParent(
        //    //        Directory.GetParent(
        //    //            Directory.GetParent(
        //    //                Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName
        //    //            ).FullName
        //    //        ).FullName
        //    //    ).FullName;
        //
            string fileName = @"c:/Temp/hanabi-results.txt";
            using (StreamWriter sw = File.AppendText(fileName)) {
                sw.WriteLine(result);
            }
            //File.WriteAllText(fileName, result);
            Print("Results can be found in " + fileName);
        }

        private static void PrintProgress() {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("#");
            Console.ResetColor();
        }

        private static void Print(string text, bool includeLinebreak = true) {
            if (!printInConsole) {
                history.Append(text + "\n");
                return;
            }
            if (string.IsNullOrEmpty(text)) {
                Console.WriteLine("");
                return;
            }

            string[] texts = text.Split("|");
            foreach (string t in texts) {
                if (t.Length == 0) {
                    // Do nothing
                } else if (t[0] == 'w') {
                    PrintWithColor(t, ConsoleColor.White);
                } else if (t[0] == 'y') {
                    PrintWithColor(t, ConsoleColor.Yellow);
                } else if (t[0] == 'r') {
                    PrintWithColor(t, ConsoleColor.Red);
                } else if (t[0] == 'g') {
                    PrintWithColor(t, ConsoleColor.Green);
                } else if (t[0] == 'b') {
                    PrintWithColor(t, ConsoleColor.Blue);
                } else {
                    Console.Write(t);
                }
            }
            if (includeLinebreak) Console.WriteLine("");
        }

        private static void PrintWithColor(string text, ConsoleColor consoleColor) {
            Console.ForegroundColor = consoleColor;
            Console.Write($"{text.Substring(1)}");
            Console.ResetColor();
        }

        private static void VerifyThatThePlayerMadeAMove() {
            if (turn > moves) {
                Console.WriteLine("The player did not make a move!");
                throw new Exception("The player did not make a move!");
            } else if (turn < moves) {
                Console.WriteLine("The player managed to make more than 1 move!");
                throw new Exception("The player managed to make more than 1 move!");
            }
        }

        static void GoToNextPlayer() {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count();
            turn++;
        }

        static void ForcePrint() {
            bool temp = printInConsole;
            printInConsole = true;
            Print(history.ToString());
            printInConsole = temp;
        }

        static bool IsTheGameOver() {
            if (lastTurns == players.Count()) return true;
            if (score == 25) return true;
            if (drawPile.Count == 0) lastTurns += 1;
            if (lifes == 0) {
                /* This code can be used to examine why the number 
                 * of lifes is 0 (which leads to a score which is 0)
                ForcePrint(history.ToString());
                */
                return true;

            }
            return false;
        }

        static void PrintData() {

            Print("======= DATA =======");
            Print($"Score: {score}");
            Print($"Lifes: {lifes}");
            Print($"Clues: {clues}");
            Print($"Players ({players.Length}):");
            PrintPlayersHands();
            Print($"Draw pile ({drawPile.Count})");
            Print($"Table     ({table.Count}): {table.BricksToString()}");
            Print($"Trash     ({trashPile.Count}): {trashPile.BricksToString()}");
            Print($"");
            PrintTableBricks();
        }

        static void PrintPlayersHands() {
            for (int i = 0; i < players.Count(); i++) {
                Print($"  Player[{i}]: {players[i]}");
            }
        }

        public static void PrintTableBricks() {
            string whites  = "[ ";
            string yellows = "[ ";
            string reds    = "[ ";
            string greens  = "[ ";
            string blues   = "[ ";
            foreach (Brick brick in table) {
                if (brick.PeakColor() == Color.white) {
                    whites += brick;
                } else if (brick.PeakColor() == Color.yellow) {
                    yellows += brick;
                } else if (brick.PeakColor() == Color.red) {
                    reds += brick;
                } else if (brick.PeakColor() == Color.green) {
                    greens += brick;
                } else if (brick.PeakColor() == Color.blue) {
                    blues += brick;
                }
            }

            int w = 5 - (whites.Count()  - 2)/4;
            int y = 5 - (yellows.Count() - 2)/4;
            int r = 5 - (reds.Count()    - 2)/4;
            int g = 5 - (greens.Count()  - 2)/4;
            int b = 5 - (blues.Count()   - 2)/4;

            string ww = new string(' ', w);
            string yy = new string(' ', y);
            string rr = new string(' ', r);
            string gg = new string(' ', g);
            string bb = new string(' ', b);

            Print(whites  + ww + " ]");
            Print(yellows + yy + " ]");
            Print(reds    + rr + " ]");
            Print(greens  + gg + " ]");
            Print(blues   + bb + " ]");
        }

        static void DealBricks() {
            foreach (Player player in players) {
                while (player.hand.Count < handSize) {
                    Brick brick = drawPile.DrawBrick();
                    player.ReceiveBrick(brick);
                }
            }
        }

        static void DealCheatBricks() {
            cheat = true;
            Brick r3 = new Brick(Color.red, 3);
            Brick b1 = new Brick(Color.blue, 1);
            Brick b2 = new Brick(Color.blue, 2);
            Brick b3 = new Brick(Color.blue, 3);
            Brick b4 = new Brick(Color.blue, 4);
            Brick b5 = new Brick(Color.blue, 5);
            
            drawPile.CheatDrawBrick(r3);
            drawPile.CheatDrawBrick(b1);
            drawPile.CheatDrawBrick(b2);
            drawPile.CheatDrawBrick(b3);
            drawPile.CheatDrawBrick(b4);
            drawPile.CheatDrawBrick(b5);

            players[0].ReceiveBrick(b1);
            players[1].ReceiveBrick(b2);
            players[2].ReceiveBrick(b3);
            players[0].ReceiveBrick(r3);
            players[1].ReceiveBrick(b4);
            players[2].ReceiveBrick(b5);

            DealBricks();
        }
    }
}
