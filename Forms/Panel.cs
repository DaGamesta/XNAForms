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
        /// The scrollbars that the panel holds.
        /// </summary>
        protected internal List<Scrollbar> scrollbars = new List<Scrollbar>();

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
        /// <summary>
        /// Adds a scrollbar.
        /// </summary>
        /// <param name="placement">Scrollbar placement.</param>
        /// <param name="t">Total function for scrollbar.</param>
        /// <param name="v">Viewable function for scrollbar.</param>
        protected void AddScrollbar(Placement placement, Func<int> t, Func<int> v)
        {
            Scrollbar scrollbar;
            switch (placement)
            {
                case Placement.TOP:
                    scrollbar = new Scrollbar(position, 0, Orientation.HORIZONTAL);
                    scrollbar.positionFunction = () => this.position;
                    scrollbar.sizeFunction = () => new Size(this.size.width, 15);
                    break;
                case Placement.RIGHT:
                    scrollbar = new Scrollbar(position, 0, Orientation.VERTICAL);
                    scrollbar.positionFunction = () => this.position + new Position(this.size.width - 15, 0);
                    scrollbar.sizeFunction = () => new Size(15, this.size.height);
                    break;
                case Placement.BOTTOM:
                    scrollbar = new Scrollbar(position, 0, Orientation.HORIZONTAL);
                    scrollbar.positionFunction = () => this.position + new Position(0, this.size.height - 15);
                    scrollbar.sizeFunction = () => new Size(this.size.width, 15);
                    break;
                default:
                    scrollbar = new Scrollbar(position, 0, Orientation.VERTICAL);
                    scrollbar.positionFunction = () => this.position;
                    scrollbar.sizeFunction = () => new Size(15, this.size.height);
                    break;
            }
            scrollbar.totalFunction = t;
            scrollbar.viewableFunction = v;
            scrollbars.Add(scrollbar);
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
