using System;
using System.Collections.Generic;
using TokenTracker.ViewModels.Base;

namespace TokenTracker.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private List<string> items = new List<string> { "Hi" };
        public List<string> Items
        {
            get => items;
            set => SetProperty(ref items, value);
        }

        public SettingsViewModel()
        {
            Title = "Settings";
        }
    }
}
