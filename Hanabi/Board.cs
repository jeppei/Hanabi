using System;
using System.IO;
using System.Linq;
using static Hanabi.Brick;
using static Hanabi.GlobalVariables;

namespace Hanabi {

    class Board {
        static void Main() {


            for (int i = 0; i < iterations; i++) {
                
                ResetValues();
                PrintData();
                DealBricks();
                PrintData();

                while (true) {

                    if (IsTheGameOver()) break;

                    Player currentPlayer = players[currentPlayerIndex];

                    string oldHand = currentPlayer.ToString();
                    Strategies.PlayOnlyIf100Sure(currentPlayer);
                    VerifyThatThePlayerMadeAMove();

                    string newHand = currentPlayer.ToString();

                    Print($"Turn {turn}, " +
                          $"Player {currentPlayerIndex}: {oldHand} --> {newHand}" +
                          $"\n\tMove: {lastMoveDetails}, " +
                          $"Score={score}, " +
                          $"Lifes={lifes}, " +
                          $"PileSize={drawPile.Count}, ");
                    PrintPlayersHands();
                    PrintTableBricks();

                    GoToNextPlayer();
                }

                Print("THE GAME IS OVER");
                score = (lifes == 0) ? 0 : score;

                PrintData();
                SaveResult();
            }
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

        private static void Print(string text) {
            if (!printInConsole) return;
            string[] texts = text.Split("|");
            foreach (string t in texts) {
                if (t.StartsWith("white")) {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{t.Substring(5)}");
                    Console.ResetColor();
                } else if (t.StartsWith("yellow")) {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"{t.Substring(6)}");
                    Console.ResetColor();
                } else if (t.StartsWith("red")) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{t.Substring(3)}");
                    Console.ResetColor();
                } else if (t.StartsWith("green")) {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"{t.Substring(5)}");
                    Console.ResetColor();
                } else if (t.StartsWith("blue")) {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"{t.Substring(4)}");
                    Console.ResetColor();
                } else {
                    Console.Write(t);
                }
            }
            Console.WriteLine("");
        }

        private static void VerifyThatThePlayerMadeAMove() {
            if (turn > moves) {
                lifes = 0;
                Console.WriteLine("The player did not make a move!");
            } else if (turn < moves) {
                lifes = 0;
                Console.WriteLine("The player managed to make more than 1 move!");
            }
        }

        static void GoToNextPlayer() {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count();
            turn++;
        }

        static bool IsTheGameOver() {
            if (lastTurns == players.Count()) return true;
            if (drawPile.Count == 0) lastTurns += 1;
            if (lifes == 0) return true;
            return false;
        }

        static void PrintData() {

            Print("======= DATA =======");
            Print($"Score: {score}");
            Print($"Lifes: {lifes}");
            Print($"Clues: {clues}");
            Print($"Pile size: {drawPile.Count}");
            Print($"Trash size: {trashPile.Count}");
            Print($"Number of players: {players.Length}");
            Print($"Hand size: {handSize}");
            Print($"Players:");
            PrintPlayersHands();
            Print($"Played tiles: {table.BricksToString()}");
            Print($"");
            PrintTableBricks();
        }

        static void PrintPlayersHands() {
            for (int i = 0; i < players.Count(); i++) {
                Print($"  Player[{i + 1}]:{players[i]}");
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

            Print(whites + "]");
            Print(yellows + "]");
            Print(reds + "]");
            Print(greens + "]");
            Print(blues + "]");
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
