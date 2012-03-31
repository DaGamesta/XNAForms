using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNAForms.Forms
{
    /// <summary>
    /// Represents a right-click context menu.
    /// </summary>
    public sealed class ContextMenu
    {
        private int alpha;
        internal Position position;
        private List<ContextMenuOption> options = new List<ContextMenuOption>();
        private Size size;

        internal ContextMenu(ContextMenuArea cma)
        {
            options = cma.options;
            position = new Position(Input.mX, Input.mY);
            int mX = 0;
            int y = 0;
            foreach (ContextMenuOption option in options)
            {
                option.owner = this;
                option.yOff = y;
                Vector2 dim = GUIHelper.StrSize(option.text);
                if (dim.X > mX)
                {
                    mX = (int)dim.X;
                }
                y += (int)dim.Y + 8;
            }
            size = new Size(mX > 200 ? mX : 200, y);
            foreach (ContextMenuOption option in options)
            {
                option.width = mX > 200 ? mX : 200;
            }
            Rectangle menu = new Rectangle(position.X, position.Y, size.width, size.height);
            if (menu.Bottom > GUIHelper.sb.GraphicsDevice.Viewport.Height)
            {
                position.Y = Input.mY - size.height;
            }
        }

        internal void Draw()
        {
            alpha += 25;
            alpha = alpha > 255 ? 255 : alpha;
            foreach (ContextMenuOption option in options)
            {
                option.alpha = alpha;
                option.Draw();
            }
        }
        internal void Update()
        {
            foreach (ContextMenuOption option in options)
            {
                if (option.Update())
                {
                    GUI.contextMenu = null;
                    return;
                }
            }
            if ((Input.LeftC || Input.RightC) && !new Rectangle(position.X, position.Y, size.width, size.height).IntersectsMouse())
            {
                GUI.contextMenu = null;
            }
        }
    }
}
