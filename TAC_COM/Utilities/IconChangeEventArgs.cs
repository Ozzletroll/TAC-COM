namespace TAC_COM.Utilities
{
    /// <summary>
    /// EventArgs class for use with <see cref="IconChangeEventArgs"/>.
    /// </summary>
    /// <param name="iconPath"> The string path to the new icon to be used.</param>
    /// <param name="tooltip"> The string tooltip to display in the sytem tray.</param>
    public class IconChangeEventArgs(string iconPath, string tooltip) : EventArgs
    {
        public string IconPath = iconPath;
        public string Tooltip = tooltip;
    }
}
