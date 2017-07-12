using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using TZDriver.Models.Tools.Extensions;
using TZDriver.Models.Tools.Utilities;
using Windows.ApplicationModel;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using XamlBrewer.Uwp.Controls;

namespace TZDriver.Models.Controls
{
    public class TripViewControl : Control
    {
        public List<FrameworkElement> Views { get; set; } = new List<FrameworkElement>();

        // Enable data binding to change the state
        public int DisplayIndex
        {
            get { return (int)GetValue(DisplayIndexProperty); }
            set
            {
                if (_animating) return;
                var nextIndex = value;
                if (nextIndex >= Views.Count) nextIndex = 0;
                SetValue(DisplayIndexProperty, nextIndex);
            }
        }

        public static readonly DependencyProperty DisplayIndexProperty =
            DependencyProperty.Register("DisplayIndex", typeof(int), typeof(TripViewControl),
                new PropertyMetadata(0, OnDisplayIndexPropertyChanged));

        // If there is a button on your view and you have set e.Handled=true,
        // this will NOT work even if this property is enabled
        public bool AllowTapToFlip
        {
            get { return (bool)GetValue(AllowTapToFlipProperty); }
            set { SetValue(AllowTapToFlipProperty, value); }
        }

        public static readonly DependencyProperty AllowTapToFlipProperty =
            DependencyProperty.Register("AllowTapToFlip", typeof(bool), typeof(TripViewControl), new PropertyMetadata(false));

        public RotateAxis RotationAxis
        {
            get { return (RotateAxis)GetValue(RotaetAxisProperty); }
            set { SetValue(RotaetAxisProperty, value); }
        }

        public static readonly DependencyProperty RotaetAxisProperty =
            DependencyProperty.Register("RotationAxis", typeof(RotateAxis), typeof(TripViewControl),
                new PropertyMetadata(RotateAxis.X));

        public Direction FlipDirection
        {
            get { return (Direction)GetValue(FlipDirectionProperty); }
            set { SetValue(FlipDirectionProperty, value); }
        }

        public static readonly DependencyProperty FlipDirectionProperty =
            DependencyProperty.Register("FlipDirection", typeof(Direction), typeof(TripViewControl), new PropertyMetadata(Direction.FrontToBack));

        private static async void OnDisplayIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TripViewControl;
            await control.NextAsync();
        }

        public int AnimationDuration
        {
            get { return (int)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }

        public static readonly DependencyProperty AnimationDurationProperty =
            DependencyProperty.Register("AnimationDuration", typeof(int), typeof(TripViewControl), new PropertyMetadata(2000));

        public bool EnablePerspect
        {
            get { return (bool)GetValue(EnablePerspectroperty); }
            set { SetValue(EnablePerspectroperty, value); }
        }

        public static readonly DependencyProperty EnablePerspectroperty =
            DependencyProperty.Register("EnablePerspect", typeof(bool), typeof(TripViewControl), new PropertyMetadata(true));

        public double TripDuration
        {
            get { return (double)GetValue(TripDurationProperty); }
            set { SetValue(TripDurationProperty, value); }
        }

        public static readonly DependencyProperty TripDurationProperty =
            DependencyProperty.Register("TripDuration", typeof(double), typeof(TripViewControl), new PropertyMetadata(null, new PropertyChangedCallback(OnTripDurationChanged)));

        private static void OnTripDurationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            TripViewControl TripValue = sender as TripViewControl;
            if (!isTimerStarted)
                TripValue.UpdateDisplayIndex();
        }

        private void UpdateDisplayIndex()
        {
            if (!smartTimer.isTimerRunning)
                smartTimer = new IntervalTimer(15);
            smartTimer.StartTimer(async () =>
            {
                if (!isFlipped)
                {
                    isFlipped = true;
                    DisplayIndex++;
                    await Task.Delay(TimeSpan.FromSeconds(30));
                    DisplayIndex++;
                    isFlipped = false;
                }
            });
            isTimerStarted = true;
        }

        private Grid _rootGrid;
        private Compositor _compositor;
        private int _zindex = 1;
        private static bool isFlipped = false;
        private static bool isTimerStarted = false;
        private IntervalTimer smartTimer;

        public TripViewControl()
        {
            DefaultStyleKey = typeof(TripViewControl);
            smartTimer = new IntervalTimer(15);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (!DesignMode.DesignModeEnabled)
            {
                _rootGrid = GetTemplateChild("RootGrid") as Grid;
                _rootGrid.Tapped += _rootGrid_Tapped;
                _rootGrid.SizeChanged += _rootGrid_SizeChanged;
                _compositor = this.GetVisual().Compositor;

                var firstView = Views.LastOrDefault();
                if (firstView != null)
                {
                    Canvas.SetZIndex(firstView, _zindex++);
                    _rootGrid.Children.Add(firstView);
                }
            }
        }

