using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XNAForms.Forms
{
    /// <summary>
    /// Represents a textbox in where text may be typed into it.
    /// </summary>
    public class TextBox : Control
    {
        private int cIndex;
        private int cPos = 4;
        private int hIndex;
        private int hPos = 4;
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
            : base(position, new Size(size, GUIHelper.fontY + 4))
        {
            text = def;
        }
        /// <summary>
        /// Clears the textbox.
        /// </summary>
        public void Clear()
        {
            cIndex = hIndex = vPos = 0;
            cPos = hPos = 4;
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
                active = rectangle.IntersectsMouse();
            }
            if (active)
            {
                if (Input.LeftD && Input.mY >= position.Y && Input.mY <= position.Y + size.height)
                {
                    if (Input.mX < position.X)
                    {
                        cIndex = 0;
                        cPos = 4;
                    }
                    else if (Input.mY > position.X + size.width)
                    {
                        cIndex = text.Length;
                        cPos = (int)GUIHelper.StrSize(text).X - vPos + 4;
                    }
                    else
                    {
                        cIndex = 0;
                        while ((int)GUIHelper.StrSize(text.Substring(0, cIndex)).X - vPos + 4 < Input.mX - position.X - 4 && cIndex < text.Length)
                        {
                            cIndex++;
                        }
                        cPos = (int)GUIHelper.StrSize(text.Substring(0, cIndex)).X - vPos + 4;
                    }
                    if (Input.LeftC)
                    {
                        hIndex = cIndex;
                        hPos = cPos + vPos;
                    }
                }
                timer++;
                timer %= 60;
                string next = Input.nextStr;
                if (next != "")
                {
                    timer = 0;
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
                if (((Input.active & SpecialKeys.LEFT) != 0 ||
                    (Input.LeftD && Input.mX < position.X && Input.mY >= position.Y && Input.mY <= position.Y + size.height)) && cIndex != 0)
                {
                    timer = 0;
                    int diff = (int)GUIHelper.StrSize(text.Substring(0, cIndex)).X - (int)GUIHelper.StrSize(text.Substring(0, cIndex - 1)).X;
                    cPos -= diff;
                    cIndex--;
                    if (!Input.Shift && !(Input.mX < position.X || Input.mX > position.X + size.width))
                    {
                        hIndex = cIndex;
                        hPos = cPos + vPos;
                    }
                }
                else if (((Input.active & SpecialKeys.RIGHT) != 0 ||
                    (Input.LeftD && Input.mX > position.X + size.width && Input.mY >= position.Y && Input.mY <= position.Y + size.height)) && cIndex != text.Length)
                {
                    timer = 0;
                    int diff = (int)GUIHelper.StrSize(text.Substring(0, cIndex + 1)).X - (int)GUIHelper.StrSize(text.Substring(0, cIndex)).X;
                    cPos += diff;
                    cIndex++;
                    if (!Input.Shift && !(Input.mX < position.X || Input.mX > position.X + size.width))
                    {
                        hIndex = cIndex;
                        hPos = cPos + vPos;
                    }
                }
                if ((Input.active & SpecialKeys.C) != 0 && cIndex != hIndex)
                {
                    if (hPos < cPos)
                    {
                        System.Windows.Forms.Clipboard.SetText(text.Substring(hIndex, cIndex - hIndex));
                    }
                    else
                    {
                        System.Windows.Forms.Clipboard.SetText(text.Substring(cIndex, hIndex - cIndex));
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
