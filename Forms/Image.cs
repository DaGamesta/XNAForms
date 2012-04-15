using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAForms.Forms
{
    /// <summary>
    /// Represents an image.
    /// </summary>
    public sealed class Image : Control
    {
        /// <summary>
        /// Tinting color for the texture.
        /// </summary>
        public Color color;
        private float scale;
        private Texture2D tex;

        /// <summary>
        /// Creates a new image.
        /// </summary>
        /// <param name="position">Position for the new image.</param>
        /// <param name="tex">Texture for the new image.</param>
        /// <param name="scale">Scale of the texture.</param>
        public Image(Position position, Texture2D tex, float scale = 1)
            : base(position, new Size((int)Math.Round(tex.Width * scale), (int)Math.Round(tex.Height * scale)))
        {
            this.scale = scale;
            this.tex = tex;
        }

        internal override void Draw()
        {
            GUIHelper.sb.Draw(tex, position + GUIHelper.offset, null, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
        internal override void Update()
        {
        }
    }
}
