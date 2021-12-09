using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using ShellDemo.Models;


namespace ShellDemo.Services
{
    public class ServerRequest
    {
        public static string BaseUrl = "http://10.0.2.2:5000";
        /// <summary>
        /// The function sends a request to sever for login. It throws out Exception with customized message correspond to different error message.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns> UserSession with information from the server</returns>
        public static async Task<UserSession> LoginRequest(string username, string password)
        {
            try
            {
                string url = BaseUrl + "/login";
                UserSession ans =  await url.WithTimeout(20).PostJsonAsync(new User
                { AccountName = username, Password = password }).ReceiveJson<UserSession>();
                return ans;
            }
            catch (FlurlHttpTimeoutException)
            {
                throw new Exception("An network Error occurs, please retry");
            }
            catch (FlurlHttpException e)
            {
                if(((Flurl.Http.FlurlHttpException)e).StatusCode == null)
                {
                    // occurs when no internet connection, like in case of airplane mode
                    throw new Exception("No wifi connection");
                }
                int errorcode = (int)((Flurl.Http.FlurlHttpException)e).StatusCode;
                if (errorcode == 403)
                {
                    // means first time login, but got 403, account alredy login in on other device
                    throw new Exception("Incorrect username or password");
                }
                else if (errorcode == 405)
                {
                    // it has been timeout, 
                    throw new Exception("You account is logined in on another device already!");
                    //TODO: what if connection lost on the way back?
                }
                else
                {
                    // server error
                    throw new Exception( "An error occurs with the server, please retry");
                }
            }
            catch (Exception)
            {
                throw new Exception("An error occurs , please retry");
            }

        }

        /// <summary>
        /// The function sends a confirm login request to the server.
        /// It throws out Exception with customized message correspond to different error message.
        /// </summary>
        /// <param name="ans"> </param>
        /// <returns>true only when successfull login.</returns>
        public static async Task<bool> ConfirmLoginRequest(UserSession ans)
        {
            try
            {
                string url =BaseUrl + "/confirm";
                var respondCode = await url.WithTimeout(20).PostJsonAsync(ans);
                return true;
            }catch(FlurlHttpTimeoutException)
            {
                throw new Exception("An network error has occurred, please retry"); 
            }catch(FlurlHttpException e)
            {
                if (((Flurl.Http.FlurlHttpException)e).StatusCode == null)
                {
                    // occurs when no internet connection, like in case of airplane mode
                    throw new Exception("No wifi connection");
                }
                int errorcode = (int)((Flurl.Http.FlurlHttpException)e).StatusCode;
                if (errorcode == 404)
                {
                    throw new Exception("Invalid user id, please try reopen the app");
                }
                else
                {
                    // could be 500 code
                    throw new Exception("unknow error has occurred");
                }
            }

        }

        /// <summary>
        /// It sends an request to the server to pick up a Order from the server
        /// It throws out Exception with customized message correspond to different error message.
        /// </summary>
        /// <param name="ord">  The order that trying to pickup </param>
        /// <param name="_barCodeid"> The assigned barcode number to this specific order  </param>
        /// <param name="userUUID"> User's current active UUID receive from login </param>
        /// <returns> List< OrderAction> correspond to this order from the server</returns>
        public static async Task<List<OrderAction>> PickOrderRequest(Order ord, string barCodeid, string userUUID)
        {
            try
            {
                string url_getOrder = BaseUrl + "/pickOrder";
                List<OrderAction> respond = await url_getOrder.WithTimeout(20).PostJsonAsync(new GetOrderRequest
                {
                    Session = userUUID,
                    OrderID = ord.IDAtDatabase,
                    Barcode = long.Parse(barCodeid)
                }).ReceiveJson<List<OrderAction>>();
                return respond;
            }
            catch (FlurlHttpTimeoutException)
            {
                throw new Exception("An network Error occurs, please retry");
            }catch(FlurlHttpException e)
            {
                if (((Flurl.Http.FlurlHttpException)e).StatusCode == null)
                {
                    // occurs when no internet connection, like in case of airplane mode
                    throw new Exception("No wifi connection");
                }
                int errorcode = (int)((Flurl.Http.FlurlHttpException)e).StatusCode;
                if (errorcode == 403)
                {
                    throw new Exception("An error occurs with your account, please try to logout and re-login");
                }
                else if (errorcode == 404)
                {
                    throw new Exception("The order has already been picked up by other users");
                }
                else
                {
                    throw new Exception("Unknow network failure");
                }
            }

        }

