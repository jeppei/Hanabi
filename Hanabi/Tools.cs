using System;
using System.Collections.Generic;
using static Hanabi.Brick;

namespace Hanabi {
    public static class Tools {

        private static readonly Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list) {

            int n = list.Count;
            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static string BricksToString(this IEnumerable<Brick> bricks) {
            return string.Join(", ", bricks);
        }

        public static string ColorToString(this Color color) {
            if (color == Color.white) {
                return "white";
            } else if (color == Color.yellow) {
                return "yellow";
            } else if (color == Color.red) {
                return "red";
            } else if (color == Color.green) {
                return "green";
            } else if (color == Color.blue) {
                return "blue";
            } else {
                throw new Exception("Invalid color");
            }
        }
    }
}
