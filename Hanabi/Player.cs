using System;
using System.Collections.Generic;
using System.Linq;
using static Hanabi.Brick;
using static Hanabi.GlobalVariables;

namespace Hanabi {

    public class Player {

        public List<Brick> hand = new List<Brick>();

        public int playerIndex;
        public override string ToString() => string.Join(", ", hand);
        public string ToStringWithClues() => string.Join(", ", hand.Select(b => b.ToStringWithClues()));

        public Player(int playerIndex) {
            this.playerIndex = playerIndex;
        }

        public void ReceiveBrick(Brick receivedBrick, int brickIndex = -1) {
            receivedBrick.brickLocation = (BrickLocation)playerIndex;
            receivedBrick.Age = 0;
            brickIndex = (brickIndex == -1) ? hand.Count : brickIndex;
            hand.Insert(brickIndex, receivedBrick);
        }

        private Brick RemoveBrick(int i, BrickLocation brickLocation) {
            Brick brickToRemove = hand[i];
            brickToRemove.brickLocation = brickLocation;
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
            
            Brick trashedBrick = RemoveBrick(brickIndex, BrickLocation.TrashPile);
            trashPile.Add(trashedBrick);
            AddClue();

            DrawABrick();
            moves += 1;
            lastMoveDetails = $"Trashed {trashedBrick}";
            return true;
        }

        public bool PlayABrick(int brickIndex = 0) {
            if (moves == turn) return false;

            Brick playedBrick = RemoveBrick(brickIndex, BrickLocation.Unknown);

            if (playedBrick.IsBrickPlayable()) {
                score += 1;
                if (playedBrick.PeakNumber() == 5) AddClue();
                playedBrick.brickLocation = BrickLocation.Table;
                table.Add(playedBrick);
            } else {
                lifes -= 1;
                playedBrick.brickLocation = BrickLocation.TrashPile;
                trashPile.Add(playedBrick);
            }

            DrawABrick();
            moves += 1;
            lastMoveDetails = $"Played {playedBrick}";
            return true;
        }

        public void PlayerCheck(Player player) {
            if (currentPlayerIndex == player.playerIndex) {
                throw new Exception("You are not allowed to give a clue to yourself!");
            }
        }

        public bool GiveAColorClueTo(Player player, Color color) {
            if (color == Color.unknown) return false;
            return GiveAClueTo(player, (int)color);
        }

        public bool GiveANumberClueTo(Player player, int number) {
            if (number < 1 || 5 < number) return false;
            return GiveAClueTo(player, number);
        }

        public bool GiveAClueTo(Player player, int clue) {
            PlayerCheck(player);

            if (moves == turn) return false;
            if (clues == 0) return false;

            int cluedBricks = 0;
            string clueAsString = "invalid clue";

            Brick brickWhoGotTheLastClue = null;
            foreach (Brick brick in player.hand) {
                if (IsColorClue(clue)) { 

                    if ((int)brick.PeakColor() == clue) {
                        brick.gotColorClue = true;
                        cluedBricks++;
                        clueAsString = ((Color)clue).ColorToString();
                        brickWhoGotTheLastClue = brick;
                    } else {
                        // Give anti clue
                        brick.antiColorClues.Add((Color)clue);
                    }
                
                } else if (IsNumberClue(clue)) {
                    if (brick.PeakNumber() == clue) {
                        brick.gotNumberClue = true;
                        cluedBricks++;
                        clueAsString = clue.ToString();
                        brickWhoGotTheLastClue = brick;
                    } else {
                        // Give anti clue
                        brick.antiNumberClues.Add(clue);
                    }
                }
            }

            if (cluedBricks == 0) {
                throw new Exception($"The user gaved an invalid clue! Clue = {clue}");
            } else {
                brickWhoGotTheLastClue.numberOfBricksWhoGotSameClue = cluedBricks;
            }

            clues -= 1;
            moves += 1;
            lastMoveDetails = $"Clue {clueAsString} to {player.playerIndex}";
            return true;
        }

        public List<Brick> GetVisibleBricks() {
            List<Brick> visibleBricks = new List<Brick>();

            foreach (Player player in players) {
                if (player.playerIndex == currentPlayerIndex) continue;

                visibleBricks.AddRange(player.hand);
            }

            visibleBricks.AddRange(trashPile);
            visibleBricks.AddRange(table);

            return visibleBricks;
        }

        public List<Brick> GetUnvisibleBricks() {
            List<Brick> unvisible = Brick.GenerateCompleteSetOfBricks();
            foreach (Brick brick in GetVisibleBricks()) unvisible.Remove(brick);
            return unvisible;
        }

        public float CalculateBrickPlayability(Brick brick) => CalculateAbility(brick, Ability.play);
        public float CalculateBrickTrashability(Brick brick) => CalculateAbility(brick, Ability.trash);

        private enum Ability { trash, play }
        private float CalculateAbility(Brick brick, Ability ability) {
            if (brick.brickLocation != (BrickLocation)this.playerIndex) {
                throw new NotImplementedException("This is not implemented");
            }

            List<Brick> possibleBricks = this.GetUnvisibleBricks();
            if (brick.gotColorClue) {
                possibleBricks = possibleBricks.Where(b => b.PeakColor() == brick.PeakColor()).ToList();
            }
            if (brick.gotNumberClue) {
                possibleBricks = possibleBricks.Where(b => b.PeakNumber() == brick.PeakNumber()).ToList();
            }

            foreach (Color antiClue in brick.antiColorClues) {
                possibleBricks = possibleBricks.Where(b => b.PeakColor() != antiClue).ToList();
            }

            foreach (int antiClue in brick.antiColorClues) {
                possibleBricks = possibleBricks.Where(b => b.PeakNumber() != antiClue).ToList();
            }

            float bricksWithAbility = 0;
            foreach (Brick possibleBrick in possibleBricks) {
                if (ability == Ability.trash) {
                    if (possibleBrick.IsBrickTrashable()) bricksWithAbility++;
                } else if (ability == Ability.play) {
                    if (possibleBrick.IsBrickPlayable()) bricksWithAbility++;
                } else {
                    throw new Exception("Not implemented");
                }
            }

            if (ability == Ability.trash) {
                brick.brickTrashability = bricksWithAbility / possibleBricks.Count();
                return brick.brickTrashability;

            } else if (ability == Ability.play) {
                brick.brickPlayability = bricksWithAbility / possibleBricks.Count();
                return brick.brickPlayability;

            } else {
                throw new Exception("Not implemented");
            }
        }

        public int NumberOfBricksWithThisClue(int clue) {
            PlayerCheck(this);
            int bricksMatchingClue = 0;
            foreach (Brick brick in hand) {
                if ((int)brick.PeakColor() == clue) bricksMatchingClue++;
                if ((int)brick.PeakNumber() == clue) bricksMatchingClue++;
            }
            return bricksMatchingClue;
        }
    }
}
