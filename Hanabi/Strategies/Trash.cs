using System;
using System.Collections.Generic;
using System.Text;
using Hanabi.PlayerClasses;
using static Hanabi.Game;

namespace Hanabi.Strategies {
    public class Trash {
        internal static bool ByTrashability() {
            currentPlayer.hand.OrderByTrashability();
            currentPlayer.TrashABrick();
            lastMoveThinking = "Will trash the most trashable brick";
            return true;
        }

        internal static bool MostTrashablexceptFives() {
            currentPlayer.hand.OrderByTrashability();
            for (int i = 0; i < currentPlayer.hand.Count; i++) {

                Brick brick = currentPlayer.hand[i];

                if (brick.brickTrashability == 1) {
                    currentPlayer.TrashABrick(i);
                    return true;
                }

                if (brick.gotNumberClue && brick.PeakNumber() == 5) continue;

                currentPlayer.TrashABrick(i);
                return true;
            }
            return false;
        }
    }
}
