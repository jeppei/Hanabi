using System;
using System.Collections.Generic;
using System.Text;

namespace Hanabi.PlayerClasses {

    public class MoveDetails {
        public enum Move { TRASH, PLAY, CLUE }
        public int turn;
        public Move move;
        public Brick brick; // if trash or play
        public int clue;
        public Hand handAfterMove;
        public string handAfterMoveStr;
    }
}
