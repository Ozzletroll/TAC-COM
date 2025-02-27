namespace TAC_COM.Utilities.MouseHook
{
    /// <summary>
    /// Enum to represent the Windows messages used in the <see cref="MouseHookExtended"/>.
    /// </summary>
    public enum MouseMessages : uint
    {
        WM_MBUTTONDOWN = 0x0207,
        WM_MBUTTONUP = 0x0208,
        WM_XBUTTON1DOWN = 0x020B,
        WM_XBUTTON1UP = 0x020C,
        WM_XBUTTON2DOWN = 0x020D,
        WM_XBUTTON2UP = 0x020E,
    }
}