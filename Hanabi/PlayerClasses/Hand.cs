using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hanabi.PlayerClasses {
    public class Hand : List<Brick> {
        public Brick GetBrick(Brick wantedBrick) {
            foreach (Brick brick in this) {
                if (brick.PeakColor() == wantedBrick.PeakColor() &&
                    brick.PeakNumber() == wantedBrick.PeakNumber()) {
                    return brick;
                }
            }
            return null;
        }

        public void OrderByPlayability() {
            var newOrder = this.OrderByDescending(b => b.brickPlayability).ToList();
            RemoveAll(b => true);
            foreach (Brick brick in newOrder) {
                Add(brick);
            }
        }

        public void OrderByTrashability() {
            var newOrder = this.OrderByDescending(b => b.brickTrashability).ToList();
            RemoveAll(b => true);
            foreach (Brick brick in newOrder) {
                Add(brick);
            }
        }

        public Hand Copy() {
            Hand copy = new Hand();
            foreach (Brick brick in this) {
                copy.Add(brick.Copy());
            }
            return copy;
        }
    }
}
