using System;
using System.Linq;
using CoreGraphics;
using UIKit;
using Xamarin.Forms.Internals;

namespace TokenTracker.Extensions
{
    public static class UIViewExtension
    {
        private const double ShowAnimationDuration = 0.1;
        private const double HideAnimationDuration = 1.2;

        public static void ShowToast(this UIView view, string message, double displayDuration)
        {
            // If a toast view was previously added, first remove the toast view from the superview to prevent overlapping toasts.
            view.Subviews
                .Where((subview) => subview is ToastView)
                .ForEach((toastView) => toastView.RemoveFromSuperviewAndDispose());

            using (var toastView = new ToastView(message))
            {
                view.Add(toastView);
                view.AddConstraints(new NSLayoutConstraint[]
                {
                    NSLayoutConstraint.Create(view, NSLayoutAttribute.BottomMargin, NSLayoutRelation.Equal, toastView, NSLayoutAttribute.Bottom, 1, 0),
                    NSLayoutConstraint.Create(view, NSLayoutAttribute.CenterXWithinMargins, NSLayoutRelation.Equal, toastView, NSLayoutAttribute.CenterX, 1, 0),
                    NSLayoutConstraint.Create(toastView, NSLayoutAttribute.Width, NSLayoutRelation.LessThanOrEqual, 1, 280),
                });

                toastView.ShowAnimated(() => toastView.HideAnimated(displayDuration));
            }
        }

        #region Private

        private static void ShowAnimated(this UIView view, Action completion)
        {
            UIView.AnimateNotify(ShowAnimationDuration,
                () => view.Alpha = 0.7f,
                (showFinished) =>
                {
                    if (!showFinished)
                    {
                        view.RemoveFromSuperviewAndDispose();
                    }
                    else
                    {
                        completion();
                    }
                }
            );
        }

        private static void HideAnimated(this UIView view, double delay)
        {
            UIView.AnimateNotify(
                HideAnimationDuration,
                delay,
                UIViewAnimationOptions.CurveEaseOut | UIViewAnimationOptions.BeginFromCurrentState,
                () => view.Alpha = 0,
                (hideFinished) => view.RemoveFromSuperviewAndDispose()
            );
        }

        private static void RemoveFromSuperviewAndDispose(this UIView view)
        {
            if (view.Handle != IntPtr.Zero)
            {
                view.RemoveFromSuperview();
                view.Dispose();
            }
        }

        private class ToastView : UILabel
        {
            private UIEdgeInsets _edgeInsets;
            public UIEdgeInsets EdgeInsets
            {
                get { return _edgeInsets; }
                set { _edgeInsets = value; InvalidateIntrinsicContentSize(); }
            }

            public ToastView(string message)
            {
                Text = message;
                TextAlignment = UITextAlignment.Center;
                BackgroundColor = UIColor.Black;
                TextColor = UIColor.White;
                TranslatesAutoresizingMaskIntoConstraints = false;
                Alpha = 0;
                Lines = 0;
                Font = UIFont.SystemFontOfSize(16);
                ClipsToBounds = true;
                Layer.CornerRadius = 10;
                EdgeInsets = new UIEdgeInsets(5, 5, 5, 5);
            }

            public override void DrawText(CGRect rect)
            {
                var textRect = _edgeInsets.InsetRect(rect);
                base.DrawText(textRect);
            }

            public override CGSize IntrinsicContentSize
            {
                get
                {
                    var size = base.IntrinsicContentSize;
                    size.Width += _edgeInsets.Left + _edgeInsets.Right;
                    size.Height += _edgeInsets.Top + _edgeInsets.Bottom;
                    return size;
                }
            }
        }

        #endregion
    }
}
