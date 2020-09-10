using System;
using System.Collections.Generic;

namespace Hanabi {
    class GlobalVariables {

        public static void ResetValues() {
            score = 0;
            lifes = 3;
            clues = 8;
            players = CreatePlayers();

            handSize = (players.Length == 2) ? 5 :
                       (players.Length == 3) ? 5 :
                       (players.Length == 4) ? 4 :
                       (players.Length == 5) ? 4 : -1;

            if (handSize == -1) throw new Exception("The number of players must be 2, 3, 4 or 5");

            currentPlayerIndex = 0;
            turn = 1;
            moves = 0;
            lastTurns = 0;
            lastMoveDetails = "";

            drawPile = new DrawPile();
            trashPile = new List<Brick>();
            table = new List<Brick>();
        }

        private static Player[] CreatePlayers() {
            Player[] thePlayers = new Player[numberOfPlayers];
            for (int p = 0; p < numberOfPlayers; p++) {
                thePlayers[p] = new Player(p);
            }
            return thePlayers;
        }
        
        public static bool cheat = false;
        public static bool printInConsole = false;
        public static int iterations = 200;
        public static int numberOfPlayers = 3;

        public static int score;
        public static int lifes;
        public static int clues;

        public static Player[] players;

        public static int currentPlayerIndex;
        public static int handSize;

        public static int turn;
        public static int moves;
        public static int lastTurns;

        public static string lastMoveDetails;

        public static DrawPile drawPile;
        public static List<Brick> trashPile;
        public static List<Brick> table;
    }
}