        /// <summary>
        /// the function sends a confirm pick up order request to the server
        /// It throws out Exception with customized message correspond to different error message.
        /// </summary>
        /// <param name="userUUID"> current active UUID from login</param>
        /// <returns>Return true if success receive by the server, else raise exception </returns>
        public static async Task<bool> ConfirmPickOrderRequest(string userUUID)
        {
            try
            {
                var ans =  new UserSession
                {
                    Session = userUUID
                };

                string url = BaseUrl + "/confirm";
                var respondCode = await url.WithTimeout(20).PostJsonAsync(ans);
                return true;
            }
            catch (FlurlHttpTimeoutException)
            {
                throw new Exception("unstable network exception, please retry");
            }catch(FlurlHttpException e)
            {
                if (((Flurl.Http.FlurlHttpException)e).StatusCode == null)
                {
                    // occurs when no internet connection, like in case of airplane mode
                    throw new Exception("No wifi connection");
                }
                int errorcode = (int)((Flurl.Http.FlurlHttpException)e).StatusCode;
                if (errorcode == 404)
                {
                    throw new Exception("Invalid user id, please try reopen the app");
                }
                else
                {
                    // could be 500 code
                    throw new Exception("unknown error has occurred");
                }
            }

        }

        /// <summary>
        /// The function send an update request to the server, to update the server with current Order
        /// It throws out Exception with customized message correspond to different error message.
        /// </summary>
        /// <param name="UserSession"> current active user UUID from the login</param>
        /// <param name="orders"> Orders want to update to the server </param>
        /// <returns>UpdateOrderResponse contains the response information of the server</returns>
        public static async Task<UpdateOrderResponse> UpdateRequest(string UserSession, List<Order>orders)
        {
            try
            {
                string updateUrl = BaseUrl + "/updateOrder";
                var respond =await  updateUrl.WithTimeout(20).PostJsonAsync(new UpdateOrderRequest
                {
                    Session = UserSession,
                    Orders = orders
                }).ReceiveJson<UpdateOrderResponse>();
                return respond;
            }
            catch (FlurlHttpTimeoutException)
            {
                throw new Exception("An network error occurs, please retry");
            }
            catch(FlurlHttpException e)
            {
                if (((Flurl.Http.FlurlHttpException)e).StatusCode == null)
                {
                    // occurs when no internet connection, like in case of airplane mode
                    throw new Exception("No wifi connection");
                }
                int errorcode = (int)((Flurl.Http.FlurlHttpException)e).StatusCode;
                if (errorcode == 404)
                {

                    // means incorrect format
                    //should not happen,
                    //but if does, report 
                     throw new Exception("An undesire error happens, please retry");
                }
                else if (errorcode == 403)
                {
                    // incorrect user UUID, 
                    //Nothing can do except let the user re-login
                    throw new Exception("Please try relogin with your account");
                }
                else
                {
                    // mayeb 500
                    throw new Exception("An undesire error happens, please retry");
                }
            }


        }


        /// <summary>
        /// Send a request to the server to confirm UpdateOrder request
        /// It throws out Exception with customized message correspond to different error message.
        /// </summary>
        /// <param name="UserUUID"> Current Active user UUID receive from login</param>
        /// <returns> true if successfully login</returns>
        public static async Task<bool> ConfirmUpdateRequest(string UserUUID)
        {
            try
            {
                string url = BaseUrl + "/confirm";
                await url.WithTimeout(50).PostJsonAsync(new UserSession
                {
                    Session = UserUUID

                });
                return true;
                // means the server has successfully got the message from us.
            }
            catch (FlurlHttpTimeoutException)
            {
                throw new Exception("An network error occurs");
            }
            catch (FlurlHttpException e)
            {
                if (((Flurl.Http.FlurlHttpException)e).StatusCode == null)
                {
                    // occurs when no internet connection, like in case of airplane mode
                    throw new Exception("No wifi connection");
                }
                int errorcode = (int)((Flurl.Http.FlurlHttpException)e).StatusCode;
                if (errorcode == 404)
                {
                    throw new Exception("Invalid user id, please try reopen the app");
                }
                else
                {
                    // could be 500 code
                    throw new Exception("unknown error occurs to server");
                }
            }

         
        }

