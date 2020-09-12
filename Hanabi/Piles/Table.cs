using System.Collections.Generic;
using static Hanabi.Brick;

namespace Hanabi {
    public class Table : List<Brick> {

        public int GreatestBrickInColor(Color color) {
            int max = 0;
            foreach (Brick brick in this) {
                if (brick.PeakColor() == color || brick.PeakNumber() > max) {
                    max = brick.PeakNumber();
                }
            }
            return max;
        }
    }
}
