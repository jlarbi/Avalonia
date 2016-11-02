namespace Avalonia.Controls
{
    /// <summary>
    /// Definition of the <see cref="WindowEdge"/> enumeration.
    /// </summary>
    public enum WindowEdge
    {
        //Please don't reorder stuff here, I was lazy to write proper conversion code
        //so the order of values is matching one from GTK

        /// <summary>
        /// North west.
        /// </summary>
        NorthWest = 0,

        /// <summary>
        /// North
        /// </summary>
        North,

        /// <summary>
        /// North east.
        /// </summary>
        NorthEast,

        /// <summary>
        /// West.
        /// </summary>
        West,

        /// <summary>
        /// East.
        /// </summary>
        East,

        /// <summary>
        /// South west.
        /// </summary>
        SouthWest,

        /// <summary>
        /// South.
        /// </summary>
        South,

        /// <summary>
        /// South east.
        /// </summary>
        SouthEast,
    }
}