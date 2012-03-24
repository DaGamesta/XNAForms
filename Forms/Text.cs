using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNAForms.Forms
{
    /// <summary>
    /// Represents colored text.
    /// </summary>
    public sealed class Text : Control
    {
        /// <summary>
        /// Color of the text.
        /// </summary>
        public Color color;

        /// <summary>
        /// Creates new text.
        /// </summary>
        /// <param name="position">Position of the new text.</param>
        /// <param name="color">Color of the new text.</param>
        /// <param name="str">String of the new text.</param>
        public Text(Position position, Color color, string str)
            : base(position, new Size(GUIHelper.StrSize(str).X, GUIHelper.StrSize(str).Y))
        {
            this.color = color;
            text = str;
        }

        internal override void Draw()
        {
            GUIHelper.DrawStr(text, position, color);
        }
        internal override void Update()
        {
        }
    }
}
