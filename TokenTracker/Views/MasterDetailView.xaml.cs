﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TokenTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailView : MasterDetailPage
    {
        public MasterDetailView()
        {
            InitializeComponent();

            Detail = new ContentPage();
        }
    }
}
