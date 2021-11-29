using Flurl.Http;
using ShellDemo.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
namespace ShellDemo.ViewModels
{

    public class PickOrderViewModel : BaseViewModel
    {
        private Order ord;
        public ZXing.Net.Mobile.Forms.ZXingScannerView scanner;

        private bool needConfirmation = false;
        private List<OrderAction> respond;

        private string _barCodeid;

        public string BarCodeID
        {
            get
            {
                return _barCodeid;
            }
            set
            {
                string newBarcode = value;
                if (valideBarCode(value))
                {
                    SetProperty(ref _barCodeid, value);
                    OnPropertyChanged(nameof(ErrorBarCodeMessage));
                    IsScanning = false;
                }
                else
                {
                    OnPropertyChanged(nameof(ErrorBarCodeMessage));
                }

            }
        }

        private bool _isScanning;
        public bool IsScanning
        {
            get => _isScanning;
            set
            {
                SetProperty(ref _isScanning, value);
                OnPropertyChanged(nameof(Scanning));
            }
        }

        private bool _isTyping;
        public bool IsTyping
        {
            get => _isScanning;
            set
            {
                SetProperty(ref _isTyping, value);
                OnPropertyChanged(nameof(IsTyping));
            }
        }

        public bool Scanning => IsScanning;


        public bool ErrorOccurred => _errorBarCodeMessage.Length != 0;
        private string _errorBarCodeMessage;
        public string ErrorBarCodeMessage
        {
            get => _errorBarCodeMessage;
            set
            {
                SetProperty(ref _errorBarCodeMessage, value);
                OnPropertyChanged(nameof(ErrorBarCodeMessage));
                OnPropertyChanged(nameof(CanGetOrder));
            }
        }

        private string _errorGetOrderMessage;
        public string ErrorGetOrderMessage
        {
            get => _errorGetOrderMessage;
            set
            {
                SetProperty(ref _errorGetOrderMessage, value);
                OnPropertyChanged(nameof(ErrorGetOrderMessage));
            }
        }
        public long OrderID => this.ord.IDAtDatabase;

        public bool HaveBarcodeID => ord.BarCode != 0;
        public bool CanChangeBarcodeID => !(HaveBarcodeID);

        public bool CanGetOrder => ErrorBarCodeMessage.Length == 0;
        public Command ScanCommand { get; }
        public Command PickOrderCommand { get; }
        public PickOrderViewModel(Order ord)
        {
            this.ord = ord;
            this._barCodeid = ord.BarCode.ToString(); // if no barcode being assinged, it will default be 0
            ScanCommand = new Command(OnScanning);
            PickOrderCommand = new Command(PickOrder);


        }

        private void ConfirmGetOrder()
        {
            try
            {   /*
                string url = MobileApp.GetSingletion().BaseUrl + "/confirm";
                var respondCode = url.WithTimeout(20).PostJsonAsync(ans).Result;
                */
                Services.ServerRequest.ConfirmPickOrderRequest();
                // means user successfully Get the order
                needConfirmation = false;
            }
            catch (Exception e)
            {
                if (e.InnerException is FlurlHttpTimeoutException)
                {
                    throw e; // tell user to retry in the onUpdate code's catch
                }
                else if (e.InnerException is FlurlHttpException)
                {
                    // most likely 404 code, means the server has already processsed it
                    int errorcode = (int)((Flurl.Http.FlurlHttpException)e.InnerException).StatusCode;
                    if (errorcode == 404)
                    {
                        needConfirmation = false; // the server has already process the request
                    }
                    else
                    {
                        // could be 500 code
                        throw e;
                    }

                }
                else
                {
                    throw e;
                }

            }
        }
        private async void PickOrder()
        {
            try
            {


                if (needConfirmation == false)
                {
                    respond = Services.ServerRequest.PickOrderRequest(this.ord, this._barCodeid);
                    needConfirmation = true;

                    foreach (OrderAction orda in respond)
                    {
                        ord.OrderActions.Add(orda);
                    }

                }


                // got and send a confirm request


                ConfirmGetOrder();
                // after server successfully receive the request
                MobileApp.GetSingletion().User.Orders.Add(ord);

                //TODO write to database later
                //_ = await MobileApp.GetSingletion().LocalDatabase.SaveOrderAsync(ord);

                //push back to orderlist page
                // remove detail page,
                //https://stackoverflow.com/questions/24856116/how-to-popasync-more-than-1-page-in-xamarin-forms-navigation
                for (var counter = 1; counter < 2; counter++)
                {
                    Shell.Current.Navigation.RemovePage(Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 2]);
                }
                await Shell.Current.Navigation.PopAsync();

            }
            catch (Exception e)
            {
                if(e.InnerException is FlurlHttpTimeoutException)
                {
                    ErrorGetOrderMessage = "An network Error occurs, please retry";
                }else if(e.InnerException is FlurlHttpException)
                {
                    int errorcode = (int)((Flurl.Http.FlurlHttpException)e.InnerException).StatusCode;
                    if (errorcode == 403)
                    {
                        ErrorGetOrderMessage = "An error occurs with your account, please try to logout and re-login";
                    }
                    else if (errorcode == 404)
                    {
                        ErrorGetOrderMessage = e.Message;
                    }
                    else
                    {
                        ErrorGetOrderMessage = "Unknow network failure";
                    }
                }
                else
                {
                    ErrorGetOrderMessage = "Hello world";
                }
                
            }
        }

        public void OnScanning()
        {
            IsScanning = true;

            //ZXingScannerView_OnScan();

        }
        public void ZXingScannerView_OnScan()
        {
            if (valideBarCode(scanner.Result.Text))
            {
                // if is valid barcode id
                ErrorBarCodeMessage = "";
                _barCodeid = scanner.Result.Text;
            }
            else
            {
                ErrorBarCodeMessage = "Not valid Barcode ID";
                // not valid id
            }
        }

        private bool valideBarCode(string newBarCode)
        {
            try
            {
                int numb = Int32.Parse(newBarCode);
                if (numb == 0)
                {
                    throw new Exception("");
                }
                ErrorBarCodeMessage = "";
                return true;
            }
            catch (Exception e)
            {
                ErrorBarCodeMessage = "Not valid Barcode ID";
                return false;
            }
        }
    }
}
