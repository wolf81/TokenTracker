using System.Collections.ObjectModel;
using TokenTracker.ViewModels.Base;

namespace TokenTracker.ViewModels
{
    public class TokenWalletViewModel : ViewModelBase
    {
        private ObservableCollection<string> items = new ObservableCollection<string> { "Hi 1", "Hi 2" };
        public ObservableCollection<string> Items {
            get => items;
            set => SetProperty(ref items, value);
        }

        public TokenWalletViewModel()
        {
            Title = "Wallet";
        }
    }
}
