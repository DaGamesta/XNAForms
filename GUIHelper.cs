using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAForms
{
    internal static class GUIHelper
    {
        internal static SpriteFont font;
        internal static SpriteBatch sb;
        internal static Texture2D wTex;

        internal static void DrawLn(Position p1, Position p2, Color c)
        {
            sb.Draw(wTex, p1, null, c, (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X), Vector2.Zero, new Vector2(((Vector2)(p1 - p2)).Length(), 1), SpriteEffects.None, 0);
        }
        internal static void DrawStr(string str, Position pos, Color col)
        {
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                    if (i != 0 && j != 0)
                        sb.DrawString(font, str, pos + new Position(i, j), new Color(0, 0, 0, col.A));
            sb.DrawString(font, str, pos, col);
        }
        internal static void FillRect(Rectangle rect, Color col)
        {
            sb.Draw(wTex, rect, col);
        }
        internal static Texture2D GenGradTex(GradientPoint[] gp, Orientation gs)
        {
            List<GradientPoint> gradientPoints = gp.ToList<GradientPoint>();
            gradientPoints.OrderBy<GradientPoint, int>((g) => g.pxOffset);
            gradientPoints[0] = new GradientPoint(gradientPoints[0].color, 0);

            int dim = gradientPoints[gradientPoints.Count - 1].pxOffset;
            Texture2D tex = null;
            switch (gs)
            {
                case Orientation.HORIZONTAL:
                    tex = new Texture2D(GUI.game.GraphicsDevice, dim, 1);
                    break;
                default:
                    tex = new Texture2D(GUI.game.GraphicsDevice, 1, dim);
                    break;
            }
            int length = gradientPoints[gradientPoints.Count - 1].pxOffset;
            for (int i = 0; i < gradientPoints.Count - 1; i++)
            {
                GradientPoint g1 = gradientPoints[i];
                GradientPoint g2 = gradientPoints[i + 1];
                Vector3 baseColor = new Vector3(g1.color.R, g1.color.G, g1.color.B) / 255;
                Vector3 stepColor = new Vector3(g2.color.R - g1.color.R, g2.color.G - g1.color.G, g2.color.B - g1.color.B) / (255 * g2.pxOffset - g1.pxOffset);
                for (int j = 0; j < g2.pxOffset - g1.pxOffset; j++)
                {
                    if (gs == Orientation.HORIZONTAL)
                        tex.SetData<Color>(0, new Rectangle(j + g1.pxOffset, 0, 1, 1), new[] { new Color(baseColor) }, 0, 1);
                    else
                        tex.SetData<Color>(0, new Rectangle(0, j + g1.pxOffset, 1, 1), new[] { new Color(baseColor) }, 0, 1);
                    baseColor += stepColor;
                }
            }
            return tex;
        }
        internal static void OutlineRect(Rectangle r, Color c)
        {
            Rectangle r1 = new Rectangle(r.X - 1, r.Y - 1, r.Width + 2, r.Height + 2);
            DrawLn(new Position(r1.X, r1.Y), new Position(r1.X + r1.Width, r1.Y), c);
            DrawLn(new Position(r1.X + r1.Width, r1.Y), new Position(r1.X + r1.Width, r1.Y + r1.Height), c);
            DrawLn(new Position(r1.X + r1.Width, r1.Y + r1.Height), new Position(r1.X, r1.Y + r1.Height), c);
            DrawLn(new Position(r1.X, r1.Y + r1.Height), new Position(r1.X, r1.Y), c);
        }
        internal static Vector2 StrSize(string str)
        {
            return font.MeasureString(str);
        }
    }
}
