using System.ComponentModel;
using TAC_COM.Models.Interfaces;
using TAC_COM.Utilities;

namespace TAC_COM.ViewModels
{
    /// <summary>
    /// Viewmodel to expose the <see cref="KeybindManager"/>
    /// properties to the <see cref="Views.KeybindWindowView"/>.
    /// </summary>
    public class KeybindWindowViewModel : ViewModelBase, IDisposable
    {
        /// <summary>
        /// Gets or sets the underlying <see cref="IKeybindManager"/>
        /// to which all data bindings occur.
        /// </summary>
        public IKeybindManager KeybindManager { get; set; }

        private string? newKeybindName;

        /// <summary>
        /// Gets or sets the string value representing
        /// the proposed new keybind name.
        /// </summary>
        public string? NewKeybindName
        {
            get => newKeybindName;
            set
            {
                newKeybindName = "[ " + value + " ]";
                OnPropertyChanged(nameof(NewKeybindName));
            }
        }

        /// <summary>
        /// Gets or sets the value representing if the
        /// new keybind should be allowed to reach 
        /// other applications.
        /// </summary>
        public bool PassthroughState
        {
            get => KeybindManager.PassthroughState;
            set
            {
                KeybindManager.PassthroughState = value;
                OnPropertyChanged(nameof(PassthroughState));
            }
        }

        /// <summary>
        /// Delegate that represents the method that will handle the close
        /// event of the <see cref="Views.KeybindWindowView"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void CloseEventHandler(object sender, EventArgs e);

        /// <summary>
        /// Event that occurs when the window is closed.
        /// </summary>
        public event CloseEventHandler? Close;

        /// <summary>
        /// Method to manually raise the close event.
        /// </summary>
        protected virtual void RaiseClose()
        {
            Close?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// <see cref="RelayCommand"/> to confirm and close the new keybind
        /// dialog. Bound to the "Confirm" button in the
        /// <see cref="Views.KeybindWindowView"/>.
        /// </summary>
        public RelayCommand CloseKeybindDialog => new(execute => ExecuteCloseKeybindDialog());

        /// <summary>
        /// Method to confirm the selected new keybind, end the
        /// existing keybind subscription and close the dialog
        /// window.
        /// </summary>
        private void ExecuteCloseKeybindDialog()
        {
            KeybindManager.UpdateKeybind();
            RaiseClose();
        }

        public override void Dispose()
        {
            KeybindManager.Dispose();
            KeybindManager.ToggleUserKeybindSubscription(false);
            KeybindManager.PropertyChanged -= KeybindManager_PropertyChanged;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Method to handle <see cref="NotifyProperty"/> property changes 
        /// from the <see cref="KeybindManager"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data for the PropertyChanged event.</param>
        private void KeybindManager_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Models.KeybindManager.NewPTTKeybind))
            {
                NewKeybindName = KeybindManager.NewPTTKeybind?.ToString().ToUpper() ?? "";
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="KeybindWindowViewModel"/>.
        /// </summary>
        /// <param name="_keybindManager"> The <see cref="IKeybindManager"/> to expose
        /// to the view.</param>
        public KeybindWindowViewModel(IKeybindManager _keybindManager)
        {
            KeybindManager = _keybindManager;
            KeybindManager.PropertyChanged += KeybindManager_PropertyChanged;
            KeybindManager.ToggleUserKeybindSubscription(true);
        }
    }
}
