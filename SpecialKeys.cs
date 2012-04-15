using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNAForms
{
    /// <summary>
    /// Specifies special keys that are active.
    /// </summary>
    public enum SpecialKeys : short
    {
        /// <summary>
        /// Indicates the left arrow key.
        /// </summary>
        LEFT = 1,
        /// <summary>
        /// Indicates the up arrow key.
        /// </summary>
        UP = 2,
        /// <summary>
        /// Indicates the right arrow key.
        /// </summary>
        RIGHT = 4,
        /// <summary>
        /// Indicates the down arrow key.
        /// </summary>
        DOWN = 8,
        /// <summary>
        /// Indicates the backspace key.
        /// </summary>
        BACK = 16,
        /// <summary>
        /// Indicates the delete key.
        /// </summary>
        DELETE = 32,
        /// <summary>
        /// Indicates the enter key.
        /// </summary>
        ENTER = 64,
        /// <summary>
        /// Indicates the C key, copying text.
        /// </summary>
        C = 128,
        /// <summary>
        /// Indicates the V key, pasting text.
        /// </summary>
        V = 256
    }
}
