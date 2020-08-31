using System.Collections.Generic;

namespace Hanabi {

    public class Player {

        public List<Brick> hand = new List<Brick>();

        public void ReceiveBrick(Brick brick, int brickIndex = -1) {
            brickIndex = (brickIndex == -1) ? hand.Count : brickIndex;
            hand.Insert(brickIndex, brick);
        }

        public Brick RemoveBrick(int i) {
            Brick brickToRemove = hand[i];
            hand.RemoveAt(i);
            return brickToRemove;
        }

        public override string ToString() {
            return string.Join(", ", hand);
        }
    }
}
