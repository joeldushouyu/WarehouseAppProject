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

        /// <summary>
        /// Event handler invoke when the camera got a result from the scan
        /// </summary>
        /// <param name="result"></param>
        void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
            PickRestockViewModel ord = (PickRestockViewModel)this.BindingContext;
            ord.ItemBarcodeByUser = result.Text;
        }

        /// <summary>
        /// Event handler when user pick a choise in the Picker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        protected override void OnAppearing()
        {

            base.OnAppearing();
        }
    }
}