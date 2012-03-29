using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private int cPos;
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
        private int vPos;

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
            GUIHelper.FillRect(rectangle, new Color(32, 32, 32, alpha));
            GUIHelper.OutlineRect(rectangle, new Color(0, 0, 0, alpha));
            GUIHelper.Scissor(rectangle);
            GUIHelper.DrawStr(text, position + new Position(-vPos + 4, 2), new Color(255, 255, 255, alpha));
            GUIHelper.Unscissor();
            if (active && timer < 30)
            {
                GUIHelper.DrawLn(position + new Position(cPos, 2), position + new Position(cPos, size.height - 2), new Color(255, 255, 255, alpha));
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
                    while ((int)GUIHelper.StrSize(text.Substring(0, cIndex)).X - vPos + 4 < Input.mX - position.X - 4 && cIndex < text.Length)
                    {
                        cIndex++;
                    }
                    cPos = (int)GUIHelper.StrSize(text.Substring(0, cIndex)).X - vPos + 4;
                    if (onActivate != null)
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
                text += next;
                cIndex += next.Length;
                cPos += (int)GUIHelper.StrSize(next).X;
                if (next != "")
                {
                    timer = 0;
                }
                if (Input.TypeKey(Keys.Back) && cIndex != 0)
                {
                    timer = 0;
                    if (vPos > 0)
                    {
                        vPos -= (int)GUIHelper.StrSize(text[cIndex - 1].ToString()).X;
                    }
                    else
                    {
                        cPos -= (int)GUIHelper.StrSize(text[cIndex - 1].ToString()).X;
                    }
                    text = text.Remove(cIndex - 1, 1);
                    cIndex--;
                }
                if (Input.TypeKey(Keys.Delete) && cIndex != text.Length)
                {
                    timer = 0;
                    if (vPos > 0)
                    {
                        vPos -= (int)GUIHelper.StrSize(text[cIndex].ToString()).X;
                        cPos += (int)GUIHelper.StrSize(text[cIndex].ToString()).X;
                    }
                    text = text.Remove(cIndex, 1);
                }
                if (Input.TypeKey(Keys.Left) && cIndex != 0)
                {
                    timer = 0;
                    cPos -= (int)GUIHelper.StrSize(text[cIndex - 1].ToString()).X;
                    cIndex--;
                }
                else if (Input.TypeKey(Keys.Right) && cIndex != text.Length)
                {
                    timer = 0;
                    cPos += (int)GUIHelper.StrSize(text[cIndex].ToString()).X;
                    cIndex++;
                }
                if (Input.TappedKey(Keys.Enter) && onEnter != null)
                {
                    onEnter.Invoke(this, new EventArgs());
                }
            }
            if (cPos > size.width - 4)
            {
                vPos += cPos - size.width + 4;
                cPos = size.width - 4;
            }
            if (cPos < 4)
            {
                vPos -= 4 - cPos;
                cPos = 4;
            }
            vPos = vPos < 0 ? 0 : vPos;
        }
    }
}
