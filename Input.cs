using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace XNAForms
{
    /// <summary>
    /// Manages input.
    /// </summary>
    public static class Input
    {
        /// <summary>
        /// Gets if a control key is down.
        /// </summary>
        public static bool Control { get { return K.IsKeyDown(Keys.LeftControl) || K.IsKeyDown(Keys.RightControl); } }
        /// <summary>
        /// Gets if the LMB is clicked.
        /// </summary>
        public static bool LeftC { get { return M.LeftButton == ButtonState.Pressed && LastM.LeftButton == ButtonState.Released; } }
        /// <summary>
        /// Gets if the LMB is down.
        /// </summary>
        public static bool LeftD { get { return M.LeftButton == ButtonState.Pressed; } }
        /// <summary>
        /// Gets if the LMB is released.
        /// </summary>
        public static bool LeftR { get { return M.LeftButton == ButtonState.Released && LastM.LeftButton == ButtonState.Pressed; } }
        private static KeyboardState K;
        internal static int[] KeyCD;
        private static event Action<Char> keyEvent;
        private static int keys;
        private static int[] keysCode = new int[10];
        private static string[] keysStr = new string[10];
        internal static Array KeysArr;
        private static KeyboardState LastK;
        private static MouseState LastM;
        private static MouseState M;
        /// <summary>
        /// Gets the scroll wheel value change.
        /// </summary>
        public static int mDS { get { return M.ScrollWheelValue - LastM.ScrollWheelValue; } }
        internal static int mDX { get { return M.X - LastM.X; } }
        internal static int mDY { get { return M.Y - LastM.Y; } }
        /// <summary>
        /// Gets the X position of the mouse.
        /// </summary>
        public static int mX { get { return M.X; } }
        /// <summary>
        /// Gets the Y position of the mouse.
        /// </summary>
        public static int mY { get { return M.Y; } }
        /// <summary>
        /// Gets if the RMB is clicked.
        /// </summary>
        public static bool RightC { get { return M.RightButton == ButtonState.Pressed && LastM.RightButton == ButtonState.Released; } }
        /// <summary>
        /// Gets if the RMB is down.
        /// </summary>
        public static bool RightD { get { return M.RightButton == ButtonState.Pressed; } }
        /// <summary>
        /// Gets if the RMB is released.
        /// </summary>
        public static bool RightR { get { return M.RightButton == ButtonState.Released && LastM.RightButton == ButtonState.Pressed; } }
        /// <summary>
        /// Gets if a shift key is down.
        /// </summary>
        public static bool Shift { get { return K.IsKeyDown(Keys.LeftShift) || K.IsKeyDown(Keys.RightShift); } }

        static Input()
        {
            System.Windows.Forms.Application.AddMessageFilter(new KeyMessageFilter());
            keyEvent += c =>
                {
                    if (keys < 10)
                    {
                        keysCode[keys] = c;
                        keysStr[keys] = c.ToString();
                        keys++;
                    }
                };
        }
        internal static void ClearStr()
        {
            keys = 0;
        }
        /// <summary>
        /// Gets the next string that is inputted.
        /// </summary>
        public static string NextStr()
        {
            string str = "";
            for (int i = 0; i < keys; i++)
            {
                if (keysCode[i] >= 32 && keysCode[i] != 127)
                {
                    str += keysStr[i];
                }
            }
            keys = 0;
            return str;
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
                    KeyCD[i] = 50;
                }
            }
        }
        private class KeyMessageFilter : System.Windows.Forms.IMessageFilter
        {
            [DllImport("user32.dll")]
            private static extern bool TranslateMessage(IntPtr ptr);

            public bool PreFilterMessage(ref System.Windows.Forms.Message m)
            {
                if (m.Msg == 256)
                {
                    IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(m));
                    Marshal.StructureToPtr(m, ptr, true);
                    TranslateMessage(ptr);
                }
                else if (m.Msg == 258)
                {
                    if (keyEvent != null)
                        keyEvent.Invoke((char)m.WParam);
                }
                return false;
            }
        }
    }
}
