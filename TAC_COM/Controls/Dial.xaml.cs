using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TAC_COM.Controls
{
    /// <summary>
    /// Interaction logic for Dial.xaml
    /// </summary>
    public partial class Dial : UserControl
    {
        private const double FULLRANGE_ANGLE = 270;
        private const double START_ANGLE_OFFSET = 90;
        private const double START_MARKER_ANGLE = 225;

        private const double MOUSE_MOVE_THRESHOLD = 2;

        private Point initialPosition;

        /// <summary>
        /// Dependency property for <see cref="Min"/>
        /// </summary>
        public static readonly DependencyProperty MinProperty
            = DependencyProperty.Register("Min", typeof(float), typeof(Dial));

        /// <summary>
        /// Gets or sets the value representing the minimum
        /// value of the UI dial.
        /// </summary>
        public float Min
        {
            get => (float)GetValue(MinProperty);
            set
            {
                SetValue(MinProperty, value);
            }
        }

        /// <summary>
        /// Dependency property for <see cref="Max"/>.
        /// </summary>
        public static readonly DependencyProperty MaxProperty
            = DependencyProperty.Register("Max", typeof(float), typeof(Dial));

        /// <summary>
        /// Gets or sets the value representing the maximum
        /// value of the UI dial.
        /// </summary>
        public float Max
        {
            get => (float)GetValue(MaxProperty);
            set
            {
                SetValue(MaxProperty, value);
            }
        }

        private float percentValue;

        /// <summary>
        /// Gets or sets the value representing the
        /// value of the dial as a percentage.
        /// </summary>
        public float PercentValue
        {
            get => percentValue;
            set
            {
                percentValue = Math.Clamp(value, 0, 100);
                float calculatedValue = MathF.Round((Min + (percentValue / 100) * (Max - Min)) / Interval) * Interval;

                // Prevent -0 values
                if (Math.Abs(calculatedValue) < float.Epsilon)
                {
                    calculatedValue = 0;
                }

                Value = calculatedValue;
            }
        }

        /// <summary>
        /// Dependency property for <see cref="Value"/>.
        /// </summary>
        public static readonly DependencyProperty ValueProperty
            = DependencyProperty.Register("Value", typeof(float), typeof(Dial), new FrameworkPropertyMetadata(0f, new PropertyChangedCallback(OnValuePropertyChanged)));

        /// <summary>
        /// Gets or sets the value representing the
        /// current value of the dial.
        /// </summary>
        public float Value
        {
            get => (float)GetValue(ValueProperty);
            set
            {
                SetValue(ValueProperty, Math.Clamp(value, Min, Max));
            }
        }

        /// <summary>
        /// Method called when the value property changes on the <see cref="ValueProperty"/>.
        /// </summary>
        /// <param name="d">The <see cref="DependencyObject"/> on which the property changed.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> for the event.</param>
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Dial? f = d as Dial;
            f?.OnValueChanged((float)e.OldValue, (float)e.NewValue);
            f?.RenderDisplay();
        }

        /// <summary>
        /// Method called when the value property changes on the <see cref="ValueProperty"/>.
        /// Updates the percentage value of the dial when the user manually enters a
        /// specific value.
        /// </summary>
        /// <param name="oldValue"> The old value of the input.</param>
        /// <param name="newValue"> The new value of the input.</param>
        protected virtual void OnValueChanged(float oldValue, float newValue)
        {
            PercentValue = (newValue - Min) / (Max - Min) * 100;
        }

        /// <summary>
        /// The dependency property for <see cref="Interval"/>.
        /// </summary>
        public static readonly DependencyProperty IntervalProperty
            = DependencyProperty.Register("Interval", typeof(float), typeof(Dial));

        /// <summary>
        /// Gets or sets the value representing the interval
        /// or increment that the dial changes at.
        /// </summary>
        public float Interval
        {
            get => (float)GetValue(IntervalProperty);
            set
            {
                SetValue(IntervalProperty, value);
            }
        }

        /// <summary>
        /// Dependency property for <see cref="Sensitivity"/>.
        /// </summary>
        public static readonly DependencyProperty SensitivityProperty
            = DependencyProperty.Register("Sensitivity", typeof(float), typeof(Dial));

        /// <summary>
        /// Gets or sets the value representing the sensitivity
        /// of the UI dial when rotated with the mouse.
        /// </summary>
        public float Sensitivity
        {
            get => (float)GetValue(SensitivityProperty);
            set
            {
                SetValue(SensitivityProperty, value);
            }
        }

        /// <summary>
        /// Method to update the dial UI elements to reflect the
        /// new values of the control.
        /// </summary>
        private void RenderDisplay()
        {
            // Draw dial outer ring display
            int offset = 190;
            double angle = FULLRANGE_ANGLE / (Max - Min) * (Value - Min);
            double rad = (Math.PI / 180) * (angle + START_ANGLE_OFFSET);

            double x = offset * Math.Cos(rad) + offset;
            double y = offset * Math.Sin(rad) + offset;

            Gauge.IsLargeArc = angle > 180;
            Gauge.Point = new Point(x, y);

            // Rotate dial marker
            RotateTransform rotateTransform = Marker;
            rotateTransform.Angle = angle + START_MARKER_ANGLE;
        }

        /// <summary>
        /// Method to handle the mouse left click button down event,
        /// capturing the initial mouse position.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data for the data available event.</param>
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(this);
            initialPosition = Mouse.GetPosition(this);
        }

        /// <summary>
        /// Method to handle the mouse left click button release event,
        /// ending the mouse capture.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data for the data available event.</param>
        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Cursor = null;
            Mouse.Capture(null);
        }

        /// <summary>
        /// Method to handle the mouse move event, calculating
        /// the distance the mouse has travelled from the
        /// initial position and converting the value to 
        /// a percentage of the total dial's available
        /// rotation.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data for the data available event.</param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.Captured == this)
            {
                Cursor = Cursors.SizeWE;

                Point currentPosition = Mouse.GetPosition(this);

                double movementDifference = (initialPosition.X - currentPosition.X) * Sensitivity;
                if (Math.Abs(movementDifference) > MOUSE_MOVE_THRESHOLD)
                {
                    PercentValue -= Math.Sign(movementDifference);
                    initialPosition = currentPosition;
                }
            }
        }

        /// <summary>
        /// Method to handle the dial load event, initialising
        /// the percentage dial value and updating the display.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data for the data available event.</param>
        private void OnDialLoaded(object sender, RoutedEventArgs e)
        {
            percentValue = (Value - Min) / (Max - Min) * 100;
            RenderDisplay();
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Dial"/>.
        /// </summary>
        public Dial()
        {
            InitializeComponent();
            MouseLeftButtonDown += OnMouseLeftButtonDown;
            MouseUp += OnMouseUp;
            MouseMove += OnMouseMove;
            Loaded += OnDialLoaded;
        }
    }
}
