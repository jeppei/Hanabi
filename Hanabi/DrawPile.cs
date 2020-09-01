using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using static Hanabi.Brick;

namespace Hanabi {
    public class DrawPile {
        readonly List<Brick> Bricks = new List<Brick>();
        public int Count => Bricks.Count;

        public DrawPile() {
            
            foreach (Color color in allColors) {
                foreach (int i in Numbers) {
                    Bricks.Add(new Brick(color, i));
                }
            }

            Bricks.Shuffle();
        }

        public Brick DrawBrick() {
            Brick theBrick = Bricks.Last();
            Bricks.RemoveAt(Bricks.Count - 1);
            return theBrick;
        }


        public Brick CheatDrawBrick(Brick brick) {
            Bricks.Remove(brick);
            return brick;
        }
    }
}
