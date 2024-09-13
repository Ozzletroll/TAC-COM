using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private const double MOUSE_MOVE_THRESHOLD = 1;
        private const double INTERVAL = 1;
        private const float SENSITIVITY = 2f;

        private Point initialPosition;

        public static readonly DependencyProperty MinProperty 
            = DependencyProperty.Register("Min", typeof(int), typeof(Dial));
        public int Min
        {
            get => (int)GetValue(MinProperty); 
            set 
            { 
                SetValue(MinProperty, value); 
            }
        }

        public static readonly DependencyProperty MaxProperty 
            = DependencyProperty.Register("Max", typeof(int), typeof(Dial));
        public int Max
        {
            get => (int)GetValue(MaxProperty);
            set 
            { 
                SetValue(MaxProperty, value); 
            }
        }

        public static readonly DependencyProperty ValueProperty 
            = DependencyProperty.Register("Value", typeof(int), typeof(Dial), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnValuePropertyChanged)));
        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set 
            { 
                SetValue(ValueProperty, Math.Clamp(value, Min, Max)); 
            }
        }
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Dial? f = d as Dial;
            f.Value = (int)e.NewValue;
            f.RenderDisplay();
        }

        private void RenderDisplay()
        {
            // Draw dial outer ring display
            int offset = 190;
            double angle = FULLRANGE_ANGLE / (Max - Min) * (Value - Min);
            double rad = (Math.PI / 180) * (angle + START_ANGLE_OFFSET);

            double x = offset * Math.Cos(rad) + offset;
            double y = offset * Math.Sin(rad) + offset;

            display.IsLargeArc = angle > 180;
            display.Point = new Point(x, y);

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
            Mouse.Capture(null);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.Captured == this)
            {
                Point currentPosition = Mouse.GetPosition(this);

                double dY = (initialPosition.X - currentPosition.X);
                if (Math.Abs(dY) > MOUSE_MOVE_THRESHOLD)
                {
                    Value -= (int)(Math.Sign(dY) * INTERVAL * SENSITIVITY);
                    initialPosition = currentPosition;
                }

            }
        }

        public Dial()
        {
            InitializeComponent();
            MouseLeftButtonDown += OnMouseLeftButtonDown;
            MouseUp += OnMouseUp;
            MouseMove += OnMouseMove;
        }
    }
}
