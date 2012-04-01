﻿using System;
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
        /// <summary>
        /// Gets the horizontal scrollbar's value.
        /// </summary>
        protected int hValue
        {
            get
            {
                return (int)hScrollbar.value;
            }
        }
        private Scrollbar vScrollbar = new Scrollbar(new Position(0, 0), 0, Orientation.VERTICAL);
        /// <summary>
        /// Gets the vertical scrollbar's value.
        /// </summary>
        protected int vValue
        {
            get
            {
                return (int)vScrollbar.value;
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
                    if (vScrollbar.isNeeded)
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
                if (vScrollbar.isNeeded)
                {
                    return this.size.width - 15;
                }
                return this.size.width;
            };

            vScrollbar.positionFunction = () => this.position + new Position(this.size.width - 15, 0);
            vScrollbar.sizeFunction = () =>
                {
                    if (hScrollbar.isNeeded)
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
                    if (hScrollbar.isNeeded)
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
                Rectangle rect = rectangle;
                if (vScrollbar.isNeeded)
                {
                    rect.Width -= 15;
                }
                if (hScrollbar.isNeeded)
                {
                    rect.Height -= 15;
                }
                GUIHelper.Scissor(rect);
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
