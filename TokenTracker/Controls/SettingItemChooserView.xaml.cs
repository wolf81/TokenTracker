using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TokenTracker.Controls
{
    public partial class SettingItemChooserView : ContentView
    {
        private ChooserSettingItem Item => BindingContext as ChooserSettingItem;

        public SettingItemChooserView()
        {
            InitializeComponent();
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectedValueChanged(picker.SelectedIndex);
        }

        private void OnSelectedValueChanged(int selectedIndex)
        {
            if (selectedIndex == -1) { return; }

            if (Item?.Items[selectedIndex] is KeyValuePair<string, string> item)
            {
                Item?.SelectedItemChanged(item);
            }
        }
    }
}
