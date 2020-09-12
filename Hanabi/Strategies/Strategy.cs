using static Hanabi.Game;

namespace Hanabi.Strategies {
    public class Strategy {

        public static void UseLifesButDontDie() {

            if (lifes > 1) currentPlayer.PlayABrick();
            else currentPlayer.TrashABrick();
        }

        public static void PlayOnlyIf100Sure() {

            if (Play.IfTwoCluesAndPlayable()) return;

            if (Clue.ToAnyPlayable()) return;

            currentPlayer.TrashABrick();
        }

        public static void PlayOnlyIfPlayability1() {

            if (Play.ByPlayability()) return;

            if (Clue.ToAnyPlayable()) return;

            currentPlayer.TrashABrick();
        }
    
        public static void PlayIfTheOddsAreHigh() {

            if (Play.ByPlayability(60)) return;

            if (Clue.ToAnyPlayable()) return;

            if (Trash.ByTrashability()) return;
        }

        public static void PlayIfTheOddsAreHigh2() {

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

            if (Play.SingleClues()) return;
            if (Play.ByPlayability(0.80f)) return;

            if (Clue.SingleBrickClues()) return;
            if (Clue.WithMostImportance()) return;

            if (Trash.ByTrashability()) return;
        }

        public static void PlayOldestBricksWIthClues() {

            if (Play.SingleClues()) return;
            if (Play.DoubleClueBrick()) return;
            if (Play.ByPlayability(0.80f)) return;

            if (Clue.SingleBrickClues()) return;
            if (Clue.DoubleBrickClues()) return;
            if (Clue.WithMostImportance()) return;

            if (Trash.ByTrashability()) return;
        }

        public static void PlayIfTheOddsAreHigh3() {

            if (Play.SingleBrickCluedFives()) return;
            if (Play.ByPlayability(60)) return;

            if (Clue.ToAnyPlayable(avoidAlreadySingleClues: true)) return;
            if (Clue.AboutFives()) return;

            if (Trash.MostTrashablexceptFives()) return;
        }

        public static void PriotitiesSingleBrickCluesAndDenise() {

            if (Play.DeniseStrategyIGotYourClue()) return;
            if (Play.DeniseStrategyWaitUntilNextArrive(out Brick waitBrick)) return;
            if (Play.WithMostPlayabilityExceptDenise(waitBrick)) return;

            if (Clue.SingleBrickClues()) return;
            if (Clue.WithMostInfo()) return;

            if (Trash.ByTrashability()) return;
        }
    }
}
