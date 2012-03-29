using System;
using System.Collections.Generic;

namespace XNAForms.Forms
{
    /// <summary>
    /// Represents a text area.
    /// </summary>
    public sealed class TextArea : Panel
    {
        private Scrollbar scrollbar
        {
            get
            {
                return scrollbars[0];
            }
        }
        private List<Text> texts = new List<Text>(1000);
        /// <summary>
        /// Creates a new textarea.
        /// </summary>
        /// <param name="position">Position of the new textarea.</param>
        /// <param name="size">Size of the new textarea.</param>
        public TextArea(Position position, Size size)
            : base(position, size)
        {
            AddScrollbar(Placement.RIGHT, () => texts.Count, () =>
                {
                    int y = 0;
                    for (int i = 0; i < texts.Count; i++)
                    {
                        y += (int)GUIHelper.StrSize(texts[i].text).Y;
                        if (y > this.size.height - 1)
                        {
                            return i;
                        }
                    }
                    return scrollbar.total + 1;
                });
        }
        /// <summary>
        /// Adds text to the textarea.
        /// </summary>
        /// <param name="text">The text to add.</param>
        public void AddText(Text text)
        {
            controls.Add(text);
        }
        internal override void Draw()
        {
            scrollbar.Draw();
            int value = (int)scrollbar.value;
            int y = 1;
            for (int i = value; i < value + scrollbar.viewable; i++)
            {
                if (i >= 0 && i < texts.Count)
                {
                    GUIHelper.DrawStr(texts[i].text, position + new Position(4, y), texts[i].color);
                    y += (int)GUIHelper.StrSize(texts[i].text).Y;
                }
            }
        }
        internal override void Update()
        {
            texts.Clear();
            int maxChars = (size.width - 15) / 6;
            for (int i = 0; i < controls.Count; i++)
            {
                string currLine = "";
                string str = controls[i].text;
                string[] strArr = str.Split(' ');
                foreach (string word in strArr)
                {
                    if (GUIHelper.StrSize(currLine + word).X >= size.width - 25)
                    {
                        texts.Add(new Text(position, ((Text)controls[i]).color, currLine));
                        currLine = "";
                    }
                    currLine += word + " ";
                }
                if (currLine != "")
                {
                    texts.Add(new Text(position, ((Text)controls[i]).color, currLine));
                }
            }
            if (rectangle.IntersectsMouse() && Input.mDS != 0)
            {
                scrollbar.scrollbarPosition += (int)((float)(3 / (float)scrollbar.total) * (float)scrollbar.size) * Math.Sign(Input.mDS);	
            }
            scrollbar.Reposition(this);
            scrollbar.Update();
            base.Update();
        }
    }
}
