using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hanabi {
    public class Trash : List<Brick> {
        public bool ContainsAllBricksOfType(Brick brick) {

            int bricksOfTypeInTrash = this.Where(b => 
                b.PeakColor() == brick.PeakColor() && 
                b.PeakNumber() == brick.PeakNumber()
            ).Count();

            if (brick.PeakNumber() == 1) {
                if (bricksOfTypeInTrash == 3) return true;

            } else if (1 < brick.PeakNumber() && brick.PeakNumber() < 5) {
                if (bricksOfTypeInTrash == 2) return true;

            } else if (brick.PeakNumber() == 5) {
                if (bricksOfTypeInTrash == 1) return true;

            } else {
                throw new Exception("Invalid brick number");
            }

            return false;
        }
    }
}
