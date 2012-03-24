using System;
using Microsoft.Xna.Framework;

namespace XNAForms
{
    /// <summary>
    /// Represents a lattice point on the coordinate plane.
    /// </summary>
    public struct Position
    {
        /// <summary>
        /// X coordinate of the position.
        /// </summary>
        public int X;
        /// <summary>
        /// Y coordinate of the position.
        /// </summary>
        public int Y;

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
        /// Converts a position to a Vector2.
        /// </summary>
        /// <param name="p">Position to convert.</param>
        public static implicit operator Vector2(Position p)
        {
            return new Vector2(p.X, p.Y);
        }
    }
}