        // Copy from WindowsUIDevLabs
        private void UpdatePerspective(Visual visual)
        {
            Vector2 rootSize = new Vector2((float)_rootGrid.ActualWidth, (float)_rootGrid.ActualHeight);

            // This matrix represents a translation along Z aix, the value is -1.0f / rootSize.x
            Matrix4x4 perspective = new Matrix4x4(
                        1.0f, 0.0f, 0.0f, 0.0f,
                        0.0f, 1.0f, 0.0f, 0.0f,
                        0.0f, 0.0f, 1.0f, -1.0f / rootSize.X,
                        0.0f, 0.0f, 0.0f, 1.0f);

            visual.TransformMatrix =
                               Matrix4x4.CreateTranslation(-rootSize.X / 2, -rootSize.Y / 2, 0f) *      // Translate to origin
                               perspective *                                                            // Apply perspective at origin
                               Matrix4x4.CreateTranslation(rootSize.X / 2, rootSize.Y / 2, 0f);         // Translate back to original position
        }

        private void _rootGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!DesignMode.DesignModeEnabled)
            {
                if (EnablePerspect)
                {
                    UpdatePerspective(_rootGrid.GetVisual());
                }
            }
        }

        private FrameworkElement _lastFrontView;
        private bool _animating = false;

        private async Task NextAsync()
        {
            if (Views.Count > 1)
            {
                var nextIndex = Views.Count - DisplayIndex - 1;
                var backView = Views[nextIndex];

                // The last front view is the current back view,replace it with the new back view.
                _rootGrid.Children.Remove(_lastFrontView);

                var frontView = _rootGrid.Children.Last() as FrameworkElement;
                _lastFrontView = frontView;

                _rootGrid.Children.Insert(0, backView);

                // Actual width and height is needed.
                await frontView.WaitForNonZeroSizeAsync();
                await backView.WaitForNonZeroSizeAsync();

                var frontViewVisual = _rootGrid.Children[1].GetVisual();
                var backViewVisual = _rootGrid.Children[0].GetVisual();

                backViewVisual.Opacity = 0;
                frontViewVisual.Opacity = 1;

                // Set the rotation center as the middle point
                backViewVisual.CenterPoint = new Vector3((float)(backView.ActualWidth / 2f), (float)(backView.ActualHeight / 2f), 0f);
                frontViewVisual.CenterPoint = new Vector3((float)(frontView.ActualWidth / 2f), (float)(frontView.ActualHeight / 2f), 0f);

                // First rotate the back view to 180
                backViewVisual.RotationAngleInDegrees = 180f;

                var linear = _compositor.CreateLinearEasingFunction();
                var delta = GetDeltaDegreeByDirection();

                var frontViewAnimation = _compositor.CreateScalarKeyFrameAnimation();
                frontViewAnimation.InsertKeyFrame(1f, frontViewVisual.RotationAngleInDegrees + delta, linear);
                frontViewAnimation.Duration = TimeSpan.FromMilliseconds(AnimationDuration);

                var backViewAnimation = _compositor.CreateScalarKeyFrameAnimation();
                backViewAnimation.InsertKeyFrame(1f, backViewVisual.RotationAngleInDegrees + delta, linear);
                backViewAnimation.Duration = TimeSpan.FromMilliseconds(AnimationDuration);

                SetRotatioinAxis(frontViewVisual);
                SetRotatioinAxis(backViewVisual);

                var batch = _compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
                _animating = true;

                // Rotate two views to 90 or -90 degree(depends on the FlipDirection)
                frontViewVisual.StartAnimation("RotationAngleInDegrees", frontViewAnimation);
                backViewVisual.StartAnimation("RotationAngleInDegrees", backViewAnimation);

                batch.Completed += (sender, e) =>
                {
                    backViewVisual.Opacity = 1;
                    frontViewVisual.Opacity = 0;

                    // Then, make the back view on top of front view, continue the animation
                    Canvas.SetZIndex(backView, _zindex++);

                    frontViewAnimation.InsertKeyFrame(1f, frontViewVisual.RotationAngleInDegrees + delta, linear);
                    backViewAnimation.InsertKeyFrame(1f, backViewVisual.RotationAngleInDegrees + delta, linear);

                    var batch2 = _compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
                    frontViewVisual.StartAnimation("RotationAngleInDegrees", frontViewAnimation);
                    backViewVisual.StartAnimation("RotationAngleInDegrees", backViewAnimation);
                    batch2.Completed += Batch2_Completed;
                    batch2.End();
                };
                batch.End();
            }
        }

        private void Batch2_Completed(object sender, CompositionBatchCompletedEventArgs args)
        {
            _animating = false;
        }

        private void SetRotatioinAxis(Visual visual)
        {
            var rotationAxis = (int)RotationAxis;
            visual.RotationAxis = new Vector3((rotationAxis & 2) != 0 ? 1 : 0, (rotationAxis & 1) != 0 ? 1 : 0, 0f);
        }

        private float GetDeltaDegreeByDirection()
        {
            return FlipDirection == Direction.FrontToBack ? 90f : -90f;
        }

        private async void _rootGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!AllowTapToFlip) return;

            if (!isFlipped)
            {
                isFlipped = true;
                DisplayIndex++;
                await Task.Delay(TimeSpan.FromSeconds(30));
                DisplayIndex++;
                isFlipped = false;
            }
        }
    }
}
