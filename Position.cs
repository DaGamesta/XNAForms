﻿using System;
using Microsoft.Xna.Framework;

namespace XNAForms
{
    /// <summary>
    /// Represents a lattice point on the coordinate plane.
    /// </summary>
    public struct Position
    {
        /// <summary>
        /// Returns a position whose coordinates are both 1.
        /// </summary>
        public Position One
        {
            get
            {
                return new Position(1, 1);
            }
        }
        /// <summary>
        /// X coordinate of the position.
        /// </summary>
        public int X;
        /// <summary>
        /// Y coordinate of the position.
        /// </summary>
        public int Y;
        /// <summary>
        /// Returns a position whose coordinates are both 0.
        /// </summary>
        public Position Zero
        {
            get
            {
                return new Position(0, 0);
            }
        }

        /// <summary>
        /// Creates a new position.
        /// </summary>
        /// <param name="X">X coordinate of the new position.</param>
        /// <param name="Y">Y coordinate of the new position.</param>
        public Position(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        /// <summary>
        /// Determines if the position is equal to another position.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is Position))
            {
                return false;
            }
            Position pos = (Position)obj;
            return pos.X == X && pos.Y == Y;
        }
        /// <summary>
        /// Gets the hash code of the position.
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Adds two positions together.
        /// </summary>
        /// <param name="p1">First position.</param>
        /// <param name="p2">Second position.</param>
        public static Position operator +(Position p1, Position p2)
        {
            return new Position(p1.X + p2.X, p1.Y + p2.Y);
        }
        /// <summary>
        /// Subtracts two positions.
        /// </summary>
        /// <param name="p1">First position.</param>
        /// <param name="p2">Second position.</param>
        public static Position operator -(Position p1, Position p2)
        {
            return new Position(p1.X - p2.X, p1.Y - p2.Y);
        }
        /// <summary>
        /// Checks if two positions are equivalent.
        /// </summary>
        public static bool operator ==(Position p1, Position p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }
        /// <summary>
        /// Checks if two positions are not equivalent.
        /// </summary>
        public static bool operator !=(Position p1, Position p2)
        {
            return p1.X != p2.X || p1.Y != p2.Y;
        }
        /// <summary>
        /// Converts a position to a Vector2.
        /// </summary>
        /// <param name="p">Position to convert.</param>
        public static implicit operator Vector2(Position p)
        {
            return new Vector2(p.X, p.Y);
        }
    }
}
