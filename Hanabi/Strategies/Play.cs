using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Hanabi.PlayerClasses;
using static Hanabi.Game;
using static Hanabi.PlayerClasses.Player;

namespace Hanabi.Strategies {
    public class Play {

        internal static bool IfTwoCluesAndPlayable() {

            for (int i = 0; i < currentPlayer.hand.Count; i++) {
                Brick brick = currentPlayer.hand[i];
                if (brick.GotEnoughClues() && brick.IsBrickPlayable()) {
                    currentPlayer.PlayABrick(i);
                    return true;
                }
            }
            return false;
        }

        internal static bool Playability100() {
            // Check if I can play a brick, if so play it
            for (int i = 0; i < currentPlayer.hand.Count; i++) {
                Brick brick = currentPlayer.hand[i];
                if (brick.brickPlayability == 1) {
                    currentPlayer.PlayABrick(i);
                    return true;
                }
            }
            return false;
        }

        internal static bool ByPlayability(float prob1lives = 1, float prob2lives = 1, float prob3lives = 1) {

            if (prob1lives < 0 || prob1lives > 1) throw new Exception("Probabilities must be between 0 and 1");
            if (prob2lives < 0 || prob2lives > 1) throw new Exception("Probabilities must be between 0 and 1");
            if (prob3lives < 0 || prob3lives > 1) throw new Exception("Probabilities must be between 0 and 1");

            // Check if I can play a brick, if so play it
            currentPlayer.hand.OrderByPlayability();
            for (int i = 0; i < currentPlayer.hand.Count; i++) {
                Brick brick = currentPlayer.hand[i];

                if (brick.brickPlayability == 1) {
                    currentPlayer.PlayABrick(i);
                    return true;

                } else if (brick.brickPlayability >= prob3lives && lifes == 3) {
                    currentPlayer.PlayABrick(i);
                    return true;

                } else if (brick.brickPlayability >= prob2lives && lifes == 2) {
                    currentPlayer.PlayABrick(i);
                    return true;

                } else if (brick.brickPlayability >= prob1lives && lifes == 1) {
                    currentPlayer.PlayABrick(i);
                    return true;
                }
            }
            return false;
        }

        internal static bool SingleClues() {
            for (int i = 0; i < currentPlayer.hand.Count; i++) {
                Brick brick = currentPlayer.hand[i];
                if (brick.SingleClued && brick.brickPlayability != 0) {
                    lastMoveThinking = "Found a brick which got the only clue and is not 0% unplayable";
                    currentPlayer.PlayABrick(i);
                    return true;
                }
            }
            return false;
        }

        internal static bool DoubleClueBrick() {
            var doubleClueBricks = currentPlayer.hand.Where(b => b.numberOfBricksWhoGotSameClue == 2).ToList();
            int handAge = -1;
            if (doubleClueBricks.Count() == 2) {
                if (doubleClueBricks[0].HandAge == doubleClueBricks[1].HandAge) return false;

                handAge = Math.Min(doubleClueBricks[0].HandAge, doubleClueBricks[1].HandAge);
                for (int i = 0; i < currentPlayer.hand.Count; i++) {
                    if (currentPlayer.hand[i].HandAge == handAge && currentPlayer.hand[i].numberOfBricksWhoGotSameClue == 2) {
                        currentPlayer.PlayABrick(i);
                    }
                }
            }
            return false;
        }

        internal static bool SingleBrickCluedFives() {
            for (int i = 0; i < currentPlayer.hand.Count; i++) {
                Brick brick = currentPlayer.hand[i];

                bool ImSureThisIsAFive = brick.gotNumberClue && brick.PeakNumber() == 5;

                if (brick.SingleClued && !ImSureThisIsAFive) {
                    currentPlayer.PlayABrick(i);
                    return true;
                }
            }
            return false;
        }

        internal static bool DeniseStrategyIGotYourClue() {

            // The previous player have waited so long to play this brick
            // which means that are probably have received this brick now 
            // (or the other team player)

            if (players.GetPreviousPlayer().History.Count() > 1) {
                MoveDetails lastMove = players.GetPreviousPlayer().History.Last();
                MoveDetails secLastMove = players.GetPreviousPlayer().History[players.GetPreviousPlayer().History.Count - 2];

                if (lastMove.move == MoveDetails.Move.PLAY) {
                    lastMoveThinking += "Prev played! ";

                    if (secLastMove.move == MoveDetails.Move.TRASH || secLastMove.move == MoveDetails.Move.CLUE) {
                        lastMoveThinking += "Prev didnt play before";
                        Brick oldBrick = secLastMove.handAfterMove.GetBrick(lastMove.brick);
                        if (oldBrick.SingleClued && oldBrick.gotColorClue) {
                            lastMoveThinking += "The played brick was color and was only one ";

                            currentPlayer.PlayABrick(currentPlayer.hand.Count() - 1);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        internal static bool WithMostPlayabilityExceptDenise(Brick waitBrick) {           
            // Check if I can play a brick, if so play it
            currentPlayer.hand.OrderByPlayability();
            for (int i = 0; i < currentPlayer.hand.Count; i++) {
                Brick brick = currentPlayer.hand[i];

                if (brick == waitBrick) continue;
                if (brick.brickPlayability == 1) {
                    currentPlayer.PlayABrick(i);
                    return true;
                } else if (brick.brickPlayability > 0.80 && lifes > 1) {
                    currentPlayer.PlayABrick(i);
                    return true;
                }
            }
            return false;
        }

        internal static bool DeniseStrategyWaitUntilNextArrive(out Brick waitBrick) {
            waitBrick = null;
            for (int i = 0; i < currentPlayer.hand.Count; i++) {
                Brick brick = currentPlayer.hand[i];
                if (brick.SingleClued) {
                    lastMoveThinking += "This brick is the only one with clue, ";
                    if (brick.gotColorClue) {
                        lastMoveThinking += "It has color clue, ";

                        // I only want to play it if I can see the next brick
                        int numberToLookFor = table.GreatestBrickInColor(brick.PeakColor()) + 2;
                        Brick brickToLookFor = new Brick(brick.PeakColor(), numberToLookFor);

                        if (numberToLookFor > 5) {
                            lastMoveThinking += "number is to high, ";

                            if (brick.brickPlayability != 0) {
                                lastMoveThinking += "it may be playable, ";
                                currentPlayer.PlayABrick(i);
                                return true;
                            }
                            continue;
                        }

                        if (trash.ContainsAllBricksOfType(brickToLookFor)) {
                            lastMoveThinking += "trash contains all bricks of next type, ";
                            if (brick.brickPlayability != 0) {
                                lastMoveThinking += "it may be playable, ";
                                currentPlayer.PlayABrick(i);
                                return true;
                            }
                        }

                        foreach (Player player in players) {

                            if (player.playerIndex == currentPlayerIndex) continue;
                            if (player.hand.Contains(brickToLookFor)) {
                                lastMoveThinking += "I can see the brick, ";

                                if (brick.brickPlayability != 0) {
                                    lastMoveThinking += "it seems playable, ";
                                    currentPlayer.PlayABrick(i);
                                    return true;
                                }
                            }
                        }

                        lastMoveThinking += "I will wait to play this one, ";
                        waitBrick = brick;
                        // If its not in trash or in a players hand then we wait for it to show up

                    } else {

                        if (brick.brickPlayability != 0) {
                            currentPlayer.PlayABrick(i);
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
