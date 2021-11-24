using System;
using System.Collections.Generic;
using System.Text;
using Flurl.Http;
using ShellDemo.Models;
using Xamarin.Forms;
using ZXing;

namespace ShellDemo.ViewModels
{

    public class PickOrderViewModel : BaseViewModel
    {
        private Order ord;
        public ZXing.Net.Mobile.Forms.ZXingScannerView scanner;


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
        public int OrderID => this.ord.IDAtDatabase;

        public bool HaveBarcodeID => ord.barCode != 0;

        public bool CanGetOrder => ErrorBarCodeMessage.Length == 0;
        public Command ScanCommand { get; }
        public Command PickOrderCommand { get; }
        public PickOrderViewModel(Order ord)
        {
            this.ord = ord;
            this._barCodeid = ord.barCode.ToString(); // if no barcode being assinged, it will default be 0
            ScanCommand = new Command(OnScanning);
            PickOrderCommand = new Command(PickOrder);


        }

        private bool ConfirmGetOrder(UserSession ans, Order ord)
        {
            try
            {
                string url = MobileApp.GetSingletion().BaseUrl + "/confirm";
                var respondCode = url.WithTimeout(20).PostJsonAsync(ans).Result;
                // means user successfully Get the order
                return true;
            }
            catch (FlurlHttpTimeoutException er)
            {
                throw er;

            }
            catch (FlurlHttpException e)
            {
                int statusCode = (int)((Flurl.Http.FlurlHttpException)e).StatusCode;
                if (statusCode == 404)
                {
                    // could be the user has already got the order, or for somereason it is lost on the server

                    //check if the user already have the order
                    try
                    {
                        string url_isPicked = MobileApp.GetSingletion().BaseUrl + "/pickedUpOrder/" + ord.IDAtDatabase;
                        var res = url_isPicked.PostJsonAsync(ans);

                        // success, returns 200
                        return true;


                    }
                    catch (FlurlHttpException es)
                    {
                        //here means just not login
                        throw es;
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
                string url_getOrder = MobileApp.GetSingletion().BaseUrl + "/pickOrder";
                List<OrderAction> respond = await url_getOrder.WithTimeout(20).PostJsonAsync(new GetOrderRequest
                {
                    Session = MobileApp.GetSingletion().User.CurrentSessionUUID,
                    OrderID = this.ord.IDAtDatabase,
                    Barcode = Int32.Parse(this._barCodeid)
                }).ReceiveJson<List<OrderAction>>();


                foreach( OrderAction orda in respond)
                {
                    ord.OrderActions.Add(orda);
                }

                // got and send a confirm request
                UserSession ses = new UserSession
                {
                    Session = MobileApp.GetSingletion().User.CurrentSessionUUID
                };


                ConfirmGetOrder(ses, ord);  
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
            catch(FlurlHttpTimeoutException e)
            {
                ErrorGetOrderMessage = "An network Error occurs, please retry";
            }
            catch(FlurlHttpException e)
            {
                int errorcode = (int)((Flurl.Http.FlurlHttpException)e).StatusCode;
                if (errorcode == 403)
                {
                    ErrorGetOrderMessage = "An error occurs with your account, please try to logout and re-login";
                }else if(errorcode == 404)
                {
                    ErrorGetOrderMessage = e.Message;
                }
                else
                {
                    ErrorGetOrderMessage = "Unknow network failure";
                }
            }catch(Exception e)
            {
                ErrorGetOrderMessage = "Hello world";
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
                if(numb == 0)
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
