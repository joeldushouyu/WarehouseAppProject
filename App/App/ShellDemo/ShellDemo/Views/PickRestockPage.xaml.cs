using ShellDemo.Models;
using ShellDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;

namespace ShellDemo.Views
{
    public partial class PickRestockPage : ContentPage
    {
    

        public PickRestockPage(PickRestockViewModel pick)
        {
            ZxingScan = new ZXing.Net.Mobile.Forms.ZXingScannerView();
            InitializeComponent();

            BindingContext = pick;
 
        }

        void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
            PickRestockViewModel ord = (PickRestockViewModel)this.BindingContext;
            ord.ItemBarcodeByUser = result.Text;
        }

        private void OnPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            PickRestockViewModel ord = this.BindingContext as PickRestockViewModel;
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                ord.UserAction = picker.Items[selectedIndex];
            }
        }

        protected override void OnDisappearing()
        {
            ZxingScan = null;
            base.OnDisappearing();
        }
    }
}