namespace TAC_COM.Utilities
{
    /// <summary>
    /// EventArgs class for use with <see cref="ProfileChangeEventArgs"/>.
    /// </summary>
    /// <param name="icon"> The new icon to change to.</param>
    public class ProfileChangeEventArgs(System.Windows.Media.ImageSource icon) : EventArgs
    {
        public System.Windows.Media.ImageSource Icon = icon;
    }
}
