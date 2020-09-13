using static Hanabi.Game;

namespace Hanabi.Strategies.WithoutSuccess {
    class TrashWS {

        internal static bool MostTrashablexceptFives() {
            CurrentPlayer.hand.OrderByTrashability();
            for (int i = 0; i < CurrentPlayer.hand.Count; i++) {

                Brick brick = CurrentPlayer.hand[i];

                if (brick.brickTrashability == 1) {
                    CurrentPlayer.TrashABrick(i);
                    return true;
                }

                if (brick.gotNumberClue && brick.PeakNumber() == 5) continue;

                CurrentPlayer.TrashABrick(i);
                return true;
            }
            return false;
        }
    }
}
