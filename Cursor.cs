using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAForms
{
    internal struct Cursor
    {
        internal byte flip;
        internal CursorType type;

        internal Cursor(CursorType type)
        {
            flip = 0;
            this.type = type;
        }

        internal void Draw()
        {
            if (type == CursorType.NORMAL)
            {
                GUIHelper.sb.Draw(GUI.cursorTexs[0], new Vector2(Input.mX, Input.mY), Color.White);
            }
            else
            {
                GUIHelper.sb.Draw(GUI.cursorTexs[(int)type],new Vector2(Input.mX, Input.mY) +
                    new Vector2(-GUI.cursorTexs[(int)type].Width / 2, -GUI.cursorTexs[(int)type].Height / 2),null, Color.White, 0, Vector2.Zero, 1, (SpriteEffects)flip, 0);
            }
        }
    }
}
