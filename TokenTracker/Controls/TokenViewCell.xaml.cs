﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenTracker.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TokenTracker.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TokenViewCell : ViewCell
    {
        public Token Token {
            get => (Token)GetValue(TokenProperty);
            set => SetValue(TokenProperty, value);
        }

        public static readonly BindableProperty TokenProperty = BindableProperty.Create(nameof(Token), typeof(Token), typeof(TokenViewCell), null, propertyChanged: Handle_PropertyChanged);

        public TokenViewCell()
        {
            InitializeComponent();
        }

        #region Private

        private static void Handle_PropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as TokenViewCell).Update();
        }

        private void Update()
        {
            nameLabel.Text = Token?.Name ?? "";
            symbolLabel.Text = Token?.Symbol ?? "";
        }

        #endregion
    }
}
