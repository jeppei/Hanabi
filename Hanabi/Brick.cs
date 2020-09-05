﻿using System;
using System.Collections.Generic;
using static Hanabi.GlobalVariables;

namespace Hanabi {

    public class Brick {

        /* A brick can be converted to an int by converting the color to a in as 
         * specified in the enum and then adding the number. For example 32 
         * correspondes to the color red (30) and the number 2.
         * 
         * When giving a clue, to a brick, the clue value gets added to the clue.
         * For instance,
         *  - 0 means no clues
         *  - 40 means that the color is blue
         *  - 1 means that the number is 1
         *  - 25 means that the color yellow and the number is 5 
         */

        public enum Color { white=10, yellow=20, red=30, blue=40, green=50, unknown=0 };
        public static Color[] allColors = new Color[] { Color.white, Color.yellow, Color.red, Color.green, Color.blue };
        public static int[] Numbers = new int[] { 1, 1, 1, 2, 2, 3, 3, 4, 4, 5 };

        private readonly Color color;
        private readonly int number;

        private void VerifyPlayer() {
            if ((int)brickLocation == currentPlayerIndex) {
                lifes = 0;
                Console.WriteLine($"Player {currentPlayerIndex} peaked at her/his brick!");
            } else if (brickLocation == BrickLocation.DrawPile) {
                lifes = 0;
                Console.WriteLine($"Player {currentPlayerIndex} peaked at a brick in the draw pile!");
            }
        }

        public Color PeakColor() {
            if (gotColorClue) return color;
            VerifyPlayer();
            return color;
        }

        public int PeakNumber() {
            if (gotNumberClue) return number;
            VerifyPlayer();
            return number;
        }

        public bool gotNumberClue = false;
        public bool gotColorClue = false;

        /*
         * The player index indicates the location of the brick which 
         * could be 
         *  - the index of a player (0, 1, 2, 3, 4)
         *  - the draw pile (-1)  
         *  - the trash pile (-2)
         *  - the table (-3)
         */
        public enum BrickLocation {
            Unknown = -4,
            Table = -3,
            TrashPile = -2,
            DrawPile = -1,
            Player0 = 0,
            Player1 = 1,
            Player2 = 2,
            Player3 = 3,
            Player4 = 4
        }

        public BrickLocation brickLocation = BrickLocation.Unknown;

        public bool GotEnoughClues() => gotNumberClue && gotColorClue;

        public Brick(Color color, int number) {
            this.color = color;
            this.number = number;
        }

        public override string ToString() {
            return $"|{color}{number}|";
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

        public bool IsBrickPlayable() {

            if (!GotEnoughClues()) VerifyPlayer();

            if (table.Contains(this)) return false;
            if (number == 1) return true;
            return table.Contains(new Brick(color, number - 1));
        }

        internal static List<Brick> GenerateCompleteSetOfBricks() {
            List<Brick> bricks = new List<Brick>();
            foreach (Color color in allColors) {
                foreach (int i in Numbers) {
                    bricks.Add(new Brick(color, i));
                }
            }
            return bricks;
        }
    }
}
