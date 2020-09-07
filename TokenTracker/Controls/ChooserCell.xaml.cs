using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TokenTracker.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChooserCell : ViewCell
    {
        private ChooserSettingItem Item => BindingContext as ChooserSettingItem;

        public ChooserCell()
        {
            InitializeComponent();            
        }

        #region Private

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectedValueChanged(picker.SelectedIndex);
        }

        private void OnSelectedValueChanged(int selectedIndex)
        {
            Item?.SelectedItemChanged(selectedIndex);
        }

        #endregion
    }
}
