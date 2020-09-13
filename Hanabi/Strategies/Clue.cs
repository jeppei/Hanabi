using Hanabi.PlayerClasses;
using System.Collections.Generic;
using System.Linq;
using static Hanabi.Brick;
using static Hanabi.Game;

namespace Hanabi.Strategies {
    public class Clue {

        internal static bool ToAnyPlayable(bool avoidAlreadySingleClues = false) {
            if (clues == 0) return false;

            List<Brick>[] allPlayableBricks = CurrentPlayer.LookForPlayableBricks();
            for (int playerindex = 0; playerindex < players.Count(); playerindex++) {

                foreach (Brick brick in allPlayableBricks[playerindex]) {

                    if (avoidAlreadySingleClues && brick.SingleClued == true) continue;

                    if (!brick.gotNumberClue) {
                        CurrentPlayer.GiveANumberClueTo(players[playerindex], brick.PeakNumber());
                        return true;
                    } else if (!brick.gotColorClue) {
                        CurrentPlayer.GiveAColorClueTo(players[playerindex], brick.PeakColor());
                        return true;
                    }

                }
            }

            return false;

        }

        internal static bool SingleBrickClues() {
            if (clues == 0) return false;

            foreach (PlayerClasses.Player.Clue clue in CurrentPlayer.PossibleCluesToGive[1]) {
                CurrentPlayer.GiveAClueTo(players[clue.playerIndex], clue.clue);
                return true;
            }

            return false;
        }

        internal static bool WithMostInfo() {
            if (clues == 0) return false;

            for (int i = 5; i > 1; i--) {
                foreach (PlayerClasses.Player.Clue clue in CurrentPlayer.PossibleCluesToGive[i]) {
                    CurrentPlayer.GiveAClueTo(players[clue.playerIndex], clue.clue);
                    return true;
                }
            }

            return false;
        }

        internal static bool WithMostImportance(bool avoidAlreadySingleClues = true) {
            if (clues == 0) return false;

            // Add all clues to a list to sort them
            List<Player.Clue> allClues = new List<PlayerClasses.Player.Clue>();
            for (int i = 5; i > 1; i--) {
                foreach (Player.Clue clue in CurrentPlayer.PossibleCluesToGive[i]) {
                    allClues.Add(clue);
                }
            }

            // Sort them
            var orderedClues = allClues.OrderByDescending(c => c.importance);

            // Use the most important one, skip singleCluedBricks
            foreach (Player.Clue clue in orderedClues) {

                // Dont give clues to bricks who already got a singleBrickCLue
                if (avoidAlreadySingleClues && clue.importantBrick.SingleClued == true) continue;
                // Dont give clues to bricks who already got a clue already
                var alreadyClued = CurrentPlayer.CluedBricksThatIKnowOf.Where(b => b.SingleClued);
                if (alreadyClued.Contains(clue.importantBrick)) continue;

                CurrentPlayer.GiveAClueTo(players[clue.playerIndex], clue.clue);
                lastMoveThinking += "Will give the most important clue";
                return true;
            }

            return false;
        }

        public static bool HintAboutManyOfSameNumber(int numberX, int leastNumberOfX = 1) {
            // As a first move we want to give one hint many ones with different colors

            Player bestPlayer = null;
            int numberOfX = -1;
            foreach (Player player in players) {
                if (player.playerIndex == currentPlayerIndex) continue;

                List<Brick> exes = new List<Brick>();
                List<Color> colors = new List<Color>();
                bool badClue = false;

                foreach (Brick brick in player.hand) {
                    if (brick.PeakNumber() == numberX) {
                        exes.Add(brick);

                        if (colors.Contains(brick.PeakColor())) badClue = true;
                        if (brick.gotNumberClue) badClue = true;

                        colors.Add(brick.PeakColor());
                    }
                }

                if (badClue == false && exes.Count > 0) {
                    if (exes.Count > numberOfX) {
                        numberOfX = exes.Count;
                        bestPlayer = player;
                    }
                }
            }

            if (bestPlayer != null && numberOfX >= leastNumberOfX) {
                CurrentPlayer.GiveAClueTo(bestPlayer, numberX);
            }

            return false;
        }
    }
}
