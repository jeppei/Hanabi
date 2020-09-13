using System;
using System.Linq;
using Hanabi.PlayerClasses;
using static Hanabi.Game;

namespace Hanabi.Strategies {
    class PlayWS {

        internal static bool DoubleClueBrick() {
            var doubleClueBricks = CurrentPlayer.hand.Where(b => b.numberOfBricksWhoGotSameClue == 2).ToList();
            int handAge = -1;
            if (doubleClueBricks.Count() == 2) {
                if (doubleClueBricks[0].HandAge == doubleClueBricks[1].HandAge) return false;

                handAge = Math.Min(doubleClueBricks[0].HandAge, doubleClueBricks[1].HandAge);
                for (int i = 0; i < CurrentPlayer.hand.Count; i++) {
                    if (CurrentPlayer.hand[i].HandAge == handAge && CurrentPlayer.hand[i].numberOfBricksWhoGotSameClue == 2) {
                        CurrentPlayer.PlayABrick(i);
                    }
                }
            }
            return false;
        }

        internal static bool SingleBrickCluedFives() {
            for (int i = 0; i < CurrentPlayer.hand.Count; i++) {
                Brick brick = CurrentPlayer.hand[i];

                bool ImSureThisIsAFive = brick.gotNumberClue && brick.PeakNumber() == 5;

                if (brick.SingleClued && !ImSureThisIsAFive) {
                    CurrentPlayer.PlayABrick(i);
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

                            CurrentPlayer.PlayABrick(CurrentPlayer.hand.Count() - 1);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        internal static bool WithMostPlayabilityExceptDenise(Brick waitBrick) {
            // Check if I can play a brick, if so play it
            CurrentPlayer.hand.OrderByPlayability();
            for (int i = 0; i < CurrentPlayer.hand.Count; i++) {
                Brick brick = CurrentPlayer.hand[i];

                if (brick == waitBrick) continue;
                if (brick.brickPlayability == 1) {
                    CurrentPlayer.PlayABrick(i);
                    return true;
                } else if (brick.brickPlayability > 0.80 && lifes > 1) {
                    CurrentPlayer.PlayABrick(i);
                    return true;
                }
            }
            return false;
        }

        internal static bool DeniseStrategyWaitUntilNextArrive(out Brick waitBrick) {
            waitBrick = null;
            for (int i = 0; i < CurrentPlayer.hand.Count; i++) {
                Brick brick = CurrentPlayer.hand[i];
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
                                CurrentPlayer.PlayABrick(i);
                                return true;
                            }
                            continue;
                        }

                        if (trash.ContainsAllBricksOfType(brickToLookFor)) {
                            lastMoveThinking += "trash contains all bricks of next type, ";
                            if (brick.brickPlayability != 0) {
                                lastMoveThinking += "it may be playable, ";
                                CurrentPlayer.PlayABrick(i);
                                return true;
                            }
                        }

                        foreach (Player player in players) {

                            if (player.playerIndex == currentPlayerIndex) continue;
                            if (player.hand.Contains(brickToLookFor)) {
                                lastMoveThinking += "I can see the brick, ";

                                if (brick.brickPlayability != 0) {
                                    lastMoveThinking += "it seems playable, ";
                                    CurrentPlayer.PlayABrick(i);
                                    return true;
                                }
                            }
                        }

                        lastMoveThinking += "I will wait to play this one, ";
                        waitBrick = brick;
                        // If its not in trash or in a players hand then we wait for it to show up

                    } else {

                        if (brick.brickPlayability != 0) {
                            CurrentPlayer.PlayABrick(i);
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
