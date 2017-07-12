﻿using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace TZDriver.Controls
{
    public sealed partial class TripFareControl : UserControl
    {
        public TripFareControl()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty TripDurationProperty =
            DependencyProperty.Register("TripDuration", typeof(TimeSpan), typeof(TripFareControl), new PropertyMetadata(15, new PropertyChangedCallback(OnTripDurationChanged)));

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(int), typeof(TripFareControl), new PropertyMetadata(5));

        public static readonly DependencyProperty SegmentColorProperty =
            DependencyProperty.Register("SegmentColor", typeof(GradientBrush), typeof(TripFareControl), new PropertyMetadata(GetDefaultSegmentColor()));

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(int), typeof(TripFareControl), new PropertyMetadata(25, new PropertyChangedCallback(OnPropertyChanged)));

        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(TripFareControl), new PropertyMetadata(90d, new PropertyChangedCallback(OnPropertyChanged)));

        // Using a DependencyProperty as the backing store for ClientelRating.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TripFareProperty =
            DependencyProperty.Register("TripFare", typeof(double), typeof(TripFareControl), new PropertyMetadata(1500.0));



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

        public TimeSpan TripDuration
        {
            get
            {
                return (TimeSpan)GetValue(TripDurationProperty);
            }
            set
            {
                SetValue(TripDurationProperty, value);
            }
        }

        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        public double TripFare
        {
            get { return (double)GetValue(TripFareProperty); }
            set { SetValue(TripFareProperty, value); }
        }

        private static void OnTripDurationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            TripFareControl circle = sender as TripFareControl;
            circle.Angle = circle.TripDuration.Seconds * 6;
        }

        private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            TripFareControl circle = sender as TripFareControl;
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
            pathRoot.Margin = new Thickness(StrokeThickness, StrokeThickness, 0, 0);

            counterGrid.Height = (Radius * 2) - (StrokeThickness * 2.6);
            counterGrid.Width = (Radius * 2) - (StrokeThickness * 2.6);
            counterGrid.CornerRadius = new CornerRadius(((Radius * 2) - (StrokeThickness * 2.6)) / 2);

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
            pickupCounter.Text = string.Format("{0} {1:00000}", "₦", TripFare);
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
            RenderArc();
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
    }
}
