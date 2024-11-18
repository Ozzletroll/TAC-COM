using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

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

        public static readonly DependencyProperty MinProperty
            = DependencyProperty.Register("Min", typeof(float), typeof(Dial));
        public float Min
        {
            get => (float)GetValue(MinProperty);
            set
            {
                SetValue(MinProperty, value);
            }
        }

        public static readonly DependencyProperty MaxProperty
            = DependencyProperty.Register("Max", typeof(float), typeof(Dial));
        public float Max
        {
            get => (float)GetValue(MaxProperty);
            set
            {
                SetValue(MaxProperty, value);
            }
        }

        private float percentValue;
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

        public static readonly DependencyProperty ValueProperty
            = DependencyProperty.Register("Value", typeof(float), typeof(Dial), new FrameworkPropertyMetadata(0f, new PropertyChangedCallback(OnValuePropertyChanged)));
        public float Value
        {
            get => (float)GetValue(ValueProperty);
            set
            {
                SetValue(ValueProperty, Math.Clamp(value, Min, Max));
            }
        }
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Dial? f = d as Dial;
            f?.RenderDisplay();
        }

        public static readonly DependencyProperty IntervalProperty
            = DependencyProperty.Register("Interval", typeof(float), typeof(Dial));
        public float Interval
        {
            get => (float)GetValue(IntervalProperty);
            set
            {
                SetValue(IntervalProperty, value);
            }
        }

        public static readonly DependencyProperty SensitivityProperty
            = DependencyProperty.Register("Sensitivity", typeof(float), typeof(Dial));
        public float Sensitivity
        {
            get => (float)GetValue(SensitivityProperty);
            set
            {
                SetValue(SensitivityProperty, value);
            }
        }

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

            // Rotate marker
            DoubleAnimation animation = new();
            Duration duration = new(TimeSpan.FromMilliseconds(250));
            ExponentialEase acc = new()
            {
                EasingMode = EasingMode.EaseOut,
                Exponent = 5
            };
            animation.To = angle + START_MARKER_ANGLE;
            animation.Duration = duration;
            animation.EasingFunction = acc;
            Storyboard.SetTargetName(animation, "Marker");
            Storyboard.SetTargetProperty(animation, new PropertyPath(RotateTransform.AngleProperty));

            Storyboard storyboard = new();
            storyboard.Children.Add(animation);
            storyboard.Begin(this);
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(this);
            initialPosition = Mouse.GetPosition(this);
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Cursor = null;
            Mouse.Capture(null);
        }

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

        private void OnDialLoaded(object sender, RoutedEventArgs e)
        {
            percentValue = (Value - Min) / (Max - Min) * 100;
            RenderDisplay();
        }

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
