using ShellDemo.Services;
using ShellDemo.Views;
using ShellDemo.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShellDemo
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            // User might still have datas not update with server since last time.


            // first start and initalize singleton class
            MobileApp.GetSingletion();

            try
            {
                string uuid = Application.Current.Properties["UserUUID"] as string;

                List<Order> orders = await MobileApp.GetSingletion().LocalDatabase.LoadOrderListWithOrderActionAsync();

                if(orders.Count != 0)
                {
   
                    // means still have data left, it was not able to update and synchronize with server in time
                    //load it into the list
                   MobileApp.GetSingletion().User.Orders = orders;
                      await Task.Run(async () =>
                   {
                       _ = await Services.ServerRequest.UpdateRequest(uuid, orders); //has to wait for this result
                        _ = await Services.ServerRequest.ConfirmUpdateRequest(uuid);
                   });
                }
                else
                {
                    ;
                }

            }
            catch(Exception e)
            {
                // nothing we can do, if throw exception
                ;
            }
            finally
            {
                // clear out the data, ready for new user
                _ = MobileApp.GetSingletion().LocalDatabase.ClearOrderActionAsync();
                _ = MobileApp.GetSingletion().LocalDatabase.ClearOrderAsync();
                MobileApp.GetSingletion().User.Orders = new List<Order>();
                MobileApp.GetSingletion().User.CurrentSessionUUID = "";
            }

        }

        protected override void OnSleep()
        {
            //Here try to update the server will what it have in the order, since it might got kill within any time.
            // ignore all exceptions? good practice?
            // try to update the server with its current information, start a extend API call
            DependencyService.Get<IExtendBackgroundThread>().ExtendBackGroundThreadTime(async () => {
                try
                {
                    Application.Current.Properties["UserUUID"] = MobileApp.GetSingletion().User.CurrentSessionUUID;
                    _= Services.ServerRequest.UpdateRequest(MobileApp.GetSingletion().User.CurrentSessionUUID, MobileApp.GetSingletion().User.Orders).Result;  //has to wait for this request
                    _ = Services.ServerRequest.ConfirmUpdateRequest(MobileApp.GetSingletion().User.CurrentSessionUUID);

                    // only will come to here if does not throw exception during updating with server
                    // clear out the database
                    MobileApp.GetSingletion().User.Orders = new List<Order>();
                    _= await MobileApp.GetSingletion().LocalDatabase.ClearOrderActionAsync();
                    _= await MobileApp.GetSingletion().LocalDatabase.ClearOrderAsync();
                }catch(Exception e)
                {
                    // nothing we can do this point;
                    ;
                }

            });
        }

        protected override void OnResume()
        {
        }
    }
}
