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
        /// Gets the special keys that are active (such as up, left, down, right, etc).
        /// </summary>
        public static SpecialKeys active { get; private set; }
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
        private static int keys;
        private static int[] keysCode = new int[10];
        private static string[] keysStr = new string[10];
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
        /// The typed in string.
        /// </summary>
        public static string nextStr;
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
        private static int specials;
        private static int[] specialsCode = new int[10];
        /// <summary>
        /// Gets if a shift key is down.
        /// </summary>
        public static bool Shift { get { return K.IsKeyDown(Keys.LeftShift) || K.IsKeyDown(Keys.RightShift); } }

        static Input()
        {
            System.Windows.Forms.Application.AddMessageFilter(new KeyMessageFilter());
        }
        internal static void NextStr()
        {
            active = 0;
            if (Control && TappedKey(Keys.V))
            {
                if (System.Windows.Forms.Clipboard.ContainsText())
                {
                    nextStr = System.Windows.Forms.Clipboard.GetText();
                    return;
                }
            }
            string str = "";
            for (int i = 0; i < keys; i++)
            {
                if (keysCode[i] >= 32 && keysCode[i] != 127)
                {
                    str += keysStr[i];
                }
                switch (keysCode[i])
                {
                    case 8:
                        active |= SpecialKeys.BACK;
                        break;
                    case 13:
                        active |= SpecialKeys.ENTER;
                        break;
                }
            }
            for (int i = 0; i < specials; i++)
            {
                switch (specialsCode[i])
                {
                    case 37:
                        active |= SpecialKeys.LEFT;
                        break;
                    case 38:
                        active |= SpecialKeys.UP;
                        break;
                    case 39:
                        active |= SpecialKeys.RIGHT;
                        break;
                    case 40:
                        active |= SpecialKeys.DOWN;
                        break;
                    case 46:
                        active |= SpecialKeys.DELETE;
                        break;
                }
            }
            keys = specials = 0;
            nextStr = str;
        }
        /// <summary>
        /// Gets if a key is tapped.
        /// </summary>
        public static bool TappedKey(Keys key)
        {
            return K.IsKeyDown(key) && LastK.IsKeyUp(key);
        }
        internal static void Update()
        {
            LastK = K;
            K = Keyboard.GetState();
            LastM = M;
            M = Mouse.GetState();
        }
        private class KeyMessageFilter : System.Windows.Forms.IMessageFilter
        {
            [DllImport("user32.dll")]
            private static extern bool TranslateMessage(IntPtr ptr);

            public bool PreFilterMessage(ref System.Windows.Forms.Message m)
            {
                if (m.Msg == 256)
                {
                    if (specials < 10)
                    {
                        specialsCode[specials] = (char)m.WParam;
                        specials++;
                    }
                    IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(m));
                    Marshal.StructureToPtr(m, ptr, true);
                    TranslateMessage(ptr);
                }
                else if (m.Msg == 258)
                {
                    if (keys < 10)
                    {
                        keysCode[keys] = (char)m.WParam;
                        keysStr[keys] = ((char)m.WParam).ToString();
                        keys++;
                    }
                }
                return false;
            }
        }
    }
}
