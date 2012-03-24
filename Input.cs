using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace XNAForms
{
    internal static class Input
    {
        internal static bool LeftC
        {
            get
            {
                return M.LeftButton == ButtonState.Pressed && LastM.LeftButton == ButtonState.Released;
            }
        }
        internal static bool LeftD
        {
            get
            {
                return M.LeftButton == ButtonState.Pressed;
            }
        }
        private static MouseState LastM;
        private static MouseState M;
        internal static int mDX
        {
            get
            {
                return M.X - LastM.X;
            }
        }
        internal static int mDY
        {
            get
            {
                return M.Y - LastM.Y;
            }
        }
        internal static int mX
        {
            get
            {
                return M.X;
            }
        }
        internal static int mY
        {
            get
            {
                return M.Y;
            }
        }

        internal static void Update()
        {
            LastM = M;
            M = Mouse.GetState();
        }
    }
}
