using System;
using System.Threading;
using System.Threading.Tasks;
using TokenTracker.ViewModels;
using TokenTracker.Views.Base;
using Xamarin.Forms;

namespace TokenTracker.Views
{
    public partial class TokenSearchView : ContentPageBase
    {
        private TokenSearchViewModel ViewModel => BindingContext as TokenSearchViewModel;

        private CancellationTokenSource throttleCts = new CancellationTokenSource();

        public TokenSearchView()
        {
            InitializeComponent();
        }

        public async void Handle_SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textLength = e.NewTextValue?.Length ?? 0;

            switch (textLength)
            {
                case 1: break;
                default: await DelayedQueryForKeyboardTypingSearches(e.NewTextValue); break;
            }
        }

        /// <summary>
        /// Runs in a background thread, checks for new Query and runs current one
        /// </summary>
        private async Task DelayedQueryForKeyboardTypingSearches(string query)
        {
            try
            {
                Interlocked.Exchange(ref throttleCts, new CancellationTokenSource()).Cancel();
                await Task
                    .Delay(TimeSpan.FromMilliseconds(500), throttleCts.Token)
                    .ContinueWith(async (t) => await ViewModel.SearchTokenAsync(query),
                            CancellationToken.None,
                            TaskContinuationOptions.OnlyOnRanToCompletion,
                            TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch
            {
                // Ignore any Threading errors
            }
        }

    }
}
