using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNAForms.Forms
{
    /// <summary>
    /// Represents the method(s) that handle various control events.
    /// </summary>
    public delegate void ControlEventHandler(object sender, EventArgs e);
    /// <summary>
    /// Represents a visible object on a form.
    /// </summary>
    public abstract class Control
    {
        private Position fPosition;
        internal int alpha = 255;
        /// <summary>
        /// Docking style of the control.
        /// </summary>
        public DockStyle dockStyle;
        /// <summary>
        /// Position of the control.
        /// </summary>
        public Position position;
        /// <summary>
        /// The function that returns the position of the control.
        /// </summary>
        public Func<Position> positionFunction;
        /// <summary>
        /// Gets the bounding rectangle of the control.
        /// </summary>
        public virtual Rectangle rectangle
        {
            get
            {
                return new Rectangle(position.X, position.Y, size.width, size.height);
            }
        }
        /// <summary>
        /// Size of the control.
        /// </summary>
        public Size size;
        /// <summary>
        /// The function that returns the size of the control.
        /// </summary>
        public Func<Size> sizeFunction;
        /// <summary>
        /// Additional text used by the control.
        /// </summary>
        public string text;

        internal Control(Position position, Size size)
        {
            fPosition = position;
            this.position = position;
            this.size = size;
        }

        internal abstract void Draw();
        internal virtual void Reposition(Panel owner)
        {
            bool left = (dockStyle & DockStyle.LEFT) != 0;
            bool top = (dockStyle & DockStyle.TOP) != 0;
            bool right = (dockStyle & DockStyle.RIGHT) != 0;
            bool bottom = (dockStyle & DockStyle.BOTTOM) != 0;
            bool fill = (dockStyle & DockStyle.FILL) != 0;
            if (sizeFunction == null)
            {
                if (fill)
                {
                    if (top || bottom)
                    {
                        size.width = owner.size.width - 12;
                    }
                    else if (left || right)
                    {
                        size.height = owner.size.height - 12;
                    }
                    else
                    {
                        size.width = owner.size.width - 12;
                        size.height = owner.size.height - 12;
                    }
                }
            }
            else
            {
                size = sizeFunction();
            }
            if (positionFunction == null)
            {
                if (left || top)
                {
                    position = owner.position + new Position(6, 6);
                }
                else if (right)
                {
                    position = owner.position + new Position(owner.size.width - size.width - 6, 6);
                }
                else if (bottom)
                {
                    position = owner.position + new Position(6, owner.size.height - size.height - 6);
                }
                else
                {
                    position = owner.position + fPosition;
                }
            }
            else
            {
                position = positionFunction();
            }
        }
        internal abstract void Update();
    }
}
