using Hanabi.PlayerClasses;
using System;
using System.Linq;
using System.Text;
using static Hanabi.Brick;
using static Hanabi.Game;

namespace Hanabi {
    public class GameHistory {

        private readonly StringBuilder history = new StringBuilder();

        public void AddCurrentBrickStats() {
            Player player = players[currentPlayerIndex];
            string oldHand = player.ToStringWithClues();
            string playAbilities = string.Join(", ", player.hand.Select(b => b.brickPlayability.ToString().Replace(",", ".").WithLenght(4)));
            string trashAbilities = string.Join(", ", player.hand.Select(b => b.brickTrashability.ToString().Replace(",", ".").WithLenght(4)));
            AddLine($"Turn {turn}, ");
            AddLine($"Player {currentPlayerIndex}:     {oldHand}");
            AddLine($"Playability:  {playAbilities}");
            AddLine($"Trashability: {trashAbilities}");
        }

        public void AddGameStats() {

            history.Append($"Score: {score}" + "\n");
            history.Append($"Lifes: {lifes}" + "\n");
            history.Append($"Clues: {clues}" + "\n");
            AddPlayersHand();
            history.Append($"Draw pile ({drawPile.Count})" + "\n");
            history.Append($"Trash     ({trash.Count}): {trash.BricksToString()}" + "\n");
            history.Append($"" + "\n");
            AddTableBricks();
            AddSeparator();
        }

        public void AddPlayersMove() {
            AddLine($"Move: {lastMoveDetails}. {lastMoveThinking}");
            AddSeparator();
        }

        private void AddSeparator() {
            history.Append("#############################################");
            history.Append("#############################################\n");
        }

        public void AddLine(string text) {
            history.Append($"{text}\n");
        }

        private void AddPlayersHand() {
            history.Append($"Players ({players.Count()}):" + "\n");
            for (int i = 0; i < players.Count; i++) {
                history.Append($"  Player[{i}]: {players[i].ToStringWithCluesCompact()}" + "\n");
            }
        }

        private void AddTableBricks() {
            string whites = "[ ";
            string yellows = "[ ";
            string reds = "[ ";
            string greens = "[ ";
            string blues = "[ ";
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

            int w = 5 - (whites.Count() - 2) / 4;
            int y = 5 - (yellows.Count() - 2) / 4;
            int r = 5 - (reds.Count() - 2) / 4;
            int g = 5 - (greens.Count() - 2) / 4;
            int b = 5 - (blues.Count() - 2) / 4;

            string ww = new string(' ', w);
            string yy = new string(' ', y);
            string rr = new string(' ', r);
            string gg = new string(' ', g);
            string bb = new string(' ', b);

            history.Append(whites + ww + " ]" + "\n");
            history.Append(yellows + yy + " ]" + "\n");
            history.Append(reds + rr + " ]" + "\n");
            history.Append(greens + gg + " ]" + "\n");
            history.Append(blues + bb + " ]" + "\n");
        }

        public void PrintToConsole() {

            string[] texts = history.ToString().Split("|");
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
            Console.WriteLine("");
        }

        private static void PrintWithColor(string text, ConsoleColor consoleColor) {
            Console.ForegroundColor = consoleColor;
            Console.Write($"{text.Substring(1)}");
            Console.ResetColor();
        }
    }
}
