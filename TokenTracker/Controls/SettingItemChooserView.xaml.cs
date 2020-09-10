using System;
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
            Item?.SelectedItemChanged(selectedIndex);
        }
    }
}
