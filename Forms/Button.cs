using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNAForms.Forms
{
    /// <summary>
    /// Represents a clickable button.
    /// </summary>
    public class Button : Control
    {
        internal static Texture2D buttonTexture;
        /// <summary>
        /// Method that is executed when the button is clicked.
        /// </summary>
        public Action callback;

        /// <summary>
        /// Creates a new button.
        /// </summary>
        /// <param name="position">Position for the new button.</param>
        /// <param name="str">Name for the new button.</param>
        public Button(Position position, string str)
            : base(position, new Size((int)GUIHelper.StrSize(str).X + 8, (int)GUIHelper.StrSize(str).Y + 8))
        {
            text = str;
        }

        internal override void Draw()
        {
            Color tint = realRectangle.IntersectsMouse() ? (Input.LeftD ? new Color(210, 210, 255, alpha) : new Color(230, 230, 255, alpha)) : new Color(255, 255, 255, alpha);
            GUIHelper.sb.Draw(buttonTexture, realRectangle, tint);
            GUIHelper.OutlineRect(rectangle, new Color(0, 0, 0, alpha));
            GUIHelper.DrawStr(text, position + new Position(4, 4), new Color(255, 255, 255, alpha));
        }
        internal override void Update()
        {
            if (rectangle.IntersectsMouse() && Input.LeftR && callback != null)
            {
                callback();
            }
        }
    }
}
