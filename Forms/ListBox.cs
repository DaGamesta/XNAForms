using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNAForms.Forms
{
    /// <summary>
    /// Represents a list of selectable items.
    /// </summary>
    public sealed class ListBox : Panel
    {
        private List<object> items = new List<object>();
        private List<bool> selected = new List<bool>();
        private int y;

        /// <summary>
        /// Creates a new list box.
        /// </summary>
        /// <param name="position">Position for the new list box.</param>
        /// <param name="size">Size for the new list box.</param>
        public ListBox(Position position, Size size)
            : base(position, size)
        {
        }

        /// <summary>
        /// Adds an object to the list of selectable items.
        /// </summary>
        public void Add(object obj)
        {
            controls.Add(new Text(new Position(4, 3 + y), new Color(255, 255, 255, alpha), obj.ToString()));
            y += (int)GUIHelper.StrSize(obj.ToString()).Y + 2;
            items.Add(obj);
            selected.Add(false);
        }

        internal override void Draw()
        {
            GUIHelper.OutlineRect(rectangle, new Color(0, 0, 0, alpha));
            GUIHelper.FillRect(rectangle, new Color(30, 30, 30, alpha));
            GUIHelper.Scissor(rectangle);
            for (int i = 0; i < items.Count; i++)
            {
                if (selected[i])
                {
                    GUIHelper.sb.Draw(ContextMenuOption.SelectedTexture,
                        new Rectangle(realRectangle.X + controls[i].position.X - 4, realRectangle.Y + controls[i].position.Y - vValue - 2,
                            size.width - (vScrollbar.active ? 15 : 0), controls[i].size.height + 2), new Color(255, 255, 255, alpha));
                }
            }
            GUIHelper.Unscissor();
            base.Draw();
        }
        internal override void Update()
        {
            Rectangle rectangle;
            for (int i = 0; i < items.Count; i++)
            {
                rectangle = new Rectangle(position.X + controls[i].position.X - 4,
                    position.Y + controls[i].position.Y - vValue - 2, size.width - (vScrollbar.active ? 15 : 0), controls[i].size.height + 2);
                if (rectangle.IntersectsMouse() && base.rectangle.IntersectsMouse() && Input.LeftC)
                {
                    if (selected.Exists(b => b) && !Input.Control)
                    {
                        for (int j = 0; j < selected.Count; j++)
                        {
                            selected[j] = false;
                        }
                    }
                    selected[i] = true;
                    break;
                }
            }
            base.Update();
        }
    }
}
