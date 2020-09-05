using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using static Hanabi.Brick;

namespace Hanabi {
    public class DrawPile {
        readonly List<Brick> Bricks;
        public int Count => Bricks.Count;

        public DrawPile(bool shuffled = true) {

            Bricks = Brick.GenerateCompleteSetOfBricks();
            foreach (Brick brick in Bricks) brick.brickLocation = BrickLocation.DrawPile;

            if (shuffled) Bricks.Shuffle();
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
