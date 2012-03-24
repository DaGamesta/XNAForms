using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNAForms.Forms
{
    /// <summary>
    /// Groups collections of controls together, and allows for two scrollbars as well.
    /// </summary>
    public class Panel : Control
    {
        /// <summary>
        /// The controls that the panel encapsulates.
        /// </summary>
        protected internal List<Control> controls = new List<Control>();

        /// <summary>
        /// Creates a new panel.
        /// </summary>
        /// <param name="position">Position for the new panel.</param>
        /// <param name="size">Size for the new panel.</param>
        /// <param name="controls">Controls for the new panel to hold.</param>
        public Panel(Position position, Size size, params Control[] controls)
            : base(position, size)
        {
            foreach (Control c in controls)
            {
                this.controls.Add(c);
            }
        }
        /// <summary>
        /// Adds a control.
        /// </summary>
        /// <param name="c">Control to add.</param>
        protected void Add(Control c)
        {
            controls.Add(c);
        }
        internal override void Draw()
        {
            foreach (Control c in controls)
            {
                c.Draw();
            }
        }
        internal override void Update()
        {
            foreach (Control c in controls)
            {
                c.Reposition(this);
                c.Update();
            }
        }
    }
}
