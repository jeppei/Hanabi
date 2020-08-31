using System;
using System.Linq;
using static Hanabi.GlobalVariables;

namespace Hanabi {

    class Program {
        static void Main(string[] args) {

            PrintData();
            DealCheatBricks();
            PrintData();

            while (true) {

                if (IsTheGameOver()) break;

                Player currentPlayer = players[currentPlayerIndex];

                string oldHand = currentPlayer.ToString();
                Strategies.MakeATurn(currentPlayer);

                string newHand = currentPlayer.ToString();

                Console.WriteLine($"Turn {turn}, " +
                                  $"Player {currentPlayerIndex}: {oldHand} \t--> {newHand}" +
                                  $"\n\tTable={Brick.BricksAsString(table)}, " +
                                  $"PileSize={drawPile.Count}, " +
                                  $"Score={score}, " +
                                  $"Lifes={lifes}, " );

                GoToNextPlayer();
            }

            Console.WriteLine("THE GAME IS OVER");
            PrintData();
        }

        static void GoToNextPlayer() {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count();
            turn += 1;
        }

        static bool IsBrickPlayable(Brick brick) {
            if (table.Contains(brick)) return false;
            if (brick.number == 1) return true;
            return table.Contains(new Brick(brick.color, brick.number-1));
        }

        static void AddClue() {
            if (clues == 8) return;
            clues += 1;
        }


        public static void TrashABrick(Player player, int brickIndex = 0) {
            Brick trashedBrick = player.RemoveBrick(brickIndex);
            trashPile.Add(trashedBrick);
            AddClue();

            DrawABrick(player);
        }
        public static void PlayABrick(Player player, int brickIndex = 0) {
            Brick playedBrick = player.RemoveBrick(brickIndex);

            if (IsBrickPlayable(playedBrick)) {
                score += 1;
                if (playedBrick.number == 5) AddClue();
                table.Add(playedBrick);
            } else {
                lifes -= 1;
                trashPile.Add(playedBrick);
            }

            DrawABrick(player);
        }

        static void DrawABrick(Player player) {
            if (drawPile.Count > 0) {
                Brick newBrick = drawPile.DrawBrick();
                player.ReceiveBrick(newBrick);
            }
        }

        static bool UseAClue() {
            if (clues == 0) return false;
            clues -= 1;
            return true;
        }

        static bool IsTheGameOver() {
            if (lastTurns == players.Count()) return true;
            if (drawPile.Count == 0) lastTurns += 1;
            if (lifes == 0) return true;
            return false;
        }

        static void PrintData() {

            Console.WriteLine("======= DATA =======");
            Console.WriteLine($"Score: {score}");
            Console.WriteLine($"Lifes: {lifes}");
            Console.WriteLine($"Clues: {clues}");
            Console.WriteLine($"Pile size: {drawPile.Count}");
            Console.WriteLine($"Trash size: {trashPile.Count}");
            Console.WriteLine($"Number of players: {players.Length}");
            Console.WriteLine($"Hand size: {handSize}");
            Console.WriteLine($"Players:");
            for (int i = 0; i < players.Count(); i++) {
                Console.WriteLine($"  Player[{i+1}]:{players[i]}");
            }
            Console.WriteLine($"");
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
            Brick r3 = new Brick(Brick.Color.red, 3);
            Brick b1 = new Brick(Brick.Color.blue, 1);
            Brick b2 = new Brick(Brick.Color.blue, 2);
            Brick b3 = new Brick(Brick.Color.blue, 3);
            Brick b4 = new Brick(Brick.Color.blue, 4);
            Brick b5 = new Brick(Brick.Color.blue, 5);
            
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
