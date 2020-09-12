using Hanabi.PlayerClasses;
using System.Collections.Generic;
using System.Linq;
using static Hanabi.Game;
using static Hanabi.PlayerClasses.Player;

namespace Hanabi {
    public class Strategies {

        public static void UseLifesButDontDie() {

            Player currentPlayer = players[currentPlayerIndex];
            if (lifes > 1) currentPlayer.PlayABrick();
            else currentPlayer.TrashABrick();
        }

        public static void PlayOnlyIf100Sure1() {

            Player currentPlayer = players[currentPlayerIndex];
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

        public static void PlayOnlyIf100Sure2() {

            Player currentPlayer = players[currentPlayerIndex];
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
    
        public static void PlayIfTheOddsAreHigh() {

            Player currentPlayer = players[currentPlayerIndex];
            List<Brick>[] allPlayableBricks = currentPlayer.LookForPlayableBricks();

            // Check if I can play a brick, if so play it
            currentPlayer.hand.OrderByPlayability();
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
            currentPlayer.hand.OrderByTrashability();
            currentPlayer.TrashABrick();
        }

        public static void PlayIfTheOddsAreHigh2() {
            // This strategy is PlayIfTheOddsAreHigh and that if a player 
            // gives a clue about one brick then the player will play it

            Player currentPlayer = players[currentPlayerIndex];
            for (int i = 0; i < currentPlayer.hand.Count; i++) {
                Brick brick = currentPlayer.hand[i];
                if (brick.OnlyOneWithClue && brick.brickPlayability != 0) {
                    currentPlayer.PlayABrick(i);
                    return;
                }
            }

            // Check if I can play a brick, if so play it
            currentPlayer.hand.OrderByPlayability();
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

                        if (brick.OnlyOneWithClue == true) continue;
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
            currentPlayer.hand.OrderByTrashability();
            currentPlayer.TrashABrick();
        }

        public static void PriotitiesSingleBrickClues() {
            // This strategy is PlayIfTheOddsAreHigh2 and but with the 
            // addon that the players have a strategy for clues

            Player currentPlayer = players[currentPlayerIndex];
            for (int i = 0; i < currentPlayer.hand.Count; i++) {
                Brick brick = currentPlayer.hand[i];
                if (brick.OnlyOneWithClue && brick.brickPlayability != 0) {
                    lastMoveThinking = "Found a brick which got the only clue and is not 0% unplayable";
                    currentPlayer.PlayABrick(i);
                    return;
                }
            }

            // Check if I can play a brick, if so play it
            currentPlayer.hand.OrderByPlayability();
            for (int i = 0; i < currentPlayer.hand.Count; i++) {
                Brick brick = currentPlayer.hand[i];
                if (brick.brickPlayability == 1) {
                    lastMoveThinking = "Found a brick with 100% playability";
                    currentPlayer.PlayABrick(i);
                    return;
                } else if (brick.brickPlayability > 0.80 && lifes > 1) {
                    lastMoveThinking = "Found a brick with 80% playability and we have some lifes left";
                    currentPlayer.PlayABrick(i);
                    return;
                }
            }

            // Check if I can see a playable brick, if so give that player a clue about the brick
            if (clues > 0) {
                List<Brick>[] allPlayableBricks = currentPlayer.LookForPlayableBricks();

                // Each playable brick can be given one of two clues (number or color). We want a dictionary where 
                // The index is the number of bricks that matches a clue, and the value list of tuples. Item1 in the tupele is 
                // the clue and item2 is which player the clue is for. In short Dict[BricksMatchingClue] = (Clue, Player)
                Dictionary<int, List<(int, int)>> myClues = new Dictionary<int, List<(int, int)>>();
                for (int i = 1; i <= 5; i++) myClues[i] = new List<(int, int)>();

                for (int playerindex = 0; playerindex < players.Count(); playerindex++) {
                    int pIndex = (playerindex + currentPlayer.playerIndex) % players.Count();
                    foreach (Brick brick in allPlayableBricks[pIndex]) {

                        if (brick.OnlyOneWithClue) continue; // Dont want to give more clues to this one

                        int colorClue = (int)brick.PeakColor();
                        int numberClue = brick.PeakNumber();

                        int bricksMatchingColorClue = players[pIndex].NumberOfBricksWithThisClue(colorClue);
                        int bricksMatchingNumberClue = players[pIndex].NumberOfBricksWithThisClue(numberClue);

                        if (!brick.gotColorClue) myClues[bricksMatchingColorClue].Add((colorClue, pIndex));
                        if (!brick.gotNumberClue) myClues[bricksMatchingNumberClue].Add((numberClue, pIndex));
                    }
                }

                foreach ((int, int) clue in myClues[1]) {
                    // clue = (clue, player)
                    lastMoveThinking = "Found a clue which points to a single playable brick";
                    currentPlayer.GiveAClueTo(players[clue.Item2], clue.Item1);
                    return;
                }

                for (int i = 5; i > 1; i--) {
                    foreach ((int, int) clue in myClues[i]) {
                        // clue = (clue, player)
                        lastMoveThinking = "Will give a clue that involves most bricks";
                        currentPlayer.GiveAClueTo(players[clue.Item2], clue.Item1);
                        return;
                    }
                }

            }

            // Trash a brick as a last option
            // sort the bricks by trashability then throw the most trashable one
            currentPlayer.hand.OrderByTrashability();
            //for (int i = 0; i < currentPlayer.hand.Count; i++) {
            //    if (currentPlayer.hand[i].Age > 6) {
            //        currentPlayer.TrashABrick(i);
            //        return;
            //    }
            //}
            lastMoveThinking = "Will trash the most trashable brick";
            currentPlayer.TrashABrick();
        }

        public static void PlayIfTheOddsAreHigh3() {
            // This strategy is PlayIfTheOddsAreHigh2 and that the player 
            // also gives clues about 5s

            Player currentPlayer = players[currentPlayerIndex];
            for (int i = 0; i < currentPlayer.hand.Count; i++) {
                Brick brick = currentPlayer.hand[i];

                bool ImSureThisIsAFive = brick.gotNumberClue && brick.PeakNumber() == 5;

                if (brick.OnlyOneWithClue && !ImSureThisIsAFive) {
                    currentPlayer.PlayABrick(i);
                    return;
                }
            }

            // Check if I can play a brick, if so play it
            currentPlayer.hand.OrderByPlayability();
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

                        if (brick.OnlyOneWithClue == true) continue;
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
            currentPlayer.hand.OrderByTrashability();
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

        public static void PriotitiesSingleBrickCluesAndDenise() {
            // This strategy is PriotitiesSingleBrickClues and the Denise-strategy,
            Player currentPlayer = players[currentPlayerIndex];

            if (players.GetPreviousPlayer().History.Count() > 1) {
                MoveDetails lastMove = players.GetPreviousPlayer().History.Last();
                MoveDetails secLastMove = players.GetPreviousPlayer().History[players.GetPreviousPlayer().History.Count - 2];

                if (lastMove.move == MoveDetails.Move.PLAY) {
                    lastMoveThinking += "Prev played! ";

                    if (secLastMove.move == MoveDetails.Move.TRASH || secLastMove.move == MoveDetails.Move.CLUE) {
                        lastMoveThinking += "Prev didnt play before";
                        Brick oldBrick = secLastMove.handAfterMove.GetBrick(lastMove.brick);
                        if (oldBrick.OnlyOneWithClue && oldBrick.gotColorClue) {
                            lastMoveThinking += "The played brick was color and was only one ";

                            currentPlayer.PlayABrick(currentPlayer.hand.Count() - 1);
                            return;
                        }
                    }
                }
            }

            Brick waitBrick = null;
            for (int i = 0; i < currentPlayer.hand.Count; i++) {
                Brick brick = currentPlayer.hand[i];
                if (brick.OnlyOneWithClue) {
                    lastMoveThinking += "This brick is the only one with clue, ";
                    if (brick.gotColorClue) {
                        lastMoveThinking += "It has color clue, ";

                        // I only want to play it if I can see the next brick
                        int numberToLookFor = table.GreatestBrickInColor(brick.PeakColor())+2;
                        Brick brickToLookFor = new Brick(brick.PeakColor(), numberToLookFor);

                        if (numberToLookFor > 5) {
                            lastMoveThinking += "number is to high, ";

                            if (brick.brickPlayability != 0) {
                                lastMoveThinking += "it may be playable, ";
                                currentPlayer.PlayABrick(i);
                                return;
                            }
                            continue;
                        }

                        if (trash.ContainsAllBricksOfType(brickToLookFor)) {
                            lastMoveThinking += "trash contains all bricks of next type, ";
                            if (brick.brickPlayability != 0) {
                                lastMoveThinking += "it may be playable, ";
                                currentPlayer.PlayABrick(i);
                                return;
                            }
                        }

                        foreach (Player player in players) {

                            if (player.playerIndex == currentPlayerIndex) continue;
                            if (player.hand.Contains(brickToLookFor)) {
                                lastMoveThinking += "I can see the brick, ";

                                if (brick.brickPlayability != 0) {
                                    lastMoveThinking += "it seems playable, ";
                                    currentPlayer.PlayABrick(i);
                                    return;
                                }
                            }
                        }

                        lastMoveThinking += "I will wait to play this one, ";
                        waitBrick = brick;
                        // If its not in trash or in a players hand then we wait for it to show up

                    } else {

                        if (brick.brickPlayability != 0) {
                            currentPlayer.PlayABrick(i);
                            return;
                        }
                    }
                }
            }

            // Check if I can play a brick, if so play it
            currentPlayer.hand.OrderByPlayability();
            for (int i = 0; i < currentPlayer.hand.Count; i++) {
                Brick brick = currentPlayer.hand[i];

                if (brick == waitBrick) continue;
                if (brick.brickPlayability == 1) {
                    currentPlayer.PlayABrick(i);
                    return;
                } else if (brick.brickPlayability > 0.80 && lifes > 1) {
                    currentPlayer.PlayABrick(i);
                    return;
                }
            }

            // Check if I can see a playable brick, if so give that player a clue about the brick
            if (clues > 0) {
                List<Brick>[] allPlayableBricks = currentPlayer.LookForPlayableBricks();

                // Each playable brick can be given one of two clues (number or color). We want a dictionary where 
                // The index is the number of bricks that matches a clue, and the value list of tuples. Item1 in the tupele is 
                // the clue and item2 is which player the clue is for. In short Dict[BricksMatchingClue] = (Clue, Player)
                Dictionary<int, List<(int, int)>> myClues = new Dictionary<int, List<(int, int)>>();
                for (int i = 1; i <= 5; i++) myClues[i] = new List<(int, int)>();

                for (int playerindex = 0; playerindex < players.Count(); playerindex++) {
                    int pIndex = (playerindex + currentPlayer.playerIndex) % players.Count();
                    foreach (Brick brick in allPlayableBricks[pIndex]) {

                        if (brick.OnlyOneWithClue) continue; // Dont want to give more clues to this one

                        int colorClue = (int)brick.PeakColor();
                        int numberClue = brick.PeakNumber();

                        int bricksMatchingColorClue = players[pIndex].NumberOfBricksWithThisClue(colorClue);
                        int bricksMatchingNumberClue = players[pIndex].NumberOfBricksWithThisClue(numberClue);

                        if (!brick.gotColorClue) myClues[bricksMatchingColorClue].Add((colorClue, pIndex));
                        if (!brick.gotNumberClue) myClues[bricksMatchingNumberClue].Add((numberClue, pIndex));
                    }
                }

                foreach ((int, int) clue in myClues[1]) {
                    // clue = (clue, player)
                    currentPlayer.GiveAClueTo(players[clue.Item2], clue.Item1);
                    return;
                }

                for (int i = 5; i > 1; i--) {
                    foreach ((int, int) clue in myClues[i]) {
                        // clue = (clue, player)
                        currentPlayer.GiveAClueTo(players[clue.Item2], clue.Item1);
                        return;
                    }
                }

            }

            // Trash a brick as a last option
            // sort the bricks by trashability then throw the most trashable one
            currentPlayer.hand.OrderByTrashability();
            //for (int i = 0; i < currentPlayer.hand.Count; i++) {
            //    if (currentPlayer.hand[i].Age > 6) {
            //        currentPlayer.TrashABrick(i);
            //        return;
            //    }
            //}
            currentPlayer.TrashABrick();
        }
    }
}
