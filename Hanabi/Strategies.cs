using System;
using System.Collections.Generic;
using System.Text;
using static Hanabi.GlobalVariables;
using static Hanabi.Program;

namespace Hanabi {
    public class Strategies {

        public static void MakeATurn(Player currentPlayer) {


            if (lifes > 1) {
                PlayABrick(currentPlayer);
            } else {
                TrashABrick(currentPlayer);
            }

        }
    }
}
