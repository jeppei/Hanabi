using System.Collections.Generic;
using System.Linq;
using static Hanabi.GlobalVariables;
using static Hanabi.Program;

namespace Hanabi {
    public class Strategies {

        static bool giveColorClue = true;

        public static void MakeATurn(Player currentPlayer) {


            if (lifes > 1) {
                PlayABrick(currentPlayer);
            } else {
                TrashABrick(currentPlayer);
            }

        }

        public static void MakeATurn2(Player currentPlayer) {

            List<Brick>[] allPlayableBricks = currentPlayer.LookForPlayableBricks();

            // Check if I can see a playable brick, if so give that player a clue about the brick
            if (clues > 0) {
                for (int playerindex = 0; playerindex < players.Count(); playerindex++) {
                    if (allPlayableBricks[playerindex].Count > 0) {

                        Brick brick = allPlayableBricks[playerindex].First();
                        if (giveColorClue) {
                            GiveAClue(players[playerindex], (int)brick.color);
                        } else {
                            GiveAClue(players[playerindex], brick.number);
                        }

                        giveColorClue = !giveColorClue;
                        return;
                    }
                }
            }

            // Check if I can play a brick, if so play it
            for (int i = 0; i < currentPlayer.hand.Count; i++) {
                if (currentPlayer.hand[i].GotEnoughClues()) {
                    PlayABrick(currentPlayer, i);
                    return;
                }
            }

            // Trash a brick as a last option
            TrashABrick(currentPlayer);

        }
    }
}
