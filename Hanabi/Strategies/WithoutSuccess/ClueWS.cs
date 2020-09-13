using System.Linq;
using static Hanabi.Game;

namespace Hanabi.Strategies.WithoutSuccess {
    class ClueWS {

        internal static bool DoubleBrickClues() {
            if (clues == 0) return false;

            foreach (PlayerClasses.Player.Clue clue in CurrentPlayer.PossibleCluesToGive[2]) {
                if (clue.cluedBricks[0].HandAge < clue.cluedBricks[1].HandAge && clue.cluedBricks[0].IsBrickPlayable()) {
                    CurrentPlayer.GiveAClueTo(players[clue.playerIndex], clue.clue);
                    return true;
                }

                if (clue.cluedBricks[0].HandAge > clue.cluedBricks[1].HandAge && clue.cluedBricks[1].IsBrickPlayable()) {
                    CurrentPlayer.GiveAClueTo(players[clue.playerIndex], clue.clue);
                    return true;
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
                        CurrentPlayer.GiveANumberClueTo(players[playerindex], 5);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
