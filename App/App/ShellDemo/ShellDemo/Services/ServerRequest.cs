using System;
using System.Collections.Generic;
using System.Text;
using Flurl.Http;
using ShellDemo.Models;

namespace ShellDemo.Services
{
    public class ServerRequest
    {
        public static UserSession LoginRequest(string username, string password)
        {
            string url = MobileApp.GetSingletion().BaseUrl + "/login";
            UserSession ans =  url.WithTimeout(20).PostJsonAsync(new User
            { AccountName = username, Password = password }).ReceiveJson<UserSession>().Result;
            return ans;
        }
        public static void ConfirmLoginRequest(UserSession ans)
        {
            string url = MobileApp.GetSingletion().BaseUrl + "/confirm";
            var respondCode = url.WithTimeout(20).PostJsonAsync(ans).Result;
        }

        public static List<OrderAction> PickOrderRequest(Order ord, string _barCodeid)
        {
            string url_getOrder = MobileApp.GetSingletion().BaseUrl + "/pickOrder";
            List<OrderAction> respond = url_getOrder.WithTimeout(20).PostJsonAsync(new GetOrderRequest
            {
                Session = MobileApp.GetSingletion().User.CurrentSessionUUID,
                OrderID = ord.IDAtDatabase,
                Barcode = Int32.Parse(_barCodeid)
            }).ReceiveJson<List<OrderAction>>().Result;
            return respond;
        }

        public static void ConfirmPickOrderRequest()
        {   var ans =  new UserSession
            {
                Session = MobileApp.GetSingletion().User.CurrentSessionUUID
            };

            string url = MobileApp.GetSingletion().BaseUrl + "/confirm";
            var respondCode = url.WithTimeout(20).PostJsonAsync(ans).Result;
        }

        public static UpdateOrderResponse UpdateRequest()
        {
            string updateUrl = MobileApp.GetSingletion().BaseUrl + "/updateOrder";
            var respond = updateUrl.WithTimeout(20).PostJsonAsync(new UpdateOrderRequest
            {
                Session = MobileApp.GetSingletion().User.CurrentSessionUUID,
                Orders = MobileApp.GetSingletion().User.Orders
            }).ReceiveJson<UpdateOrderResponse>().Result;
            return respond;
        }

        public static IFlurlResponse ConfirmUpdateRequest()
        {
            string url = MobileApp.GetSingletion().BaseUrl + "/confirm";
            var respondCode = url.WithTimeout(50).PostJsonAsync(new UserSession
            {
                Session = MobileApp.GetSingletion().User.CurrentSessionUUID

            }).Result;
            // means the server has successfully got the message from us.
            return respondCode;
        }

        public static void LogoutRequest()
        {
            string url = MobileApp.GetSingletion().BaseUrl + "/logout";
            var respond = url.WithTimeout(10).PostJsonAsync(new UserSession
            {
                Session = MobileApp.GetSingletion().User.CurrentSessionUUID

            }).Result;
            
        }
        
    }
}
