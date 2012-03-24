using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNAForms
{
    internal static class Extensions
    {
        internal static int Clamp(this int i, int l, int u)
        {
            i = i >= l ? i : l;
            i = i <= u ? i : u;
            return i;
        }
        internal static bool IntersectsMouse(this Rectangle r)
        {
            return r.Intersects(new Rectangle(Input.mX, Input.mY, 1, 1));
        }
    }
}
