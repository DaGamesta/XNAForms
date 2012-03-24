using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAForms.Forms
{
    /// <summary>
    /// Represents a movable scrollbar.
    /// </summary>
    public sealed class Scrollbar : Control
    {
        internal static Texture2D HScrollbarTexture;
        internal static Texture2D VScrollbarTexture;

        private MoveInfo movInfo;
        private Orientation orientation;
        private int sSize;
        /// <summary>
        /// Gets if the Scrollbar is needed.
        /// </summary>
        public bool isNeeded
        {
            get
            {
                return viewable < total;
            }
        }
        /// <summary>
        /// Gets if the Scrollbar is vertical.
        /// </summary>
        public bool isVertical
        {
            get
            {
                return orientation == Orientation.VERTICAL;
            }
        }
        /// <summary>
        /// The Scrollbar position (that is, the position of the movable part).
        /// </summary>
        public int scrollbarPosition;
        /// <summary>
        /// Gets the size of the Scrollbar (that is, the movable part).
        /// </summary>
        public int scrollbarSize
        {
            get
            {
                return sSize;
            }
        }
        /// <summary>
        /// Gets the length/width of the Scrollbar.
        /// </summary>
        public new int size
        {
            get
            {
                return isVertical ? base.size.height : base.size.width;
            }
        }
        /// <summary>
        /// Gets the total number of items.
        /// </summary>
        public int total
        {
            get
            {
                return totalFunction();
            }
        }
        /// <summary>
        /// The function that gets the amount of total items.
        /// </summary>
        public Func<int> totalFunction;
        /// <summary>
        /// Gets the value of the scrollbar.
        /// </summary>
        public float value
        {
            get
            {
                if (!isNeeded)
                {
                    return 0;
                }
                return ((float)(size - scrollbarSize - scrollbarPosition) / (float)(size - scrollbarSize)) * (float)(total - viewable);
            }
        }
        /// <summary>
        /// Gets the viewable amount of items.
        /// </summary>
        public int viewable
        {
            get
            {
                return viewableFunction();
            }
        }
        /// <summary>
        /// The function that gets the amount of viewable items.
        /// </summary>
        public Func<int> viewableFunction;
        /// <summary>
        /// Creates a new Scrollbar.
        /// </summary>
        /// <param name="position">Position of the Scrollbar.</param>
        /// <param name="size">Length/width of the Scrollbar.</param>
        /// <param name="orientation">Optionally, the orientation of the Scrollbar.</param>
        public Scrollbar(Position position, int size, Orientation orientation = Orientation.VERTICAL)
            : base(position, orientation == Orientation.VERTICAL ? new Size(15, size) : new Size(size, 15))
        {
            this.orientation = orientation;
        }

        internal override void Draw()
        {
            if (isNeeded)
            {
                int dist = size - sSize - scrollbarPosition;
                Rectangle scrollbar = isVertical ? new Rectangle(position.X, position.Y + dist, 15, sSize) : new Rectangle(position.X + dist, position.Y, sSize, 15);
                Color tint = (new Rectangle(scrollbar.X + position.X, scrollbar.Y + position.Y, scrollbar.Width, scrollbar.Height).IntersectsMouse() || movInfo.dir != Orientation.NONE) ?
                    (Input.LeftD ? new Color(210, 210, 255, alpha) : new Color(230, 230, 255, alpha)) : new Color(255, 255, 255, alpha);
                Rectangle backing = new Rectangle(position.X, position.Y, base.size.width, base.size.height);
                GUIHelper.FillRect(backing, new Color(26, 26, 26, alpha));
                GUIHelper.OutlineRect(backing, new Color(0, 0, 0, alpha));
                GUIHelper.sb.Draw(isVertical ? VScrollbarTexture : HScrollbarTexture, scrollbar, tint);
                GUIHelper.OutlineRect(scrollbar, new Color(0, 0, 0, alpha));
            }
        }
        internal override void Update()
        {
            sSize = (int)Math.Round((float)viewable / total * size);
            if (!Input.LeftD)
            {
                movInfo.dir = Orientation.NONE;
                movInfo.mOff = Direction.NONE;
            }
            int dist = size - sSize - scrollbarPosition;
            Rectangle scrollbar = isVertical ? new Rectangle(position.X, position.Y + dist, 15, sSize) : new Rectangle(position.X + dist, position.Y, sSize, 15);
            if (scrollbar.IntersectsMouse() && Input.LeftC)
            {
                movInfo.dir = orientation;
                movInfo.pt = new Position(Input.mX - scrollbar.X, Input.mY - scrollbar.Y);
            }
            if (movInfo.dir != Orientation.NONE)
            {
                if (isVertical)
                {
                    if (scrollbarPosition - Input.mDY > size - sSize)
                    {
                        movInfo.mOff |= Direction.UP;
                        scrollbarPosition = size - sSize;
                    }
                    if (scrollbarPosition - Input.mDY < 0)
                    {
                        movInfo.mOff |= Direction.DOWN;
                        scrollbarPosition = 0;
                    }
                    if ((movInfo.mOff & (Direction.UP | Direction.DOWN)) == 0)
                    {
                        scrollbarPosition -= Input.mDY;
                    }
                    if (Input.mY > movInfo.pt.Y + scrollbar.Y && (movInfo.mOff & Direction.UP) != 0)
                    {
                        movInfo.mOff &= ~Direction.UP;
                    }
                    if (Input.mY < movInfo.pt.Y + scrollbar.Y && (movInfo.mOff & Direction.DOWN) != 0)
                    {
                        movInfo.mOff &= ~Direction.DOWN;
                    }
                }
                else
                {
                    if (scrollbarPosition - Input.mDX > size - sSize)
                    {
                        movInfo.mOff |= Direction.LEFT;
                        scrollbarPosition = size - sSize;
                    }
                    if (scrollbarPosition - Input.mDX < 0)
                    {
                        movInfo.mOff |= Direction.RIGHT;
                        scrollbarPosition = 0;
                    }
                    if ((movInfo.mOff & (Direction.LEFT | Direction.RIGHT)) == 0)
                    {
                        scrollbarPosition -= Input.mDX;
                    }
                    if (Input.mX > movInfo.pt.X + scrollbar.X && (movInfo.mOff & Direction.LEFT) != 0)
                    {
                        movInfo.mOff &= ~Direction.LEFT;
                    }
                    if (Input.mX < movInfo.pt.X + scrollbar.X && (movInfo.mOff & Direction.RIGHT) != 0)
                    {
                        movInfo.mOff &= ~Direction.RIGHT;
                    }
                }
            }
            scrollbarPosition = scrollbarPosition.Clamp(0, size - sSize);
        }
    }
}
