using System;
using System.Collections.Generic;
using System.Text;
using static Hanabi.Game;

namespace Hanabi.PlayerClasses {
    public class Players : List<Player> {

        public Player GetPreviousPlayer() {
            int prevPlayerIndex = (currentPlayerIndex + 2) % Count;
            return this[prevPlayerIndex];
        }
    }
}
