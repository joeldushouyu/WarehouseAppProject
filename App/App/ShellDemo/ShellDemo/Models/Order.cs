using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SQLite;
using Flurl;
namespace ShellDemo.Models
{
    public class Order
    {   
        [JsonIgnore]
        [PrimaryKey, AutoIncrement]
        public long ID { get; set; }

        [JsonProperty("id")]
        public long IDAtDatabase { get; set; }

        [JsonProperty("barcodeID")]
        public long BarCode { get; set; }

        [JsonProperty("boxSize")]
        public string BoxSize { get; set; }


        
        private DateTime _orderDate;

        [JsonProperty("orderDate")]

        public DateTime OrderDate {
            get;
            set; }

 
        public string orderDateInString => OrderDate.ToString();
        

        [JsonProperty("errorOccurred")]
        public bool ErrorOccurred { get; set; }

        [JsonProperty("locked")]
        public bool Locked { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [Ignore]
        public List<OrderAction> OrderActions { get; set; }

        /*
        public int currentOrderActionIndex()
        {
            for(int i = 0; i < OrderActions.Count; i++)
            {   
                //iterate over each orderActions in this Order
                var ordAct = OrderActions[i] as OrderAction;
                if( withinPickingRange(ordAct.LocationId) && ordAct.Completed == false)
                {   
                   
                    return i;  // return the index of this orderAction.
                }

            }
            return -1; // indicate this order is complete so far base on user's picking section and status.
        }*/

        
        public bool withinPickingRange(int locationID)
        {
            List<int>pickingRange =  MobileApp.GetSingletion().User.CalculatePickingRange();
            int beginngRange = pickingRange[0];
            int endingRange = pickingRange[1];

            return beginngRange <= locationID && endingRange >= locationID;
        }



        public Order()
        {
            OrderActions = new List<OrderAction>();
        }
    }
}
