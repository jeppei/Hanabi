namespace Hanabi.Strategies.WithoutSuccess {
    class StrategyWS {

        public static void PlayOldestBricksWIthClues() {

            if (Play.SingleClues()) return;
            if (PlayWS.DoubleClueBrick()) return;
            if (Play.ByPlayability(0.80f)) return;

            if (Clue.SingleBrickClues()) return;
            if (ClueWS.DoubleBrickClues()) return;
            if (Clue.WithMostImportance()) return;

            if (Trash.ByTrashability()) return;
        }

        public static void PlayIfTheOddsAreHigh3() {

            if (PlayWS.SingleBrickCluedFives()) return;
            if (Play.ByPlayability(60)) return;

            if (Clue.ToAnyPlayable(avoidAlreadySingleClues: true)) return;
            if (ClueWS.AboutFives()) return;

            if (TrashWS.MostTrashablexceptFives()) return;
        }

        public static void PriotitiesSingleBrickCluesAndDenise() {

            if (PlayWS.DeniseStrategyIGotYourClue()) return;
            if (PlayWS.DeniseStrategyWaitUntilNextArrive(out Brick waitBrick)) return;
            if (PlayWS.WithMostPlayabilityExceptDenise(waitBrick)) return;

            if (Clue.SingleBrickClues()) return;
            if (Clue.WithMostInfo()) return;

            if (Trash.ByTrashability()) return;
        }
    }
}
