using System.Collections.Generic;

namespace Hanabi {

    public class Brick {

        public enum Color { white=10, yellow=20, red=30, blue=40, green=50 };
        public static int[] Numbers = new int[] { 1, 1, 1, 2, 2, 3, 3, 4, 4, 5 };

        public Color color;
        public int number;

        public Brick(Color color, int number) {
            this.color = color;
            this.number = number;
        }

        public override string ToString() {
            return $"[{color} {number}]";
        }

        public override bool Equals(object obj) {

            if (obj.GetType() != typeof(Brick)) return false;

            Brick brickObj = (Brick)obj;

            return brickObj.color == this.color && 
                   brickObj.number == this.number;
        }

        public override int GetHashCode() {
            return (int)color + number;
        }

        public static string BricksAsString(IEnumerable<Brick> bricks) {
            return string.Join(", ", bricks);
        }
    }

}
