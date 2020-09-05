using System.Collections.Generic;
using System.Linq;
using static Hanabi.GlobalVariables;

namespace Hanabi {
    public class Strategies {

        public static void UseLifesButDontDie(Player currentPlayer) {


            if (lifes > 1) {
                currentPlayer.PlayABrick();
            } else {
                currentPlayer.TrashABrick();
            }

        }

        public static void PlayOnlyIf100Sure(Player currentPlayer) {

            List<Brick>[] allPlayableBricks = currentPlayer.LookForPlayableBricks();

            // Check if I can play a brick, if so play it
            for (int i = 0; i < currentPlayer.hand.Count; i++) {
                Brick brick = currentPlayer.hand[i];
                if (currentPlayer.CalculateBrickPlayability(brick) == 1) {
                //if (brick.GotEnoughClues() && brick.IsBrickPlayable()) {
                    currentPlayer.PlayABrick(i);
                    return;
                }
            }

            // Check if I can see a playable brick, if so give that player a clue about the brick
            if (clues > 0) {
                for (int playerindex = 0; playerindex < players.Count(); playerindex++) {
                    if (allPlayableBricks[playerindex].Count > 0) {

                        Brick brick = allPlayableBricks[playerindex].First();
                        if (!brick.gotNumberClue) {
                            currentPlayer.GiveANumberClueTo(players[playerindex], brick.PeakNumber());
                            return;
                        } else if (!brick.gotColorClue) {
                            currentPlayer.GiveAColorClueTo(players[playerindex], brick.PeakColor());
                            return;
                        } 
                    }
                }
            }

            // Trash a brick as a last option
            currentPlayer.TrashABrick();
        }
    }
}
