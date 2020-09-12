using System.Collections.Generic;
using System.Linq;
using static Hanabi.Game;
using static Hanabi.PlayerClasses.Player;

namespace Hanabi.Strategies {
    public class Clue {

        internal static bool ToAnyPlayable(bool avoidAlreadySingleClues = false) {
            if (clues == 0) return false;

            List<Brick>[] allPlayableBricks = currentPlayer.LookForPlayableBricks();
            for (int playerindex = 0; playerindex < players.Count(); playerindex++) {

                foreach (Brick brick in allPlayableBricks[playerindex]) {

                    if (avoidAlreadySingleClues && brick.SingleClued == true) continue;

                    if (!brick.gotNumberClue) {
                        currentPlayer.GiveANumberClueTo(players[playerindex], brick.PeakNumber());
                        return true;
                    } else if (!brick.gotColorClue) {
                        currentPlayer.GiveAColorClueTo(players[playerindex], brick.PeakColor());
                        return true;
                    }

                }
            }

            return false;

        }

        internal static bool AboutFives() {
            if (clues == 0) return false;
            // Check if you see any fives
            for (int playerindex = 0; playerindex < players.Count(); playerindex++) {

                if (playerindex == currentPlayerIndex) continue;

                foreach (Brick brick in players[playerindex].hand) {

                    if (brick.PeakNumber() == 5) {
                        currentPlayer.GiveANumberClueTo(players[playerindex], 5);
                        return true;
                    }
                }
            }

            return false;
        }

        internal static bool SingleBrickClues() {
            if (clues == 0) return false;

            foreach (PlayerClasses.Player.Clue clue in currentPlayer.PossibleCluesToGive[1]) {
                currentPlayer.GiveAClueTo(players[clue.playerIndex], clue.clue);
                return true;
            }

            return false;
        }

        internal static bool DoubleBrickClues() {
            if (clues == 0) return false;

            foreach (PlayerClasses.Player.Clue clue in currentPlayer.PossibleCluesToGive[2]) {
                if (clue.cluedBricks[0].HandAge < clue.cluedBricks[1].HandAge && clue.cluedBricks[0].IsBrickPlayable()) {
                    currentPlayer.GiveAClueTo(players[clue.playerIndex], clue.clue);
                    return true;
                }

                if (clue.cluedBricks[0].HandAge > clue.cluedBricks[1].HandAge && clue.cluedBricks[1].IsBrickPlayable()) {
                    currentPlayer.GiveAClueTo(players[clue.playerIndex], clue.clue);
                    return true;
                }
            }

            return false;
        }

        internal static bool WithMostInfo() {
            if (clues == 0) return false;

            for (int i = 5; i > 1; i--) {
                foreach (PlayerClasses.Player.Clue clue in currentPlayer.PossibleCluesToGive[i]) {
                    currentPlayer.GiveAClueTo(players[clue.playerIndex], clue.clue);
                    return true;
                }
            }

            return false;
        }


        internal static bool WithMostImportance() {
            if (clues == 0) return false;

            List<PlayerClasses.Player.Clue> allClues = new List<PlayerClasses.Player.Clue>();

            for (int i = 5; i > 1; i--) {
                foreach (PlayerClasses.Player.Clue clue in currentPlayer.PossibleCluesToGive[i]) {
                    allClues.Add(clue);
                }
            }

            var orderedClues = allClues.OrderByDescending(c => c.importance);

            if (orderedClues.Count() != 0) {
                var theClue = orderedClues.First();
                currentPlayer.GiveAClueTo(players[theClue.playerIndex], theClue.clue);
                lastMoveThinking += "Will give the most important clue";
                return true;
            }

            return false;
        }
    }
}
