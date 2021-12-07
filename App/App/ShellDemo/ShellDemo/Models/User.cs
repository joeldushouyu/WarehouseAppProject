using System;
using System.Collections.Generic;
using System.Text;
using Flurl.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace ShellDemo.Models
{
    public class User
    {
        private int _id;
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }


        private string _accountName = "";
        [JsonProperty("accountName")]
        public string AccountName
        {
            get
            {
                return _accountName;
            }
            set
            {
                _accountName = value;
            }
        }

        private string _workingSection = "";

        [JsonProperty("section")]

        public string WorkingSection
        {
            get
            {
                return _workingSection;
            }
            set
            {
                _workingSection = value;
            }
        }



        private string _currentSessionUUID;
        [JsonProperty("session")]
        public string CurrentSessionUUID
        {
            get
            {
                return _currentSessionUUID;
            }
            set
            {
                _currentSessionUUID = value;
            }
        }


        private string _pass;
        [JsonProperty("password")]
        public string Password
        {
            set
            {
                _pass = value;
            }
            get => _pass;
        }


        private List<Order> _orders;
        public List<Order> Orders
        {
            get => _orders;
            set
            {
                _orders = value;

                
            }
        }
        private List<OrderAction> _sortedOrderActions;
        public List<OrderAction> SortedOrderActions {

            get => _sortedOrderActions;
            set => _sortedOrderActions = value;
        }


        public void logoutUser()
        {
            //reset data
            this.CurrentSessionUUID = "";
            this.AccountName = "";
            this.WorkingSection = "AA-AA";
            this.Password = "";
            this.Orders = new List<Order>();
        }
        /// <summary>
        /// The function determine if the current user is logout by checking User.CurrentSessionUUID is empty or not
        /// </summary>
        /// <returns> bool to indicate true or false</returns>
        public bool IsLogout()
        {
            return this.CurrentSessionUUID == "";
        }

        /// <summary>
        /// This function will sorted all OrderActions in each of the Order in User.Orders, and assign to User.SortedOrderActions
        /// </summary>
        public void SortOrderActions()
        {
            _sortedOrderActions.Clear();
            // get current user's picking range
            List<int> pickingRange = CalculatePickingRange();
            int beginRange = pickingRange[0];
            int endRange = pickingRange[1];
            // first get all orderActions in the _orders that are within user's picking range

            foreach(Order pickedOrder in _orders)
            {
                foreach(OrderAction ordAct in pickedOrder.OrderActions)
                {
                    if(ordAct.LocationId >= beginRange && ordAct.LocationId <= endRange)
                    {
                        this.SortedOrderActions.Add(ordAct); // add this Orderaction into range.
                  

                    }
                }
            }

            this.SortedOrderActions.Sort((OrderAction ord1, OrderAction ord2) => { return ord1.CompareTo(ord2); });

            // now check to see if whether or not user start picking from location AA0 which is locationID 1 in database
            // If does, the app will first make user pick up all the items need later on for each Supply action
            if (beginRange == 1)
            {
                foreach (Order pickedOrder in _orders)
                {
                    foreach (OrderAction ordAct in pickedOrder.OrderActions)
                    {
                        if (ordAct.LocationId >= beginRange && ordAct.LocationId <= endRange && ordAct.Action=="Supply")
                        {
                            this.SortedOrderActions.Insert(0,ordAct); // add this Orderaction into the list
                            ordAct.Initialpick = false;
                        }
                        else
                        {
                            ordAct.Initialpick = true;
                        }
                    }
                }
            }
            else
            {
                ; // do nothing
            }

            //now sort them base on the locationID in each orderAction
           

        }

        /// <summary>
        /// This function calculate and convert user's current Picking range in User.WorkingSection
        /// </summary>
        /// <returns> A List<int>, where [0] represent beginning picking range, and [1] represent the ending picking range </int></inheritdoc>/></returns>
        public List<int> CalculatePickingRange()
        {
            List<string> sectionHeaders =new List<string> {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U",
                                                             "V", "W", "X", "Y", "Z" };

            List<string> sects = new List<string>(this.WorkingSection.Split('-'));

            string beginSection = sects[0];
            string endSection = sects[1];

            int beginningRange = sectionHeaders.IndexOf(beginSection[0].ToString()) * 260 + sectionHeaders.IndexOf(beginSection[1].ToString()) * 10  + 1;

            int endRange = sectionHeaders.IndexOf(endSection[0].ToString()) * 260 + sectionHeaders.IndexOf(endSection[1].ToString()) * 10 + 1 + 9;


            List<int> returnList = new List<int>();
            returnList.Add(beginningRange);
            returnList.Add(endRange);

            return returnList;
        }
        
        public User()
        {
            _currentSessionUUID = ""; // inidication that is not logined
            // default working section
            _workingSection = "AA-AA";
            _orders = new List<Order>();
            _sortedOrderActions = new List<OrderAction>();
           
        }

      
        
    }
}
