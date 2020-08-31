using System.Collections.Generic;
using System.Linq;
using static Hanabi.GlobalVariables;

namespace Hanabi {

    public class Player {

        public List<Brick> hand = new List<Brick>();
        public int playerIndex;

        public Player(int playerIndex) {
            this.playerIndex = playerIndex;
        }

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

        public void GetAClue(int clue) {

        }

        public List<Brick>[] LookForPlayableBricks() {
            List<Brick>[] playableBricks = new List<Brick>[players.Count()];
            for (int i = 0; i < playableBricks.Count(); i++) {
                playableBricks[i] = new List<Brick>();
            }

            for (int i = 0; i < players.Count(); i++) {
                if (i == playerIndex) continue;
                
                foreach (Brick brick in players[i].hand) {
                    if (Program.IsBrickPlayable(brick)) {
                        playableBricks[i].Add(brick);
                    }
                }
            }

            return playableBricks;
        }
    }
}
