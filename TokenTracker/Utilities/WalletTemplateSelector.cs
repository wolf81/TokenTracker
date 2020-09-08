using System;
using TokenTracker.Models;
using Xamarin.Forms;

namespace TokenTracker
{
    public class WalletTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AddItemTemplate { get; set; }

        public DataTemplate ViewItemTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is WalletAddItem)
            {
                return AddItemTemplate;
            }
            else if (item is WalletViewItem)
            {
                return ViewItemTemplate;
            }

            throw new NotImplementedException();
        }
    }
}
