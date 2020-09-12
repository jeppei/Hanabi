using Hanabi.PlayerClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hanabi {
    public class Game {

        public static int numberOfPlayers = 3;
        public static int iterations = 200; // must be at least 50
        public static bool cheat = false;

        public static int score;
        public static int lifes;
        public static int clues;
        public static Players players;

        public delegate void Strategy();

        readonly Strategy MakeAMove = Strategies.Strategy.PlayOldestBricksWIthClues;

        public int handSize = (numberOfPlayers == 2) ? 5 :
                              (numberOfPlayers == 3) ? 5 :
                              (numberOfPlayers == 4) ? 4 :
                              (numberOfPlayers == 5) ? 4 : -1;

        public int lastTurns = 0;
        public GameHistory history = new GameHistory();

        public static int moves;
        public static int turn;

        public static string lastMoveDetails = "";
        public static string lastMoveThinking = "";
        public static int currentPlayerIndex;
        public static Player currentPlayer => players[currentPlayerIndex];

        public static Trash trash;
        public static Table table;
        public static DrawPile drawPile;

        public Game() {
            if (handSize == -1) {
                throw new Exception("The number of players must be 2, 3, 4 or 5");
            }

            currentPlayerIndex = 0;
            turn = 1;
            moves = 0;

            score = 0;
            lifes = 3;
            clues = 8;

            trash = new Trash();
            table = new Table();
            drawPile = new DrawPile();

            players = CreatePlayers();
            DealBricks();
            history.AddGameStats();
        }

        private static Players CreatePlayers() {
            Players thePlayers = new Players();
            for (int p = 0; p < numberOfPlayers; p++) {
                thePlayers.Add(new Player(p));
            }
            return thePlayers;
        }

        private void DealBricks() {
            foreach (Player player in players) {
                while (player.hand.Count < handSize) {
                    Brick brick = drawPile.DrawBrick();
                    player.ReceiveBrick(brick);
                }
            }
        }

        public void Play() {
            while (true) {

                if (IsTheGameOver()) break;

                UpdateBrickStats();

                history.AddCurrentBrickStats();

                MakeAMove();
                VerifyThatThePlayerMadeAMove();

                history.AddPlayersMove();
                history.AddGameStats();
                if (lifes == 0) score = 0;

                GoToNextPlayer();
            }
            history.AddLine("THE GAME IS OVER");
        }
        
        private bool IsTheGameOver() {
            if (lastTurns == players.Count()) return true;
            if (score == 25) return true;
            if (lifes == 0) return true;

            if (drawPile.Count == 0) lastTurns += 1;

            return false;
        }

        private void UpdateBrickStats() {
            Player currentPlayer = players[currentPlayerIndex];
            foreach (Brick brick in currentPlayer.hand) {
                currentPlayer.CalculateBrickPlayability(brick);
                currentPlayer.CalculateBrickTrashability(brick);
                currentPlayer.CalculateBrickImportance(brick);
                brick.HandAge++;
                if (brick.gotColorClue || brick.gotNumberClue) brick.ClueAge++;
            }
            foreach (Brick brick in table) brick.TableAge++;
            lastMoveThinking = "";

            List<Brick>[] allPlayableBricks = currentPlayer.LookForPlayableBricks();
            // Each playable brick can be given one of two clues (number or color). We want a dictionary where 
            // The index is the number of bricks that matches a clue, and the value list of tuples. Item1 in the tupele is 
            // the clue and item2 is which player the clue is for. In short Dict[BricksMatchingClue] = (Clue, Player)
            currentPlayer.CalculatePossibleCluesToGive();
        }

        private void VerifyThatThePlayerMadeAMove() {
            if (turn > moves) {
                Console.WriteLine("The player did not make a move!");
                throw new Exception("The player did not make a move!");
            } else if (turn < moves) {
                Console.WriteLine("The player managed to make more than 1 move!");
                throw new Exception("The player managed to make more than 1 move!");
            }
        }

        private void GoToNextPlayer() {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count();
            turn++;
        }
    }
}
