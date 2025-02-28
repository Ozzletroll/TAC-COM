
namespace TAC_COM.Utilities.MouseHook
{
    /// <summary>
    /// Enum to represent the allowd Windows messages used in the <see cref="MouseHookExtended"/>
    /// and the <see cref="TAC_COM.Models.KeybindManager"/>.
    /// </summary>
    public static class MouseMessageFilter
    {
        public static readonly HashSet<MouseMessages> AllowedMouseMessages =
        [
            MouseMessages.WM_XBUTTON1DOWN,
            MouseMessages.WM_XBUTTON1UP,
            MouseMessages.WM_XBUTTON2DOWN,
            MouseMessages.WM_XBUTTON2UP,
            MouseMessages.WM_MBUTTONDOWN,
            MouseMessages.WM_MBUTTONUP,
        ];
    }
}
