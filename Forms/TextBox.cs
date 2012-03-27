using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XNAForms.Forms
{
    /// <summary>
    /// Represents a textbox in where text may be typed into it.
    /// </summary>
    public class TextBox : Control
    {
        private bool active;
        private int cIndex;
        private int letters;
        /// <summary>
        /// Fires when the textbox is activated.
        /// </summary>
        public event ControlEventHandler onActivate;
        /// <summary>
        /// Fires when the textbox is active and the enter key is pressed.
        /// </summary>
        public event ControlEventHandler onEnter;
        private byte timer;
        private int vIndex;

        /// <summary>
        /// Creates a new textbox.
        /// </summary>
        /// <param name="position">Position for the new textbox.</param>
        /// <param name="size">Size for the new textbox.</param>
        /// <param name="def">Default string for the new textbox.</param>
        public TextBox(Position position, int size, string def = "")
            : base(position, new Size(size, (int)GUIHelper.StrSize("A").Y + 4))
        {
            text = def;
        }

        internal override void Draw()
        {
            GUIHelper.FillRect(rectangle, new Color(26, 26, 26, alpha));
            GUIHelper.OutlineRect(rectangle, new Color(0, 0, 0, alpha));
            GUIHelper.DrawStr(text.Substring(vIndex, letters), position + new Position(4, 2), new Color(255, 255, 255, alpha));
            if (active && timer < 30)
            {
                int x = (int)GUIHelper.StrSize(text.Substring(vIndex, cIndex)).X + 5;
                GUIHelper.DrawLn(new Position(position.X + x, position.Y + 2), new Position(position.X + x, position.Y + size.height - 2), new Color(255, 255, 255, alpha));
            }
        }
        internal override void Update()
        {
            if (rectangle.IntersectsMouse())
            {
                GUI.SetCursor(CursorType.TEXT);
            }
            if (Input.LeftC)
            {
                active = rectangle.IntersectsMouse();
                if (active)
                {
                    cIndex = 0;
                    while ((int)GUIHelper.StrSize(text.Substring(vIndex, cIndex)).X + 4 < Input.mX - position.X - 4 && cIndex < text.Length - vIndex)
                    {
                        cIndex++;
                    }
                    if (Input.TappedKey(Keys.Enter) && onActivate != null)
                    {
                        onActivate.Invoke(this, new EventArgs());
                    }
                }
            }
            if (active)
            {
                timer++;
                timer %= 60;
                string next = Input.NextStr();
                text = text.Substring(0, cIndex + vIndex) + next + text.Substring(cIndex + vIndex);
                cIndex += next.Length;
                if (next != "")
                {
                    timer = 0;
                }
                if (Input.TypeKey(Keys.Back) && cIndex + vIndex != 0)
                {
                    timer = 0;
                    text = text.Remove(cIndex + vIndex - 1, 1);
                    if (vIndex == 0)
                    {
                        cIndex--;
                    }
                    else
                    {
                        vIndex--;
                    }
                }
                if (Input.TypeKey(Keys.Delete) && text.Length > 0 && cIndex + vIndex < text.Length)
                {
                    timer = 0;
                    text = text.Remove(cIndex + vIndex, 1);
                }
                if (Input.TypeKey(Keys.Left))
                {
                    timer = 0;
                    cIndex--;
                }
                else if (Input.TypeKey(Keys.Right))
                {
                    timer = 0;
                    cIndex++;
                }
                if (Input.TappedKey(Keys.Enter) && onEnter != null)
                {
                    onEnter.Invoke(this, new EventArgs());
                }
            }
            letters = 0;
            try
            {
                while ((int)GUIHelper.StrSize(text.Substring(vIndex, letters)).X + 4 < size.width - 4 && letters < text.Length)
                {
                    letters++;
                }
            }
            catch
            {
            }
            if (cIndex < 0)
            {
                vIndex += cIndex;
                cIndex = 0;
            }
            if (cIndex >= letters)
            {
                vIndex += cIndex - letters;
                cIndex = letters;
            }
            cIndex = cIndex > text.Length ? text.Length : cIndex;
            vIndex = vIndex < 0 ? 0 : vIndex;
            if (text.Length > letters)
            {
                vIndex = vIndex > text.Length - letters ? text.Length - letters : vIndex;
            }
            vIndex = vIndex + letters > text.Length ? 0 : vIndex;
        }
    }
}
