using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace TZDriver.Controls
{
    public sealed partial class PickupControl : UserControl
    {
        public PickupControl()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty RemainMinuteProperty =
            DependencyProperty.Register("RemainMinutes", typeof(double), typeof(PickupControl), new PropertyMetadata(5.0, new PropertyChangedCallback(OnRemainMinuteChanged)));

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(int), typeof(PickupControl), new PropertyMetadata(5));

        public static readonly DependencyProperty SegmentColorProperty =
            DependencyProperty.Register("SegmentColor", typeof(GradientBrush), typeof(PickupControl), new PropertyMetadata(GetDefaultSegmentColor()));

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(int), typeof(PickupControl), new PropertyMetadata(25, new PropertyChangedCallback(OnPropertyChanged)));

        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(PickupControl), new PropertyMetadata(120d, new PropertyChangedCallback(OnPropertyChanged)));

        public int Radius
        {
            get { return (int)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public GradientBrush SegmentColor
        {
            get { return (GradientBrush)GetValue(SegmentColorProperty); }
            set { SetValue(SegmentColorProperty, value); }
        }

        public int StrokeThickness
        {
            get { return (int)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public double RemainMinutes
        {
            get { return (double)GetValue(RemainMinuteProperty); }
            set { SetValue(RemainMinuteProperty, value); }
        }

        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        private static void OnRemainMinuteChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            PickupControl circle = sender as PickupControl;
            if (circle.RemainMinutes >= 1 && circle.RemainMinutes <= 60)
            {
                circle.Angle = (60 - circle.RemainMinutes) * 6;
                circle.PickupFlashingStoryboard.Begin();
            }
            else
            {
                circle.Angle = 360;
                circle.PickupFlashingStoryboard.Stop();
            }
        }

        private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            PickupControl circle = sender as PickupControl;
            circle.RenderArc();
        }

        private void RenderArc()
        {
            RenderArc(360, fullPath, fullFigure, fullSegment);
            RenderArc(Angle, pathRoot, pathFigure, arcSegment);
        }

        private void RenderArc(double Angle, Path pathRoot, PathFigure pathFigure, ArcSegment arcSegment)
        {
            Point startPoint = new Point(Radius, 0);
            Point endPoint = ComputeCartesianCoordinate(Angle, Radius);
            endPoint.X += Radius;
            endPoint.Y += Radius;

            pathRoot.Width = (Radius * 2) + StrokeThickness;
            pathRoot.Height = (Radius * 2) + StrokeThickness;
            //pathRoot.Margin = new Thickness(StrokeThickness, StrokeThickness, 0, 0);

            bool largeArc = Angle > 180.0;

            Size outerArcSize = new Size(Radius, Radius);

            pathFigure.StartPoint = startPoint;

            if (startPoint.X == Math.Round(endPoint.X) && startPoint.Y == Math.Round(endPoint.Y))
            {
                endPoint.X -= 0.01;
            }

            arcSegment.Point = endPoint;
            arcSegment.Size = outerArcSize;
            arcSegment.IsLargeArc = largeArc;
        }

        private Point ComputeCartesianCoordinate(double angle, double radius)
        {
            double angleRad = (Math.PI / 180.0) * (angle - 90);

            double x = radius * Math.Cos(angleRad);
            double y = radius * Math.Sin(angleRad);

            return new Point(x, y);
        }

        private void ControlLoaded(object sender, RoutedEventArgs e)
        {
            counterGrid.Height = (Radius * 2) - (StrokeThickness * 2.6);
            counterGrid.Width = (Radius * 2) - (StrokeThickness * 2.6);
            counterGrid.CornerRadius = new CornerRadius(((Radius * 2) - (StrokeThickness * 2.6)) / 2);

            FlashBackground.Height = (Radius * 2) - (StrokeThickness * 2.6);
            FlashBackground.Width = (Radius * 2) - (StrokeThickness * 2.6);
            FlashBackground.CornerRadius = new CornerRadius(((Radius * 2) - (StrokeThickness * 2.6)) / 2);

            RenderArc();

            pickupCounter.FontSize = Radius * 0.8;
        }

        private static LinearGradientBrush GetDefaultSegmentColor()
        {
            GradientStopCollection gsc = new GradientStopCollection()
            {
                new GradientStop
                {
                    Color = Colors.Red
                }
           };
            return new LinearGradientBrush(gsc, 0);
        }

        bool isFadeIn = false;

        private async void counterGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!isFadeIn)
            {
                ClientDetailFadeInStoryboard.Begin();
                isFadeIn = true;
                await Task.Delay(TimeSpan.FromSeconds(30));
                ClientDetailFadeOutStoryboard.Begin();
                isFadeIn = false;
            }
        }
    }
}
