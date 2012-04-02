using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace XNAForms.Forms
{
    /// <summary>
    /// Represents a text area.
    /// </summary>
    public sealed class TextArea : Panel
    {
        private List<Text> texts = new List<Text>(1000);
        /// <summary>
        /// Creates a new textarea.
        /// </summary>
        /// <param name="position">Position of the new textarea.</param>
        /// <param name="size">Size of the new textarea.</param>
        public TextArea(Position position, Size size)
            : base(position, size)
        {
        }
        /// <summary>
        /// Adds a line of text to the textarea
        /// </summary>
        /// <param name="text">The text to add.</param>
        public void Add(Text text)
        {
            texts.Add(text);
        }
        /// <summary>
        /// Clears the textarea of all text.
        /// </summary>
        public void Clear()
        {
            controls.Clear();
            texts.Clear();
        }
        internal override void Draw()
        {
            base.Draw();
            GUIHelper.OutlineRect(rectangle, new Color(0, 0, 0, 255));
        }
        internal override void Update()
        {
            controls.Clear();
            int y = 0;
            for (int i = 0; i < texts.Count; i++)
            {
                string currLine = "";
                string str = texts[i].text;
                string[] strArr = str.Split(' ');
                foreach (string word in strArr)
                {
                    if (GUIHelper.StrSize(currLine + word).X >= size.width - (vScrollbar.active ? 25 : 6))
                    {
                        controls.Add(new Text(new Position(4, y), texts[i].color, currLine));
                        y += (int)GUIHelper.StrSize(currLine).Y;
                        currLine = "";
                    }
                    currLine += word + " ";
                }
                if (currLine != "")
                {
                    controls.Add(new Text(new Position(4, y), texts[i].color, currLine));
                    y += (int)GUIHelper.StrSize(currLine).Y;
                }
            }
            base.Update();
        }
    }
}
