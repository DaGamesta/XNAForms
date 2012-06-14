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
        public bool isVertical
        {
            get
            {
                return orientation == Orientation.VERTICAL;
            }
        }
        private Orientation orientation;
        public int scrollbarPosition;
        public new int size
        {
            get
            {
                return isVertical ? base.size.height : base.size.width;
            }
        }
        public int sSize;
        private int realPos
        {
            get
            {
                if (isVertical)
                {
                    return size - sSize - scrollbarPosition;
                }
                else
                {
                    return scrollbarPosition;
                }
            }
        }
        public int total
        {
            get
            {
                return totalFunction();
            }
        }
        public Func<int> totalFunction;
        public float value
        {
            get
            {
                return active ? ((float)realPos / (float)(size - sSize)) * (float)(total - viewable) : 0;
            }
        }
        public int viewable
        {
            get
            {
                return viewableFunction();
            }
        }
        public Func<int> viewableFunction;

        internal Scrollbar(Position position, int size, Orientation orientation = Orientation.VERTICAL)
            : base(position, orientation == Orientation.VERTICAL ? new Size(15, size) : new Size(size, 15))
        {
            this.orientation = orientation;
        }
        internal override void Draw()
        {
            if (active)
            {
                Rectangle scrollbar = isVertical ? new Rectangle(position.X, position.Y + realPos, 15, sSize) : new Rectangle(position.X + realPos, position.Y, sSize, 15);
                Color tint = (scrollbar.IntersectsMouse() || movInfo.dir != Orientation.NONE) ?
                    (Input.LeftD ? new Color(210, 210, 255, alpha) : new Color(230, 230, 255, alpha)) : new Color(255, 255, 255, alpha);
                Rectangle backing = new Rectangle(position.X, position.Y, base.size.width, base.size.height);
                GUIHelper.FillRect(backing, new Color(20, 20, 20, alpha));
                GUIHelper.OutlineRect(backing, new Color(0, 0, 0, alpha));
                GUIHelper.sb.Draw(isVertical ? VScrollbarTexture : HScrollbarTexture,
                    new Rectangle(scrollbar.X + GUIHelper.offset.X, scrollbar.Y + GUIHelper.offset.Y, scrollbar.Width, scrollbar.Height), tint);
                GUIHelper.OutlineRect(scrollbar, new Color(0, 0, 0, alpha));
            }
        }
        internal override void Update()
        {
            active = viewable < total;
            sSize = (int)Math.Round((float)viewable / total * size);
            if (!Input.LeftD)
            {
                movInfo.dir = Orientation.NONE;
                movInfo.mOff = Direction.NONE;
            }
            Rectangle scrollbar = isVertical ? new Rectangle(position.X, position.Y + realPos, 15, sSize) : new Rectangle(position.X + realPos, position.Y, sSize, 15);
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
                    if (scrollbarPosition + Input.mDX > size - sSize)
                    {
                        movInfo.mOff |= Direction.RIGHT;
                        scrollbarPosition = size - sSize;
                    }
                    if (scrollbarPosition + Input.mDX < 0)
                    {
                        movInfo.mOff |= Direction.LEFT;
                        scrollbarPosition = 0;
                    }
                    if ((movInfo.mOff & (Direction.LEFT | Direction.RIGHT)) == 0)
                    {
                        scrollbarPosition += Input.mDX;
                    }
                    if (Input.mX < movInfo.pt.X + scrollbar.X && (movInfo.mOff & Direction.RIGHT) != 0)
                    {
                        movInfo.mOff &= ~Direction.RIGHT;
                    }
                    if (Input.mX > movInfo.pt.X + scrollbar.X && (movInfo.mOff & Direction.LEFT) != 0)
                    {
                        movInfo.mOff &= ~Direction.LEFT;
                    }
                }
            }
            scrollbarPosition = scrollbarPosition.Clamp(0, size - sSize);
        }
    }
}
