using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNAForms.Forms
{
    /// <summary>
    /// Groups collections of controls together, and allows for two scrollbars as well.
    /// </summary>
    public class Panel : Control
    {
        /// <summary>
        /// The controls that the panel encapsulates.
        /// </summary>
        protected internal List<Control> controls = new List<Control>();
        private Scrollbar hScrollbar = new Scrollbar(new Position(0, 0), 0, Orientation.HORIZONTAL);
        private Scrollbar vScrollbar = new Scrollbar(new Position(0, 0), 0, Orientation.VERTICAL);

        /// <summary>
        /// Creates a new panel.
        /// </summary>
        /// <param name="position">Position for the new panel.</param>
        /// <param name="size">Size for the new panel.</param>
        /// <param name="controls">Controls for the new panel to hold.</param>
        public Panel(Position position, Size size, params Control[] controls)
            : base(position, size)
        {
            foreach (Control c in controls)
            {
                this.controls.Add(c);
            }
            hScrollbar.positionFunction = () => this.position + new Position(0, this.size.height - 15);
            hScrollbar.sizeFunction = () => new Size(this.size.width, 15);
            hScrollbar.totalFunction = () =>
                {
                    int maxX = 0;
                    foreach (Control c in this.controls)
                    {
                        if (c.position.X + c.size.width > maxX)
                        {
                            maxX = c.position.X + c.size.width;
                        }
                    }
                    return maxX;
                };
            hScrollbar.viewableFunction = () => this.size.width;

            vScrollbar.positionFunction = () => this.position + new Position(this.size.width - 15, 0);
            vScrollbar.sizeFunction = () => new Size(15, this.size.height);
            vScrollbar.totalFunction = () =>
            {
                int maxY = 0;
                foreach (Control c in this.controls)
                {
                    if (c.position.Y + c.size.height > maxY)
                    {
                        maxY = c.position.Y + c.size.height;
                    }
                }
                return maxY;
            };
            vScrollbar.viewableFunction = () => this.size.height;
        }
        /// <summary>
        /// Adds a control.
        /// </summary>
        /// <param name="c">Control to add.</param>
        protected void Add(Control c)
        {
            if (!(c is Form))
            {
                controls.Add(c);
            }
        }
        internal override void Draw()
        {
            if (GUIHelper.sb.GraphicsDevice.Viewport.Bounds.Intersects(rectangle))
            {
                GUIHelper.Scissor(rectangle);
                Position offset = GUIHelper.offset;
                if (!(this is Form))
                {
                    int x = 0, y = 0;
                    if (hScrollbar.isNeeded)
                    {
                        x = (int)hScrollbar.value;
                    }
                    if (vScrollbar.isNeeded)
                    {
                        y = (int)vScrollbar.value;
                    }
                    GUIHelper.offset = position + offset - new Position(x, y);
                }
                else
                {
                    GUIHelper.offset = position;
                }
                foreach (Control c in controls)
                {
                    Rectangle r = c.rectangle;
                    r.Offset(GUIHelper.offset.X, GUIHelper.offset.Y);
                    if (GUIHelper.sb.GraphicsDevice.Viewport.Bounds.Intersects(r))
                    {
                        c.Draw();
                    }
                }
                GUIHelper.Unscissor();
                GUIHelper.offset = new Position(0, 0);
                if (!(this is Form))
                {
                    hScrollbar.Draw();
                    vScrollbar.Draw();
                }
                GUIHelper.offset = offset;
            }
        }
        internal void MaskUpdate<T>()
        {
            foreach (Control c in controls)
            {
                if (c is T)
                {
                    c.Update();
                }
            }
        }
        internal override void Update()
        {
            hScrollbar.Reposition(this);
            hScrollbar.Update();
            vScrollbar.Reposition(this);
            vScrollbar.Update();
            foreach (Control c in controls)
            {
                c.Reposition(this);
                c.position += this.position;
                c.Update();
                c.position -= this.position;
            }
        }
    }
}
