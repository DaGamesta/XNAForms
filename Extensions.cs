using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNAForms
{
    internal static class Extensions
    {
        internal static bool IntersectsMouse(this Rectangle r)
        {
            return r.Intersects(new Rectangle(Input.mX, Input.mY, 1, 1));
        }
    }
}
