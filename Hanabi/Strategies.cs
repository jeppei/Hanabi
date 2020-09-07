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

        public static void PlayOnlyIf100Sure1(Player currentPlayer) {

            List<Brick>[] allPlayableBricks = currentPlayer.LookForPlayableBricks();

            // Check if I can play a brick, if so play it
            for (int i = 0; i < currentPlayer.hand.Count; i++) {
                Brick brick = currentPlayer.hand[i];
                if (brick.GotEnoughClues() && brick.IsBrickPlayable()) {
                    currentPlayer.PlayABrick(i);
                    return;
                }
            }

            // Check if I can see a playable brick, if so give that player a clue about the brick
            if (clues > 0) {
                for (int playerindex = 0; playerindex < players.Count(); playerindex++) {
                    if (allPlayableBricks[playerindex].Count > 0) {

                        Brick brick = allPlayableBricks[playerindex].First();
                        if (!brick.gotColorClue) {
                            currentPlayer.GiveAColorClueTo(players[playerindex], brick.PeakColor());
                            return;
                        } else if (!brick.gotNumberClue) {
                            currentPlayer.GiveANumberClueTo(players[playerindex], brick.PeakNumber());
                            return;
                        } 
                    }
                }
            }

            // Trash a brick as a last option
            currentPlayer.TrashABrick();
        }

        public static void PlayOnlyIf100Sure2(Player currentPlayer) {

            List<Brick>[] allPlayableBricks = currentPlayer.LookForPlayableBricks();

            // Check if I can play a brick, if so play it
            for (int i = 0; i < currentPlayer.hand.Count; i++) {
                Brick brick = currentPlayer.hand[i];
                if (brick.brickPlayability == 1) {
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
    
        public static void PlayIfTheOddsAreHigh(Player currentPlayer) {

            List<Brick>[] allPlayableBricks = currentPlayer.LookForPlayableBricks();

            // Check if I can play a brick, if so play it
            currentPlayer.hand = currentPlayer.hand.OrderByDescending(b => b.brickPlayability).ToList();
            for (int i = 0; i < currentPlayer.hand.Count; i++) {
                Brick brick = currentPlayer.hand[i];
                if (brick.brickPlayability == 1) {
                    currentPlayer.PlayABrick(i);
                    return;
                } else if (brick.brickPlayability > 0.48 && lifes > 1) {
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
            // sort the bricks by trashability then throw the most trashable one
            currentPlayer.hand = currentPlayer.hand.OrderByDescending(b => b.brickTrashability).ToList();
            currentPlayer.TrashABrick();
        }
    }
}
