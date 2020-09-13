using static Hanabi.Game;

namespace Hanabi.Strategies {
    public class Trash {
        internal static bool ByTrashability() {
            CurrentPlayer.hand.OrderByTrashability();
            CurrentPlayer.TrashABrick();
            lastMoveThinking = "Will trash the most trashable brick";
            return true;
        }
    }
}
