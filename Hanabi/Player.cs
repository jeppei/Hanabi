using System.Collections.Generic;
using System.Linq;
using static Hanabi.Brick;
using static Hanabi.GlobalVariables;

namespace Hanabi {

    public class Player {

        public List<Brick> hand = new List<Brick>();

        public int playerIndex;
        public override string ToString() => string.Join(", ", hand);

        public Player(int playerIndex) {
            this.playerIndex = playerIndex;
        }

        public void ReceiveBrick(Brick receivedBrick, int brickIndex = -1) {
            receivedBrick.playerIndex = playerIndex;
            brickIndex = (brickIndex == -1) ? hand.Count : brickIndex;
            hand.Insert(brickIndex, receivedBrick);
        }

        private Brick RemoveBrick(int i) {
            Brick brickToRemove = hand[i];
            brickToRemove.playerIndex = -1;
            hand.RemoveAt(i);
            return brickToRemove;
        }


        public List<Brick>[] LookForPlayableBricks() {
            List<Brick>[] playableBricks = new List<Brick>[players.Count()];
            for (int i = 0; i < playableBricks.Count(); i++) {
                playableBricks[i] = new List<Brick>();
            }

            for (int i = 0; i < players.Count(); i++) {
                if (i == playerIndex) continue;
                
                foreach (Brick brick in players[i].hand) {
                    if (brick.IsBrickPlayable()) {
                        playableBricks[i].Add(brick);
                    }
                }
            }

            return playableBricks;
        }

        private bool DrawABrick() {
            if (drawPile.Count > 0) {
                Brick newBrick = drawPile.DrawBrick();
                ReceiveBrick(newBrick);
                return true;
            }
            return false;
        }

        private static void AddClue() {
            if (clues == 8) return;
            clues += 1;
        }

        // Moves
        /* These methods returns true if the move was made 
         * and false if the player is not allowed to make the 
         * move or if the player already have made a move
         */
        public bool TrashABrick(int brickIndex = 0) {
            if (moves == turn) return false;
            
            Brick trashedBrick = RemoveBrick(brickIndex);
            trashPile.Add(trashedBrick);
            AddClue();

            DrawABrick();
            moves += 1;
            lastMoveDetails = $"Trashed {trashedBrick}";
            return true;
        }

        public bool PlayABrick(int brickIndex = 0) {
            if (moves == turn) return false;

            Brick playedBrick = RemoveBrick(brickIndex);

            if (playedBrick.IsBrickPlayable()) {
                score += 1;
                if (playedBrick.PeakNumber() == 5) AddClue();
                table.Add(playedBrick);
            } else {
                lifes -= 1;
                trashPile.Add(playedBrick);
            }
            DrawABrick();
            moves += 1;
            lastMoveDetails = $"Played {playedBrick}";
            return true;
        }

        public bool GiveAColorClueTo(Player player, Color color) {
            return GiveAClueTo(player, (int)color);
        }

        public bool GiveANumberClueTo(Player player, int number) {
            if (number < 1 || 5 < number) return false;
            return GiveAClueTo(player, number);
        }

        private bool GiveAClueTo(Player player, int clue) {

            if (moves == turn) return false;
            if (clues == 0) return false;

            int cluedBricks = 0;
            string clueAsString = "invalid clue";
            foreach (Brick brick in player.hand) {
                if ((int)brick.PeakColor() == clue) {
                    brick.gotColorClue = true;
                    cluedBricks++;
                    clueAsString = ((Brick.Color)clue).ColorToString();
                } else if (brick.PeakNumber() == clue) {
                    brick.gotNumberClue = true;
                    cluedBricks++;
                    clueAsString = clue.ToString();
                }
            }
            if (cluedBricks == 0) return false;

            clues -= 1;
            moves += 1;
            lastMoveDetails = $"Clue {clueAsString} to {player.playerIndex}";
            return true;
        }
    }
}
