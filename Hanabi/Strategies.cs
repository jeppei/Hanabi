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
                } else if (brick.brickPlayability > 0.60 && lifes > 1) {
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

        public static void PlayIfTheOddsAreHigh2(Player currentPlayer) {
            // This strategy is PlayIfTheOddsAreHigh and that if a player 
            // gives a clue about one brick then the player will play it

            for (int i = 0; i < currentPlayer.hand.Count; i++) {
                Brick brick = currentPlayer.hand[i];
                if (brick.onlyOneWithClue && brick.brickPlayability != 0) {
                    currentPlayer.PlayABrick(i);
                    return;
                }
            }

            // Check if I can play a brick, if so play it
            currentPlayer.hand = currentPlayer.hand.OrderByDescending(b => b.brickPlayability).ToList();
            for (int i = 0; i < currentPlayer.hand.Count; i++) {
                Brick brick = currentPlayer.hand[i];
                if (brick.brickPlayability == 1) {
                    currentPlayer.PlayABrick(i);
                    return;
                } else if (brick.brickPlayability > 0.80 && lifes > 1) {
                    currentPlayer.PlayABrick(i);
                    return;
                }
            }

            // Check if I can see a playable brick, if so give that player a clue about the brick
            List<Brick>[] allPlayableBricks = currentPlayer.LookForPlayableBricks();
            if (clues > 0) {
                for (int playerindex = 0; playerindex < players.Count(); playerindex++) {

                    foreach (Brick brick in allPlayableBricks[playerindex]) {

                        if (brick.onlyOneWithClue == true) continue;
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

        public static void PlayIfTheOddsAreHigh3(Player currentPlayer) {
            // This strategy is PlayIfTheOddsAreHigh2 and that the player 
            // also gives clues about 5s

            for (int i = 0; i < currentPlayer.hand.Count; i++) {
                Brick brick = currentPlayer.hand[i];

                bool ImSureThisIsAFive = brick.gotNumberClue && brick.PeakNumber() == 5;

                if (brick.onlyOneWithClue && !ImSureThisIsAFive) {
                    currentPlayer.PlayABrick(i);
                    return;
                }
            }

            // Check if I can play a brick, if so play it
            currentPlayer.hand = currentPlayer.hand.OrderByDescending(b => b.brickPlayability).ToList();
            for (int i = 0; i < currentPlayer.hand.Count; i++) {
                Brick brick = currentPlayer.hand[i];
                if (brick.brickPlayability == 1) {
                    currentPlayer.PlayABrick(i);
                    return;
                } else if (brick.brickPlayability > 0.60 && lifes > 1) {
                    currentPlayer.PlayABrick(i);
                    return;
                }
            }

            // Check if I can see a playable brick, if so give that player a clue about that brick
            List<Brick>[] allPlayableBricks = currentPlayer.LookForPlayableBricks();
            if (clues > 0) {
                for (int playerindex = 0; playerindex < players.Count(); playerindex++) {

                    foreach (Brick brick in allPlayableBricks[playerindex]) {

                        if (brick.onlyOneWithClue == true) continue;
                        if (!brick.gotNumberClue) {
                            currentPlayer.GiveANumberClueTo(players[playerindex], brick.PeakNumber());
                            return;
                        } else if (!brick.gotColorClue) {
                            currentPlayer.GiveAColorClueTo(players[playerindex], brick.PeakColor());
                            return;
                        }
                    }
                }

                // Check if you see any fives
                for (int playerindex = 0; playerindex < players.Count(); playerindex++) {

                    if (playerindex == currentPlayerIndex) continue;

                    foreach (Brick brick in players[playerindex].hand) {

                        if (brick.PeakNumber() == 5) {
                            currentPlayer.GiveANumberClueTo(players[playerindex], 5);
                            return;
                        }
                    }
                }
            }

            // Trash a brick as a last option
            // sort the bricks by trashability then throw the most trashable one
            currentPlayer.hand = currentPlayer.hand.OrderByDescending(b => b.brickTrashability).ToList();
            for (int i = 0; i < currentPlayer.hand.Count; i++ ) {

                Brick brick = currentPlayer.hand[i];

                if (brick.brickTrashability == 1) {
                    currentPlayer.TrashABrick(i);
                    return;
                }

                if (brick.gotNumberClue && brick.PeakNumber() == 5) continue;

                currentPlayer.TrashABrick(i);
                return;
            }
        }
    }
}
