using System;
using static Hanabi.Game;

namespace Hanabi.Strategies {
    public class Play {

        internal static bool ByPlayability(float prob1lives = 1, float prob2lives = 1, float prob3lives = 1) {

            if (prob1lives < 0 || prob1lives > 1) throw new Exception("Probabilities must be between 0 and 1");
            if (prob2lives < 0 || prob2lives > 1) throw new Exception("Probabilities must be between 0 and 1");
            if (prob3lives < 0 || prob3lives > 1) throw new Exception("Probabilities must be between 0 and 1");

            // Check if I can play a brick, if so play it
            CurrentPlayer.hand.OrderByPlayability();
            for (int i = 0; i < CurrentPlayer.hand.Count; i++) {
                Brick brick = CurrentPlayer.hand[i];

                if (brick.brickPlayability == 1) {
                    lastMoveThinking = "This brick is 100% playable";
                    CurrentPlayer.PlayABrick(i);
                    return true;

                } else if (brick.brickPlayability >= prob3lives && lifes >= 3) {
                    lastMoveThinking = $"This brick is {prob3lives * 100}% playable (and we have 3 lives)";
                    CurrentPlayer.PlayABrick(i);
                    return true;

                } else if (brick.brickPlayability >= prob2lives && lifes >= 2) {
                    lastMoveThinking = $"This brick is {prob2lives * 100}% playable (and we have 2 lives)";
                    CurrentPlayer.PlayABrick(i);
                    return true;

                } else if (brick.brickPlayability >= prob1lives && lifes >= 1) {
                    lastMoveThinking = $"This brick is {prob1lives * 100}% playable (and we have 1 lives)";
                    CurrentPlayer.PlayABrick(i);
                    return true;
                }
            }
            return false;
        }

        internal static bool SingleClues() {
            for (int i = 0; i < CurrentPlayer.hand.Count; i++) {
                Brick brick = CurrentPlayer.hand[i];
                if (brick.SingleClued && brick.brickPlayability != 0) {
                    lastMoveThinking = "Found a brick which got the only clue and is not 0% playable";
                    CurrentPlayer.PlayABrick(i);
                    return true;
                }
            }
            return false;
        }

        internal static bool PlayOnes() {
            for (int i = 0; i < CurrentPlayer.hand.Count; i++) {
                Brick brick = CurrentPlayer.hand[i];
                if (brick.gotNumberClue && brick.PeakNumber() == 1) {
                    CurrentPlayer.PlayABrick(i);
                }
            }
            return false;
        }
    }
}
