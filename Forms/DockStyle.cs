namespace XNAForms.Forms
{
    /// <summary>
    /// Indicates how controls dock to their owners.
    /// </summary>
    public enum DockStyle : byte
    {
        /// <summary>
        /// Indicates that a control should not attempt to specially dock to its owner.
        /// </summary>
        NONE,
        /// <summary>
        /// Indicates that a control should attempt to dock to the left side of its owner.
        /// </summary>
        LEFT = 1,
        /// <summary>
        /// Indicates that a control should attempt to dock to the top side of its owner.
        /// </summary>
        TOP = 2,
        /// <summary>
        /// Indicates that a control should attempt to dock to the right side of its owner.
        /// </summary>
        RIGHT = 4,
        /// <summary>
        /// Indicates that a control should attempt to dock to the bottom side of its owner.
        /// </summary>
        BOTTOM = 8,
        /// <summary>
        /// Indicates that a control should attempt to fill up the space provided by its owner.
        /// </summary>
        FILL = 16,
    }
}
