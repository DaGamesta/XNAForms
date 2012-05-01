using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAForms.Forms
{
    /// <summary>
    /// Represents a form, or window, which makes up the core of the graphical user interface.
    /// </summary>
    public class Form : Panel
    {
        internal static Texture2D tbTex;

        /// <summary>
        /// Whether or not the form can be closed.
        /// </summary>
        protected bool canClose;
        /// <summary>
        /// Whether or not the form can be resized.
        /// </summary>
        protected bool canResize;
        internal int index;
        /// <summary>
        /// Gets if the form is active.
        /// </summary>
        public virtual bool isActive
        {
            get
            {
                return GUI.formOrder[GUI.formOrder.Count - 1] == index;
            }
        }
        private bool minimized;
        /// <summary>
        /// The minimum size of the form.
        /// </summary>
        protected Size minSize;
        private bool moving;
        /// <summary>
        /// Fires when the form is updated.
        /// </summary>
        protected event Action onUpdate;
        /// <summary>
        /// Gets the bounding rectangle of the entire form, including the titlebar.
        /// </summary>
        public sealed override Rectangle rectangle
        {
            get
            {
                return new Rectangle(position.X, position.Y - tBarHeight, size.width, tBarHeight + (minimized ? 0 : size.height));
            }
        }
        private ResizeInfo res;
        internal bool runClicks
        {
            get
            {
                if (GUI.contextMenu != null)
                {
                    return false;
                }
                foreach (Form f in GUI.forms)
                {
                    if (f != this)
                    {
                        if (rectangle.IntersectsMouse() && f.rectangle.IntersectsMouse() && GUI.formOrder.IndexOf(index) < GUI.formOrder.IndexOf(f.index))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        private int tBarHeight
        {
            get
            {
                return (int)GUIHelper.font.MeasureString(text).Y + 12;
            }
        }

        /// <summary>
        /// Creates a new form.
        /// </summary>
        /// <param name="position">Position for the new form.</param>
        /// <param name="size">Size for the new form.</param>
        /// <param name="title">Titlebar text for the new form.</param>
        public Form(Position position, Size size, string title)
            : base(position, size)
        {
            text = title;
            this.position.Y += tBarHeight;
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        public virtual void Close()
        {
            GUI.forms.RemoveAt(index);
        }
        internal override void Draw()
        {
            alpha += minimized ? -20 : 20;
            alpha = alpha.Clamp(150, 255);

            Rectangle titlebar = new Rectangle(position.X, position.Y - tBarHeight, size.width, tBarHeight);
            GUIHelper.sb.Draw(tbTex, titlebar, new Color(255, 255, 255, alpha));
            GUIHelper.OutlineRect(titlebar, new Color(0, 0, 0, alpha));
            GUIHelper.DrawStr(text, position + new Position(6, -tBarHeight + 6), new Color(255, 255, 255, alpha));

            Rectangle minimize = new Rectangle(position.X + size.width - tBarHeight + 6, position.Y - tBarHeight + 6, tBarHeight - 12, tBarHeight - 12);
            Color minTint = minimize.IntersectsMouse() ? (Input.LeftD ? new Color(120, 120, 120, alpha)
                : new Color(135, 135, 135, alpha)) : new Color(150, 150, 150, alpha);
            GUIHelper.sb.Draw(ContextMenuOption.SelectedTexture, minimize, minTint);
            GUIHelper.OutlineRect(minimize, new Color(0, 0, 0, alpha));
            Rectangle minShape = new Rectangle(minimize.X + 3, minimize.Bottom - 5, minimize.Width - 6, 2);
            GUIHelper.OutlineRect(minShape, new Color(0, 0, 0, alpha));
            GUIHelper.FillRect(minShape, new Color(60, 60, 60, alpha));
            if (!minimized)
            {
                Rectangle window = new Rectangle(position.X, position.Y, size.width, size.height);
                GUIHelper.FillRect(window, new Color(16, 16, 16, alpha));
                GUIHelper.OutlineRect(window, new Color(0, 0, 0, alpha));
                window.Inflate(-6, -6);
                GUIHelper.FillRect(window, new Color(23, 23, 23, alpha));
                GUIHelper.OutlineRect(window, new Color(0, 0, 0, alpha));
                foreach (Control c in controls)
                {
                    c.alpha = alpha;
                }
                base.Draw();
            }
        }
        internal override void Update()
        {
            Rectangle minRect = new Rectangle(rectangle.Right - tBarHeight + 6, position.Y - tBarHeight + 6, tBarHeight - 12, tBarHeight - 12);
            Rectangle titleRect = new Rectangle(position.X, position.Y - tBarHeight, size.width, tBarHeight);
            if (!Input.LeftD)
            {
                moving = false;
                res = new ResizeInfo();
            }

            bool lResize = new Rectangle(rectangle.X, rectangle.Y, 6, tBarHeight + (minimized ? 0 : size.height)).IntersectsMouse();
            bool rResize = new Rectangle(rectangle.Right - 6, rectangle.Y, 6, tBarHeight + (minimized ? 0 : size.height)).IntersectsMouse();
            bool tResize = new Rectangle(rectangle.X, rectangle.Y, size.width, 6).IntersectsMouse();
            bool bResize = new Rectangle(rectangle.X, rectangle.Bottom - 6, size.width, 6).IntersectsMouse();
            if (res.dir != Direction.NONE)
            {
                lResize = (res.dir & Direction.LEFT) != 0;
                rResize = (res.dir & Direction.RIGHT) != 0;
                tResize = (res.dir & Direction.UP) != 0;
                bResize = (res.dir & Direction.DOWN) != 0;
            }
            if (minimized)
            {
                tResize = bResize = false;
            }
            if (moving || !canResize)
            {
                lResize = rResize = tResize = bResize = false;
            }

            if (runClicks)
            {
                if (lResize || rResize)
                {
                    GUI.SetCursor(CursorType.RESIZE_HORIZONTAL);
                    if (Input.LeftC)
                    {
                        res.pt.X = Input.mX - position.X - (rResize ? size.width : 0);
                        res.dir |= rResize ? Direction.RIGHT : Direction.LEFT;
                    }
                }
                if (tResize || bResize)
                {
                    GUI.SetCursor((lResize || rResize) ? CursorType.RESIZE_DIAGONAL : CursorType.RESIZE_VERTICAL);
                    if ((rResize && tResize) || (bResize && lResize))
                    {
                        GUI.FlipCursor(true);
                    }
                    if (Input.LeftC)
                    {
                        res.pt.Y = Input.mY - position.Y - (bResize ? size.height : 0);
                        res.dir |= bResize ? Direction.DOWN : Direction.UP;
                    }
                }
                if (minRect.IntersectsMouse())
                {
                    if (Input.LeftR)
                    {
                        minimized = !minimized;
                    }
                }
                else if (titleRect.IntersectsMouse() && res.dir == Direction.NONE && Input.LeftC)
                {
                    moving = true;
                }
                if (rectangle.IntersectsMouse() && Input.LeftC)
                {
                    GUI.formOrder.Remove(index);
                    GUI.formOrder.Add(index);
                }
            }

            if (moving)
            {
                position += new Position(Input.mDX, Input.mDY);
            }
            #region Resizing
            if ((res.dir & Direction.LEFT) != 0)
            {
                if (size.width - Input.mDX < minSize.width)
                {
                    res.mOff |= Direction.LEFT;
                    position.X += size.width - minSize.width;
                    size.width = minSize.width;
                }
                if ((res.mOff & Direction.LEFT) == 0)
                {
                    size.width -= Input.mDX;
                    position.X += Input.mDX;
                }
                if (Input.mX - position.X < res.pt.X && (res.mOff & Direction.LEFT) != 0)
                {
                    res.mOff &= ~Direction.LEFT;
                    int disp = res.pt.X - Input.mX + position.X;
                    position.X -= disp;
                    size.width += disp;
                }
            }
            if ((res.dir & Direction.RIGHT) != 0)
            {
                if (size.width + Input.mDX < minSize.width)
                {
                    res.mOff |= Direction.RIGHT;
                    size.width = minSize.width;
                }
                if ((res.mOff & Direction.RIGHT) == 0)
                {
                    size.width += Input.mDX;
                }
                if (Input.mX - size.width - position.X > res.pt.X && (res.mOff & Direction.RIGHT) != 0)
                {
                    res.mOff &= ~Direction.RIGHT;
                    size.width = Input.mX - position.X - res.pt.X;
                }
            }
            if ((res.dir & Direction.UP) != 0)
            {
                if (size.height - Input.mDY < minSize.height)
                {
                    res.mOff |= Direction.UP;
                    position.Y += size.height - minSize.height;
                    size.height = minSize.height;
                }
                if ((res.mOff & Direction.UP) == 0)
                {
                    size.height -= Input.mDY;
                    position.Y += Input.mDY;
                }
                if (Input.mY - position.Y < res.pt.Y && (res.mOff & Direction.UP) != 0)
                {
                    res.mOff &= ~Direction.UP;
                    int disp = res.pt.Y - Input.mY + position.Y;
                    position.Y -= disp;
                    size.height += disp;
                }
            }
            if ((res.dir & Direction.DOWN) != 0)
            {
                if (size.height + Input.mDY < minSize.height)
                {
                    res.mOff |= Direction.DOWN;
                    size.height = minSize.height;
                }
                if ((res.mOff & Direction.DOWN) == 0)
                {
                    size.height += Input.mDY;
                }
                if (Input.mY - size.height - position.Y > res.pt.Y && (res.mOff & Direction.DOWN) != 0)
                {
                    res.mOff &= ~Direction.DOWN;
                    size.height = Input.mY - position.Y - res.pt.Y;
                }
            }
            #endregion
            if (!minimized)
            {
                if (GUI.contextMenu != null)
                {
                    MaskUpdate<ContextMenuArea>();
                }
                else if (isActive)
                {
                    if (onUpdate != null)
                    {
                        onUpdate.Invoke();
                    }
                    base.Update();
                }
                else
                {
                    foreach (Control c in controls)
                    {
                        c.Reposition(this);
                    }
                }
            }
        }
    }
}
