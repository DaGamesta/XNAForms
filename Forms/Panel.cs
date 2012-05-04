using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNAForms.Forms
{
    /// <summary>
    /// Groups collections of controls together, allowing for two scrollbars as well.
    /// </summary>
    public class Panel : Control
    {
        /// <summary>
        /// The controls that the panel encapsulates.
        /// </summary>
        protected internal List<Control> controls = new List<Control>();
        internal Scrollbar hScrollbar = new Scrollbar(new Position(0, 0), 0, Orientation.HORIZONTAL);
        /// <summary>
        /// Gets the horizontal scrollbar's value.
        /// </summary>
        protected int hValue
        {
            get
            {
                return hScrollbar.active ? (int)hScrollbar.value : 0;
            }
        }
        /// <summary>
        /// Scrollbar step size.
        /// </summary>
        protected int stepSize = 10;
        internal Scrollbar vScrollbar = new Scrollbar(new Position(0, 0), 0, Orientation.VERTICAL);
        /// <summary>
        /// Gets the vertical scrollbar's value.
        /// </summary>
        protected int vValue
        {
            get
            {
                return vScrollbar.active ? (int)vScrollbar.value : 0;
            }
        }

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
            hScrollbar.sizeFunction = () =>
                {
                    if (vScrollbar.active)
                    {
                        return new Size(this.size.width - 16, 15);
                    }
                    return new Size(this.size.width, 15);
                };
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
            hScrollbar.viewableFunction = () =>
            {
                if (vScrollbar.active)
                {
                    return this.size.width - 15;
                }
                return this.size.width;
            };

            vScrollbar.positionFunction = () => this.position + new Position(this.size.width - 15, 0);
            vScrollbar.sizeFunction = () =>
                {
                    if (hScrollbar.active)
                    {
                        return new Size(15, this.size.height - 16);
                    }
                    return new Size(15, this.size.height);
                };
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
            vScrollbar.viewableFunction = () =>
                {
                    if (hScrollbar.active)
                    {
                        return this.size.height - 15;
                    }
                    return this.size.height;
                };
        }
        /// <summary>
        /// Adds a control.
        /// </summary>
        /// <param name="c">Control to add.</param>
        public void Add(Control c)
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
                Position offset = GUIHelper.offset;
                if (!(this is Form))
                {
                    Rectangle rect = rectangle;
                    if (vScrollbar.active)
                    {
                        rect.Width -= 15;
                    }
                    if (hScrollbar.active)
                    {
                        rect.Height -= 15;
                    }
                    GUIHelper.Scissor(rect);
                    GUIHelper.offset = position + offset - new Position(hValue, vValue);
                }
                else
                {
                    GUIHelper.offset = position;
                }
                foreach (Control c in controls)
                {
                    Rectangle r = c.rectangle;
                    r.Offset(GUIHelper.offset.X, GUIHelper.offset.Y);
                    if (GUIHelper.sb.GraphicsDevice.Viewport.Bounds.Intersects(r) && (GUIHelper.scissor.Intersects(r) || this is Form))
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
            if (!(this is Form) && rectangle.IntersectsMouse() && vScrollbar.total != 0)
            {
                vScrollbar.scrollbarPosition += (int)Math.Round(((float)stepSize / (float)vScrollbar.total) * vScrollbar.size * Math.Sign(Input.mDS));
            }
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
