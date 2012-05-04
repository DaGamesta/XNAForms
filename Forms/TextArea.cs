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
        /// <summary>
        /// Creates a new textarea.
        /// </summary>
        /// <param name="position">Position of the new textarea.</param>
        /// <param name="size">Size of the new textarea.</param>
        public TextArea(Position position, Size size)
            : base(position, size)
        {
            stepSize = GUIHelper.fontY * 3;
        }
        /// <summary>
        /// Adds a line of text to the textarea
        /// </summary>
        /// <param name="text">The text to add.</param>
        public void Add(Text text)
        {
            foreach (string s in text.text.Split('\n'))
            {
                controls.Add(new Text(new Position(4, controls.Count * GUIHelper.fontY), text.color, s));
            }
        }
        /// <summary>
        /// Clears the textarea of all text.
        /// </summary>
        public void Clear()
        {
            controls.Clear();
        }
        internal override void Draw()
        {
            base.Draw();
            GUIHelper.OutlineRect(rectangle, new Color(0, 0, 0, alpha));
        }
    }
}
