using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAForms.Forms
{
    /// <summary>
    /// Represents an option of a context menu.
    /// </summary>
    public class ContextMenuOption
    {
        internal static Texture2D SelectedTexture;
        /// <summary>
        /// Represents the method that handles a context menu option.
        /// </summary>
        public delegate void ClickHandler(object sender, EventArgs e);
        internal int alpha;
        /// <summary>
        /// Fires when the context menu option is clicked.
        /// </summary>
        public ClickHandler onClick;
        internal ContextMenu owner;
        internal string text;
        internal int yOff;
        internal int width;
        /// <summary>
        /// Creates a new context menu option.
        /// </summary>
        /// <param name="str">Text for the new context menu option.</param>
        public ContextMenuOption(string str)
        {
            text = str;
        }

        internal virtual void Draw()
        {
            int height = (int)GUIHelper.StrSize(text).Y + 8;
            Rectangle option = new Rectangle(owner.position.X, owner.position.Y + yOff, height + width, height);
            GUIHelper.FillRect(option, new Color(29, 29, 29, alpha));
            GUIHelper.OutlineRect(option, new Color(0, 0, 0, alpha));
            if (option.IntersectsMouse())
            {
                option.Inflate(-3, -3);
                GUIHelper.sb.Draw(SelectedTexture, option, new Color(255, 255, 255, alpha));
                GUIHelper.OutlineRect(option, new Color(0, 0, 0, alpha));
            }
            GUIHelper.DrawStr(text, owner.position + new Position(4 + height, 4 + yOff), new Color(255, 255, 255, alpha));
            GUIHelper.DrawLn(owner.position + new Position(height, yOff), owner.position + new Position(height, height + yOff), new Color(0, 0, 0, alpha));
        }
        internal virtual bool Update()
        {
            int height = (int)GUIHelper.StrSize(text).Y + 8;
            Rectangle option = new Rectangle(owner.position.X, owner.position.Y + yOff, height + width, height);
            if (option.IntersectsMouse() && Input.LeftR)
            {
                if (onClick != null)
                {
                    onClick.Invoke(this, new EventArgs());
                }
                return true;
            }
            return false;
        }
    }
}
