using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAForms.Forms
{
    /// <summary>
    /// Represents the method(s) that handle various form events.
    /// </summary>
    public delegate void FormEventHandler(object sender, EventArgs e);
    /// <summary>
    /// Represents a window, which make up the core of the graphical user interface.
    /// </summary>
    public class Form : Panel
    {
        internal static Texture2D tbTex;

        /// <summary>
        /// Whether or not the form can be resized.
        /// </summary>
        protected bool canResize;
        internal int index;
        /// <summary>
        /// Gets if the form is active.
        /// </summary>
        public bool isActive
        {
            get
            {
                return GUI.formOrder[GUI.formOrder.Count - 1] == index;
            }
        }
        /// <summary>
        /// The minimum size of the form.
        /// </summary>
        protected Size minSize;
        private bool moving;
        /// <summary>
        /// Fires when the form is drawn.
        /// </summary>
        protected event FormEventHandler onDraw;
        /// <summary>
        /// Fires when the form is updated.
        /// </summary>
        protected event FormEventHandler onUpdate;
        /// <summary>
        /// Gets the bounding rectangle of the entire form, including the titlebar.
        /// </summary>
        public sealed override Rectangle rectangle
        {
            get
            {
                return new Rectangle(position.X, position.Y - tBarHeight, size.width, size.height + tBarHeight);
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

        internal override void Draw()
        {
            Rectangle titlebar = new Rectangle(position.X, position.Y - tBarHeight, size.width, tBarHeight);
            GUIHelper.sb.Draw(tbTex, titlebar, new Color(255, 255, 255, alpha));
            GUIHelper.OutlineRect(titlebar, new Color(0, 0, 0, alpha));
            GUIHelper.DrawStr(text, position + new Position(6, -tBarHeight + 6), new Color(255, 255, 255, alpha));
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
            if (onDraw != null)
            {
                onDraw.Invoke(this, new EventArgs());
            }
            base.Draw();
        }
        internal override void Update()
        {
            Rectangle titlebar = new Rectangle(position.X, position.Y - tBarHeight, size.width, tBarHeight);
            if (!Input.LeftD)
            {
                moving = false;
                res.dir = Direction.NONE;
                res.mOff = Direction.NONE;
            }
            bool maybeLeftResize = new Rectangle(position.X, position.Y - tBarHeight, 6, size.height + tBarHeight).IntersectsMouse() || (res.dir & Direction.LEFT) != 0;
            bool maybeRightResize = new Rectangle(position.X + size.width - 6, position.Y - tBarHeight, 6, size.height + tBarHeight).IntersectsMouse() || (res.dir & Direction.RIGHT) != 0;
            bool maybeTopResize = new Rectangle(position.X, position.Y - tBarHeight, size.width, 6).IntersectsMouse() || (res.dir & Direction.UP) != 0;
            bool maybeBottomResize = new Rectangle(position.X, position.Y + size.height - 6, size.width, 6).IntersectsMouse() || (res.dir & Direction.DOWN) != 0;
            if (res.dir != Direction.NONE)
            {
                maybeLeftResize = (res.dir & Direction.LEFT) != 0;
                maybeRightResize = (res.dir & Direction.RIGHT) != 0;
                maybeTopResize = (res.dir & Direction.UP) != 0;
                maybeBottomResize = (res.dir & Direction.DOWN) != 0;
            }
            if (moving || !canResize)
            {
                maybeLeftResize = maybeRightResize = maybeTopResize = maybeBottomResize = false;
            }
            if (runClicks)
            {
                if (maybeLeftResize)
                {
                    GUI.SetCursor(CursorType.RESIZE_HORIZONTAL);
                    if (Input.LeftC)
                    {
                        res.pt.X = Input.mX - position.X;
                        res.dir |= Direction.LEFT;
                    }
                }
                if (maybeRightResize)
                {
                    GUI.SetCursor(CursorType.RESIZE_HORIZONTAL);
                    if (Input.LeftC)
                    {
                        res.pt.X = Input.mX - size.width - position.X;
                        res.dir |= Direction.RIGHT;
                    }
                }
                if (maybeTopResize)
                {
                    GUI.SetCursor((maybeRightResize || maybeLeftResize) ? CursorType.RESIZE_DIAGONAL : CursorType.RESIZE_VERTICAL);
                    if (maybeRightResize)
                    {
                        GUI.FlipCursor(true);
                    }
                    if (Input.LeftC)
                    {
                        res.pt.Y = Input.mY - position.Y;
                        res.dir |= Direction.UP;
                    }
                }
                if (maybeBottomResize)
                {
                    GUI.SetCursor((maybeRightResize || maybeLeftResize) ? CursorType.RESIZE_DIAGONAL : CursorType.RESIZE_VERTICAL);
                    if (maybeLeftResize)
                    {
                        GUI.FlipCursor(true);
                    }
                    if (Input.LeftC)
                    {
                        res.pt.Y = Input.mY - size.height - position.Y;
                        res.dir |= Direction.DOWN;
                    }
                }
                if (Input.LeftC)
                {
                    if (titlebar.IntersectsMouse() && res.dir == Direction.NONE)
                    {
                        moving = true;
                    }
                    if (rectangle.IntersectsMouse())
                    {
                        GUI.formOrder.Remove(index);
                        GUI.formOrder.Add(index);
                    }
                }
            }
            if (moving)
            {
                position.X += Input.mDX;
                position.Y += Input.mDY;
            }
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
            if (GUI.contextMenu != null)
            {
                MaskUpdate<ContextMenuArea>();
            }
            else if (isActive)
            {
                if (onUpdate != null)
                {
                    onUpdate.Invoke(this, new EventArgs());
                }
                base.Update();
            }
        }
    }
}
