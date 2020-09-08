using System;
using TokenTracker.Models;
using Xamarin.Forms;

namespace TokenTracker
{
    public class WalletTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AddTokenItemTemplate { get; set; }

        public DataTemplate ViewTokenItemTemplate { get; set; }

        public DataTemplate ViewTotalItemTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is WalletAddTokenItem)
            {
                return AddTokenItemTemplate;
            }
            else if (item is WalletViewTokenItem)
            {
                return ViewTokenItemTemplate;
            }
            else if (item is WalletViewTotalItem)
            {
                return ViewTotalItemTemplate;
            }

            throw new NotImplementedException();
        }
    }
}
