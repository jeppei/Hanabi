using System;
using System.Collections.Generic;
using System.Text;

namespace Hanabi {
    class GlobalVariables {

        public static int score = 0;
        public static int lifes = 3;
        public static int clues = 8;

        public static readonly Player[] players = new Player[] {
            new Player(0),
            new Player(1),
            new Player(2)
        };

        public static int currentPlayerIndex = 0;
        public static readonly int handSize = 5;

        public static int turn = 1;
        public static int moves = 0;
        public static int lastTurns = 0;

        public static readonly DrawPile drawPile = new DrawPile();
        public static readonly List<Brick> trashPile = new List<Brick>();
        public static readonly List<Brick> table = new List<Brick>();
    }
}
