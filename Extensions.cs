using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNAForms
{
    /// <summary>
    /// Extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Clamps an integer between two values.
        /// </summary>
        public static int Clamp(this int i, int l, int u)
        {
            i = i >= l ? i : l;
            i = i <= u ? i : u;
            return i;
        }
        /// <summary>
        /// Gets if a rectangle is intersecting the mouse.
        /// </summary>
        public static bool IntersectsMouse(this Rectangle r)
        {
            return r.Intersects(new Rectangle(Input.mX, Input.mY, 1, 1));
        }
    }
}