        /// <summary>
        /// Send a request to server to logout this user correspond to the user UUID
        /// </summary>
        /// <param name="UserUUID">Current Active user UUID from login</param>
        /// <returns></returns>
        public static async Task LogoutRequest( string UserUUID)
        {
            try
            {
                string url = BaseUrl + "/logout";
                await url.WithTimeout(15).PostJsonAsync(new UserSession
                {
                    Session = UserUUID

                });
            }
            catch (Exception)
            {
                throw new Exception("Fail to logout, please retry by click login button");
                // no matter what error it is, since it is indempotent, so safe to retry anyway
            }
            
            
        }

        /// <summary>
        /// Send request to server to get detail information about this item
        /// It throws out Exception with customized message correspond to different error message.
        /// </summary>
        /// <param name="locationID"> LocationID correspond to thsi Item </param>
        /// <param name="UserUUID"> current Active user UUID received from login</param>
        /// <returns></returns>
        public static async Task<Item> LoadItemRequest(long locationID, string UserUUID)
        {
            try
            {
                string url = BaseUrl + "/itemDetail/" + locationID.ToString();
                var respond = await url.WithTimeout(10).PostJsonAsync(new UserSession { Session = UserUUID }).ReceiveJson<Item>();
                return respond;
            }
            catch (FlurlHttpTimeoutException)
            {
                throw new Exception("unknow Network error, please retry");
            }
            catch (FlurlHttpException e)
            {
                if (((Flurl.Http.FlurlHttpException)e).StatusCode == null)
                {
                    // occurs when no internet connection, like in case of airplane mode
                    throw new Exception("No wifi connection");
                }
                int errorcode = (int)((Flurl.Http.FlurlHttpException)e).StatusCode;
                if (errorcode == 403)
                {
                   throw new Exception("An error occurs with your credentials, please try to relogin");
                }
                else
                {
                    // means unknow error 
                    throw new Exception("Unknow error occurs, please retry by went back to the previous page");
                }
            }

        }

        /// <summary>
        /// Send request to server to get a list of Orders that is within user's picking range
        /// It throws out Exception with customized message correspond to different error message.
        /// </summary>
        /// <param name="user"> current logined user</param>
        /// <returns></returns>
        public static async Task<List<Order>> LoadOrderRequest( User user)
        {
            try
            {


                string url = BaseUrl + "/orderList"; 
                var act =  await url.WithTimeout(20).PostJsonAsync(user);
                return await act.GetJsonAsync<List<Order>>();
            }
            catch(FlurlHttpTimeoutException e)
            {
                throw new Exception(" The network is unstable, please try to refresh the page");
            }catch(FlurlHttpException e)
            {
                if (((Flurl.Http.FlurlHttpException)e).StatusCode == null)
                {
                    // occurs when no internet connection, like in case of airplane mode
                    throw new Exception("No wifi connection");
                }
                int statusCode = (int)((Flurl.Http.FlurlHttpException)e).StatusCode;

                if (statusCode == 403)
                {
                    // means for some reason, the user was logged out
                    // let the user retry it

                    throw new Exception("You are no longer logined, please try to login again!");
                }
                else if (statusCode == 500)
                {
                    throw new Exception("an error occured on the server, please contact your manager");
                }
                else
                {
                    // 404 error code or potentially be other unsure error
                    throw new Exception("Unknow error has occurred, please retry");
                }
            }
    
        }
        /// <summary>
        /// Send a request to server to load all Items in the warehouse
        /// </summary>
        /// <param name="UserUUID"> current Active user UUID receive from login</param>
        /// <returns></returns>
        public static async  Task<List<Item>> LoadItemListRequest(string UserUUID)
        {   
            try
            {
                string url = BaseUrl + "/itemList";
                var act =   await url.WithTimeout(30).AllowAnyHttpStatus().PostJsonAsync(new UserSession { Session = UserUUID }).ReceiveJson<List<Item>>();

                return act;


         
            }
            catch (FlurlHttpTimeoutException)
            {
                throw new Exception("An internet error occurs, please try to refresth the page");
            }
            catch (FlurlHttpException e)
            {
                if (((Flurl.Http.FlurlHttpException)e).StatusCode == null)
                {
                    // occurs when no internet connection, like in case of airplane mode
                    throw new Exception("No wifi connection");
                }
                int errorcode = (int)((Flurl.Http.FlurlHttpException)e).StatusCode;
                if (errorcode == 403)
                {
                    throw new Exception ("Invalid cridential, please try to relogin");
                }
                else
                {
                    throw new Exception("Unknow network error occurs, please retry");
                }
            }
            
   
            
            
        }
    }
}
