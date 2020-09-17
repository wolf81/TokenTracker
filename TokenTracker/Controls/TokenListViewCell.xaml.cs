using System.Windows.Input;
using TokenTracker.Models;
using TokenTracker.Services;
using TokenTracker.ViewModels.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TokenTracker.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TokenListViewCell : ViewCell
    {
        public Token Token
        {
            get => (Token)GetValue(TokenProperty);
            set => SetValue(TokenProperty, value);
        }

        public static readonly BindableProperty TokenProperty = BindableProperty.Create(nameof(Token), typeof(Token), typeof(TokenListViewCell), null, propertyChanged: Handle_PropertyChanged);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(TokenListViewCell), null, BindingMode.OneWay);

        public TokenListViewCell()
        {
            InitializeComponent();
        }

        #region Private

        private static async void Handle_PropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var isCached = false;

            if (newValue is Token token)
            {
                var cache = ViewModelLocator.Resolve<ICache>();
                isCached = await cache.GetTokenAsync(token.Id) != null;
            }

            (bindable as TokenListViewCell).Update(isCached: isCached);
        }

        private void Update(bool isCached)
        {
            nameLabel.Text = Token?.Name ?? "";
            symbolLabel.Text = Token?.Symbol ?? "";
            checkmarkImage.IsVisible = isCached;
        }

        private void Handle_TokenListViewCell_Tapped(object sender, System.EventArgs e)
        {
            checkmarkImage.IsVisible = !checkmarkImage.IsVisible;

            Command?.Execute(Token);
        }

        #endregion
    }
}
