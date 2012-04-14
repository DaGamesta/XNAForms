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
        private int cIndex;
        private int cPos;
        private int hIndex;
        private bool hl;
        private int hPos = 4;
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
        /// <summary>
        /// Clears the textbox.
        /// </summary>
        public void Clear()
        {
            cIndex = cPos = hIndex = vPos = 0;
            hPos = 4;
            text = "";
        }
        internal override void Draw()
        {
            Rectangle highlighted = new Rectangle(position.X + hPos - vPos, position.Y + 2, cPos + vPos - hPos, size.height - 4);
            if (hPos > cPos + vPos)
            {
                highlighted = new Rectangle(position.X + cPos, position.Y + 2, hPos - cPos - vPos, size.height - 4);
            }
            GUIHelper.FillRect(rectangle, new Color(32, 32, 32, alpha));
            GUIHelper.OutlineRect(rectangle, new Color(0, 0, 0, alpha));
            GUIHelper.Scissor(Rectangle.Intersect(GUIHelper.sb.GraphicsDevice.Viewport.Bounds, rectangle));
            GUIHelper.DrawStr(text, position + new Position(-vPos + 4, 2), new Color(255, 255, 255, alpha));
            GUIHelper.FillRect(highlighted, new Color(25, 75, 125, 100));
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
                hl = rectangle.IntersectsMouse();
            }
            if (Input.LeftD || Input.RightC)
            {
                active = rectangle.IntersectsMouse();
                if (hl && Input.LeftD)
                {
                    cIndex = 0;
                    while ((int)GUIHelper.StrSize(text.Substring(0, cIndex)).X - vPos + 4 < Input.mX - position.X - 4 && cIndex < text.Length)
                    {
                        cIndex++;
                    }
                    cPos = (int)GUIHelper.StrSize(text.Substring(0, cIndex)).X - vPos + 4;
                    if (Input.mX > rectangle.Right)
                    {
                        cIndex++;
                    }
                    if (Input.LeftC && active)
                    {
                        hIndex = cIndex;
                        hPos = cPos + vPos;
                        if (onActivate != null)
                        {
                            onActivate.Invoke(this, new EventArgs());
                        }
                    }
                }
            }
            if (active)
            {
                timer++;
                timer %= 60;
                string next = Input.nextStr;
                if (next != "")
                {
                    if (hPos < cPos)
                    {
                        text = text.Substring(0, hIndex) + next + text.Substring(cIndex);
                        cIndex += next.Length - Math.Abs(hIndex - cIndex);
                    }
                    else
                    {
                        text = text.Substring(0, cIndex) + next + text.Substring(hIndex);
                        cIndex += next.Length;
                    }
                    int diff = (int)GUIHelper.StrSize(text.Substring(0, cIndex)).X - cPos - vPos;
                    cPos += diff + 4;
                    timer = 0;
                    hIndex = cIndex;
                    hPos = cPos + vPos;
                }
                if ((Input.active & SpecialKeys.BACK) != 0 && !(hIndex == 0 && cIndex == 0))
                {
                    timer = 0;
                    if (cIndex == hIndex)
                    {
                        hIndex = cIndex - 1;
                        hPos = cPos - 1;
                    }
                    int diff = 0;
                    if (hPos < cPos)
                    {
                        diff = (int)GUIHelper.StrSize(text.Substring(0, cIndex)).X - (int)GUIHelper.StrSize(text.Substring(0, hIndex)).X;
                        text = text.Substring(0, hIndex) + text.Substring(cIndex);
                        cIndex -= Math.Abs(cIndex - hIndex);
                    }
                    else
                    {
                        text = text.Substring(0, cIndex) + text.Substring(hIndex);
                    }
                    if (vPos > 0)
                    {
                        vPos -= diff;
                    }
                    else
                    {
                        cPos -= diff;
                    }
                    hIndex = cIndex;
                    hPos = cPos + vPos;
                }
                if ((Input.active & SpecialKeys.DELETE) != 0 && !(cIndex == text.Length && hIndex == text.Length))
                {
                    timer = 0;
                    if (cIndex == hIndex)
                    {
                        hIndex = cIndex + 1;
                        hPos = cPos + 1;
                    }
                    int diff = 0;
                    if (hPos < cPos)
                    {
                        diff = (int)GUIHelper.StrSize(text.Substring(0, cIndex)).X - (int)GUIHelper.StrSize(text.Substring(0, hIndex)).X;
                        text = text.Substring(0, hIndex) + text.Substring(cIndex);
                        cIndex -= Math.Abs(cIndex - hIndex);
                    }
                    else
                    {
                        text = text.Substring(0, cIndex) + text.Substring(hIndex);
                    }
                    if (vPos > 0)
                    {
                        vPos -= diff;
                    }
                    else
                    {
                        cPos -= diff;
                    }
                    hIndex = cIndex;
                    hPos = cPos + vPos;
                }
                if (((Input.active & SpecialKeys.LEFT) != 0 || (Input.LeftD && Input.mX > rectangle.Right)) && cIndex != 0)
                {
                    timer = 0;
                    int diff = (int)GUIHelper.StrSize(text.Substring(0, cIndex)).X - (int)GUIHelper.StrSize(text.Substring(0, cIndex - 1)).X;
                    cPos -= diff;
                    cIndex--;
                    if (!Input.Shift)
                    {
                        hIndex = cIndex;
                        hPos = cPos + vPos;
                    }
                }
                else if (((Input.active & SpecialKeys.RIGHT) != 0 || (Input.LeftD && Input.mX < rectangle.X)) && cIndex != text.Length)
                {
                    timer = 0;
                    int diff = (int)GUIHelper.StrSize(text.Substring(0, cIndex + 1)).X - (int)GUIHelper.StrSize(text.Substring(0, cIndex)).X;
                    cPos += diff;
                    cIndex++;
                    if (!Input.Shift)
                    {
                        hIndex = cIndex;
                        hPos = cPos + vPos;
                    }
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
