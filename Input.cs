using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using System.Runtime.InteropServices;

namespace XNAForms
{
    internal static class Input
    {
        internal static bool Control
        {
            get
            {
                return K.IsKeyDown(Keys.LeftControl) || K.IsKeyDown(Keys.RightControl);
            }
        }
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
        private static KeyboardState K;
        internal static int[] KeyCD;
        internal static Array KeysArr;
        private static KeyboardState LastK;
        private static MouseState LastM;
        private static MouseState M;
        internal static int mDS
        {
            get
            {
                return M.ScrollWheelValue - LastM.ScrollWheelValue;
            }
        }
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
        internal static bool Shift
        {
            get
            {
                return K.IsKeyDown(Keys.LeftShift) || K.IsKeyDown(Keys.RightShift);
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        private static extern short GetKeyState(int keyCode);
        internal static string NextStr()
        {
            if (TypeKey(Keys.Space))
                return " ";
            if (Control)
            {
                if (TappedKey(Keys.V))
                {
                    if (System.Windows.Forms.Clipboard.ContainsText())
                    {
                        return System.Windows.Forms.Clipboard.GetText();
                    }
                }
                return "";
            }
            bool shift = Shift ^ (((ushort)GetKeyState(20)) & 0xffff) != 0;
            bool noCapsShift = Shift;
            if (TypeKey(Keys.A))
                return shift ? "A" : "a";
            if (TypeKey(Keys.B))
                return shift ? "B" : "b";
            if (TypeKey(Keys.C))
                return shift ? "C" : "c";
            if (TypeKey(Keys.D))
                return shift ? "D" : "d";
            if (TypeKey(Keys.E))
                return shift ? "E" : "e";
            if (TypeKey(Keys.F))
                return shift ? "F" : "f";
            if (TypeKey(Keys.G))
                return shift ? "G" : "g";
            if (TypeKey(Keys.H))
                return shift ? "H" : "h";
            if (TypeKey(Keys.I))
                return shift ? "I" : "i";
            if (TypeKey(Keys.J))
                return shift ? "J" : "j";
            if (TypeKey(Keys.K))
                return shift ? "K" : "k";
            if (TypeKey(Keys.L))
                return shift ? "L" : "l";
            if (TypeKey(Keys.M))
                return shift ? "M" : "m";
            if (TypeKey(Keys.N))
                return shift ? "N" : "n";
            if (TypeKey(Keys.O))
                return shift ? "O" : "o";
            if (TypeKey(Keys.P))
                return shift ? "P" : "p";
            if (TypeKey(Keys.Q))
                return shift ? "Q" : "q";
            if (TypeKey(Keys.R))
                return shift ? "R" : "r";
            if (TypeKey(Keys.S))
                return shift ? "S" : "s";
            if (TypeKey(Keys.T))
                return shift ? "T" : "t";
            if (TypeKey(Keys.U))
                return shift ? "U" : "u";
            if (TypeKey(Keys.V))
                return shift ? "V" : "v";
            if (TypeKey(Keys.W))
                return shift ? "W" : "w";
            if (TypeKey(Keys.X))
                return shift ? "X" : "x";
            if (TypeKey(Keys.Y))
                return shift ? "Y" : "y";
            if (TypeKey(Keys.Z))
                return shift ? "Z" : "z";
            if (TypeKey(Keys.D0))
                return noCapsShift ? ")" : "0";
            if (TypeKey(Keys.D1))
                return noCapsShift ? "!" : "1";
            if (TypeKey(Keys.D2))
                return noCapsShift ? "@" : "2";
            if (TypeKey(Keys.D3))
                return noCapsShift ? "#" : "3";
            if (TypeKey(Keys.D4))
                return noCapsShift ? "$" : "4";
            if (TypeKey(Keys.D5))
                return noCapsShift ? "%" : "5";
            if (TypeKey(Keys.D6))
                return noCapsShift ? "^" : "6";
            if (TypeKey(Keys.D7))
                return noCapsShift ? "&" : "7";
            if (TypeKey(Keys.D8))
                return noCapsShift ? "*" : "8";
            if (TypeKey(Keys.D9))
                return noCapsShift ? "(" : "9";
            if (TypeKey(Keys.OemComma))
                return noCapsShift ? "<" : ",";
            if (TypeKey(Keys.OemPeriod))
                return noCapsShift ? ">" : ".";
            if (TypeKey(Keys.OemQuestion))
                return noCapsShift ? "?" : "/";
            if (TypeKey(Keys.OemOpenBrackets))
                return noCapsShift ? "{" : "[";
            if (TypeKey(Keys.OemCloseBrackets))
                return noCapsShift ? "}" : "]";
            if (TypeKey(Keys.OemSemicolon))
                return noCapsShift ? ":" : ";";
            if (TypeKey(Keys.OemQuotes))
                return noCapsShift ? "\"" : "'";
            if (TypeKey(Keys.OemMinus))
                return noCapsShift ? "_" : "-";
            if (TypeKey(Keys.OemPlus))
                return noCapsShift ? "+" : "=";
            if (TypeKey(Keys.OemPipe))
                return noCapsShift ? "|" : "\\";
            if (TypeKey(Keys.OemTilde))
                return noCapsShift ? "~" : "`";
            return "";
        }
        internal static bool TappedKey(Keys key)
        {
            return K.IsKeyDown(key) && LastK.IsKeyUp(key);
        }
        internal static bool TypeKey(Keys key)
        {
            if (TappedKey(key))
            {
                return true;
            }
            for (int i = 0; i < KeysArr.Length; i++)
            {
                if ((Keys)KeysArr.GetValue(i) == key)
                {
                    return KeyCD[i] == 0;
                }
            }
            return false;
        }
        internal static void Update()
        {
            LastK = K;
            K = Keyboard.GetState();
            LastM = M;
            M = Mouse.GetState();
            for (int i = 0; i < KeyCD.Length; i++)
            {
                if (K.IsKeyDown((Keys)KeysArr.GetValue(i)))
                {
                    if (KeyCD[i] > 0)
                    {
                        KeyCD[i]--;
                    }
                }
                else
                {
                    KeyCD[i] = 40;
                }
            }
        }
    }
}
