using Microsoft.Xna.Framework;

namespace XNAForms
{
    internal struct GradientPoint
    {
        internal Color color;
        internal int pxOffset;
        internal GradientPoint(Color color, int pxOffset)
        {
            this.color = color;
            this.pxOffset = pxOffset;
        }
    }
}
