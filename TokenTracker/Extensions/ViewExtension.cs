using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TokenTracker.Extensions
{
    public static class ViewExtension
    {
        public static Task<bool> ColorTo(this VisualElement self, Color fromColor, Color toColor, Action<Color> callback, uint length = 250, Easing easing = null)
        {
            Color transform(double t) =>
              Color.FromRgba(fromColor.R + t * (toColor.R - fromColor.R),
                             fromColor.G + t * (toColor.G - fromColor.G),
                             fromColor.B + t * (toColor.B - fromColor.B),
                             fromColor.A + t * (toColor.A - fromColor.A));
            return ColorAnimation(self, "ColorTo", transform, callback, length, easing);
        }

        public static void CancelAnimation(this VisualElement self)
        {
            self.AbortAnimation("ColorTo");
        }

		public static Task<bool> RgbColorAnimation(this VisualElement view, Color fromColor, Color toColor, Action<Color> callback, uint length = 250, Easing easing = null)
		{
			Func<double, Color> transform = (t) =>
			{
				return Color.FromRgba(fromColor.R + t * (toColor.R - fromColor.R), fromColor.G + t * (toColor.G - fromColor.G), fromColor.B + t * (toColor.B - fromColor.B),
				fromColor.A + t * (toColor.A - fromColor.A));
			};
			return ColorAnimation(view, "RgbColorAnimation", transform, callback, length, easing);
		}

		public static Task<bool> HslColorAnimation(this VisualElement view, Color fromColor, Color toColor, Action<Color> callback, uint length = 250, Easing easing = null)
		{
			Func<double, Color> transform = (t) =>
			{
				return Color.FromHsla(
				fromColor.Hue + t * (toColor.Hue - fromColor.Hue),
				fromColor.Saturation + t * (toColor.Saturation - fromColor.Saturation), fromColor.Luminosity + t * (toColor.Luminosity - fromColor.Luminosity),
				fromColor.A + t * (toColor.A - fromColor.A));
			};
			return ColorAnimation(view, "HslColorAnimation", transform, callback, length, easing);
		}

		public static void CancelRgbColorAnimation(this VisualElement view)
		{
			view.AbortAnimation("RgbColorAnimation");
		}

		public static void CancelHslColorAnimation(this VisualElement view)
		{
			view.AbortAnimation("HslColorAnimation");
		}

		private static Task<bool> ColorAnimation(VisualElement view, string name, Func<double, Color> transform, Action<Color> callback, uint length, Easing easing)
		{
			easing = easing ?? Easing.Linear;

			TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
			view.Animate(name, transform, callback, 16, length, easing, (value, canceled) => {
				taskCompletionSource.SetResult(canceled);
			});
			return taskCompletionSource.Task;
		}
	}
}
