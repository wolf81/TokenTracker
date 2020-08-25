using System.Threading.Tasks;
using TokenTracker.Extensions;
using TokenTracker.Models;
using Xamarin.Forms;

namespace TokenTracker.Controls
{
    public class TokenView : ContentView
    {
        private readonly Label priceLabel = new Label { Text = "Bla", HorizontalTextAlignment = TextAlignment.Center, FontFamily = "Inconsolata-Regular", FontSize = 14 };
        private readonly Label symbolLabel = new Label { Text = "Hi", HorizontalTextAlignment = TextAlignment.Center, FontFamily = "Inconsolata-SemiBold", FontSize = 14 };

        public Color HighlightColor
        {
            get => (Color)GetValue(HighlightColorProperty);
            set => SetValue(HighlightColorProperty, value);
        }

        public static readonly BindableProperty HighlightColorProperty = BindableProperty.Create(nameof(HighlightColor), typeof(Color), typeof(TokenView), Color.Green);

        public Token Token {
            get => (Token)GetValue(TokenProperty);
            set => SetValue(TokenProperty, value);
        }

        public static readonly BindableProperty TokenProperty = BindableProperty.Create(nameof(Token), typeof(Token), typeof(TokenView), null, propertyChanged: Handle_PropertyChanged);

        public TokenView()
        {
            WidthRequest = 120;
            HeightRequest = 60;

            var grid = new Grid
            {
                Margin = new Thickness(1),
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Star },
                },
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star },
                },
            };

            grid.Children.Add(symbolLabel);
            Grid.SetRow(symbolLabel, 1);
            Grid.SetColumn(symbolLabel, 1);

            grid.Children.Add(priceLabel);
            Grid.SetRow(priceLabel, 2);
            Grid.SetColumn(priceLabel, 1);

            Content = grid;
        }

        private static void Handle_PropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as TokenView).Update();
        }

        private void Update()
        {
            priceLabel.Text = string.Format("{0:0.00#####}", Token.PriceUSD);
            symbolLabel.Text = Token.Symbol;

            StartAnimation();
        }

        private async void StartAnimation()
        {
            var color = BackgroundColor;
            await this.ColorTo(BackgroundColor, HighlightColor, (c) => { BackgroundColor = c; }, 250, Easing.CubicInOut);
            BackgroundColor = color;
        }
    }
}
