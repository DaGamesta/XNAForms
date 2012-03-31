using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNAForms.Forms
{
    /// <summary>
    /// Represents a context menu area which contains all information needed for a context menu to be created.
    /// </summary>
    public sealed class ContextMenuArea : Control
    {
        internal List<ContextMenuOption> options = new List<ContextMenuOption>();
        /// <summary>
        /// Creates a new context menu area.
        /// </summary>
        /// <param name="position">Position for the new context menu area.</param>
        /// <param name="size">Size for the new context menu area.</param>
        public ContextMenuArea(Position position, Size size)
            : base(position, size)
        {
        }
        /// <summary>
        /// Adds a context menu option.
        /// </summary>
        /// <param name="option">Context menu option to be added.</param>
        public void Add(ContextMenuOption option)
        {
            options.Add(option);
        }

        internal override void Draw()
        {
        }
        internal override void Update()
        {
            if (rectangle.IntersectsMouse() && Input.RightR)
            {
                GUI.contextMenu = new ContextMenu(this);
            }
        }
    }
}
