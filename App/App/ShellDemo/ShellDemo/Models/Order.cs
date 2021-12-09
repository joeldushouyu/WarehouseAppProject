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

        /// <summary>
        /// The function check to see whether the locationID passed in is withing currentOrder's range of picking/supplying
        /// </summary>
        /// <param name="locationID"></param>
        /// <returns>boolean </returns>
        public bool WithinPickingRange(long locationID)
        {
            List<long>pickingRange =  MobileApp.GetSingletion().User.CalculatePickingRange();
            long beginngRange = pickingRange[0];
            long endingRange = pickingRange[1];

            return beginngRange <= locationID && endingRange >= locationID;
        }



        public Order()
        {
            OrderActions = new List<OrderAction>();
        }
    }
}
