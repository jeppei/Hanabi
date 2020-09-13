using System;
using static Hanabi.Game;

namespace Hanabi.Strategies {
    public class Strategy {

        public static void UseLifesButDontDie() {
            if (lifes > 1) CurrentPlayer.PlayABrick();
            else CurrentPlayer.TrashABrick();
        }
    
        public static void PlayIfTheOddsAreHigh() {

            if (Play.ByPlayability(60)) return;

            if (Clue.ToAnyPlayable()) return;

            if (Trash.ByTrashability()) return;
        }

        public static void HighOddsAndSingleClues() {

            if (Play.SingleClues()) return;
            if (Play.ByPlayability(80)) return;

            if (Clue.ToAnyPlayable(avoidAlreadySingleClues: true)) return;

            if (Trash.ByTrashability()) return;
        }

        public static void PriotitiesSingleBrickClues() {

            if (Play.SingleClues()) return;
            if (Play.ByPlayability(0.80f)) return;

            if (Clue.SingleBrickClues()) return;
            if (Clue.WithMostInfo()) return;

            if (Trash.ByTrashability()) return;
        }


        public static void PriotitiesSingleBrickAndImportantClues() {
            // Best strategy so far, average score is 20.3-20.4
            if (turn == 1) {
                if (Clue.HintAboutManyOfSameNumber(1)) return;
            }

            if (lastTurns == players.Count && lifes > 1) {
                if (Play.ByPlayability(1, 0, 0)) return;
            }

            //if (Play.PlayOnes()) return;
            if (Play.SingleClues()) return;
            if (Play.ByPlayability(0.80f)) return;

            if (Clue.SingleBrickClues()) return;
            if (Clue.HintAboutManyOfSameNumber(5, 2)) return;
            if (Clue.WithMostImportance()) return;

            if (Trash.ByTrashability()) return;
        }
    }
}
