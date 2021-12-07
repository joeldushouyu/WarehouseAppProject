using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShellDemo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;

namespace ShellDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PickOrderPage : ContentPage
    {
  
        public bool IsScanning
        {
            get;
            set;
        }

        public string ScanResult
        {
            get;
            set;
        }
        public PickOrderPage(PickOrderViewModel ordPage)
        {
            ZxingScan = new ZXing.Net.Mobile.Forms.ZXingScannerView();
            InitializeComponent();
            this.BindingContext = ordPage;
            ordPage.scanner = this.ZxingScan;
            NavigationPage.SetBackButtonTitle(this, ""); // hide backward button for PickRestockPage
        }


        void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
            PickOrderViewModel ord = (PickOrderViewModel)this.BindingContext;
            ord.BarCodeID = result.Text;
        }

  

    }
}