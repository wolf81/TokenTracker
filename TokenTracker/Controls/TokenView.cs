using System.Windows.Input;
using TokenTracker.Extensions;
using TokenTracker.Models;
using Xamarin.Forms;

namespace TokenTracker.Controls
{
    public class TokenView : ContentView
    {
        private readonly TapGestureRecognizer TapRecognizer = new TapGestureRecognizer();

        private const double DefaultFontSize = 14;

        private readonly Label priceLabel = new Label { HorizontalTextAlignment = TextAlignment.Center, FontFamily = "Inconsolata-Regular", FontSize = DefaultFontSize };
        private readonly Label symbolLabel = new Label { HorizontalTextAlignment = TextAlignment.Center, FontFamily = "Inconsolata-SemiBold", FontSize = DefaultFontSize };
        private readonly Image addImage = new Image { Source = ImageSource.FromResource("TokenTracker.Resources.ic_add_b.png"), Aspect = Aspect.AspectFit, HeightRequest = 32, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };

        public ICommand Command {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(TokenView), null, BindingMode.OneWay);
        
        public Color HighlightColor
        {
            get => (Color)GetValue(HighlightColorProperty);
            set => SetValue(HighlightColorProperty, value);
        }

        public static readonly BindableProperty HighlightColorProperty = BindableProperty.Create(nameof(HighlightColor), typeof(Color), typeof(TokenView), Color.Green);

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(TokenView), DefaultFontSize, propertyChanged: Handle_PropertyChanged);

        public Token Token {
            get => (Token)GetValue(TokenProperty);
            set => SetValue(TokenProperty, value);
        }

        public static readonly BindableProperty TokenProperty = BindableProperty.Create(nameof(Token), typeof(Token), typeof(TokenView), null, propertyChanged: Handle_PropertyChanged);

        public TokenView()
        {
            WidthRequest = 120;
            HeightRequest = 60;

            GestureRecognizers.Add(TapRecognizer);

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

            InputTransparent = false;
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();

            if (Parent == null)
            {
                TapRecognizer.Tapped -= Handle_TapRecognizer_Tapped;
            }
            else
            {
                TapRecognizer.Tapped += Handle_TapRecognizer_Tapped;
            }
        }

        #region Private

        private void Handle_TapRecognizer_Tapped(object sender, System.EventArgs e)
        {
            if (Command is ICommand command)
            {
                command.Execute(Token);
            }
        }

        private static void Handle_PropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as TokenView).Update();
        }

        private void Update()
        {
            symbolLabel.FontSize = FontSize;
            priceLabel.FontSize = FontSize;

            if (Token is Token token)
            {
                priceLabel.Text = string.Format("{0:0.00#####}", token.PriceUSD);
                symbolLabel.Text = token.Symbol;

                if (token != Token.Dummy)
                {
                    ShowHighlightAnimation();
                }
                else
                {
                    var grid = new Grid { };
                    grid.Children.Add(addImage);
                    Content = grid;
                }
            }
        }

        private async void ShowHighlightAnimation()
        {
            var color = BackgroundColor;
            await this.ColorTo(BackgroundColor, HighlightColor, (c) => { BackgroundColor = c; }, 250, Easing.CubicInOut);
            BackgroundColor = color;
        }

        #endregion
    }
}
