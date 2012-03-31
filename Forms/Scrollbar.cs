using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAForms.Forms
{
    /// <summary>
    /// Represents a movable scrollbar.
    /// </summary>
    internal sealed class Scrollbar : Control
    {
        internal static Texture2D HScrollbarTexture;
        internal static Texture2D VScrollbarTexture;

        private MoveInfo movInfo;
        internal bool isNeeded
        {
            get
            {
                return viewable < total;
            }
        }
        internal bool isVertical
        {
            get
            {
                return orientation == Orientation.VERTICAL;
            }
        }
        private Orientation orientation;
        internal int scrollbarPosition;
        internal new int size
        {
            get
            {
                return isVertical ? base.size.height : base.size.width;
            }
        }
        internal int sSize;
        internal int total
        {
            get
            {
                return totalFunction();
            }
        }
        internal Func<int> totalFunction;
        internal float value
        {
            get
            {
                if (!isNeeded)
                {
                    return 0;
                }
                return ((float)(size - sSize - scrollbarPosition) / (float)(size - sSize)) * (float)(total - viewable);
            }
        }
        internal int viewable
        {
            get
            {
                return viewableFunction();
            }
        }
        internal Func<int> viewableFunction;

        internal Scrollbar(Position position, int size, Orientation orientation = Orientation.VERTICAL)
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
                GUIHelper.FillRect(backing, new Color(20, 20, 20, alpha));
                GUIHelper.OutlineRect(backing, new Color(0, 0, 0, alpha));
                GUIHelper.sb.Draw(isVertical ? VScrollbarTexture : HScrollbarTexture, new Rectangle(scrollbar.X + GUIHelper.offset.X, scrollbar.Y + GUIHelper.offset.Y, scrollbar.Width, scrollbar.Height), tint);
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
