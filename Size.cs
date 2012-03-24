using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNAForms
{
    /// <summary>
    /// Represents an integral size.
    /// </summary>
    public struct Size
    {
        /// <summary>
        /// Height of the size.
        /// </summary>
        public int height;
        /// <summary>
        /// Width of the size.
        /// </summary>
        public int width;

        /// <summary>
        /// Creates a new size.
        /// </summary>
        /// <param name="width">Width of the new size.</param>
        /// <param name="height">Height of the new size.</param>
        public Size(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
        /// <summary>
        /// Creates a new size, truncating the floats.
        /// </summary>
        /// <param name="width">Width of the new size.</param>
        /// <param name="height">Height of the new size.</param>
        public Size(float width, float height)
        {
            this.width = (int)width;
            this.height = (int)height;
        }
    }
}
