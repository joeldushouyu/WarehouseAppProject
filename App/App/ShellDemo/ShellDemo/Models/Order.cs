using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SQLite;

namespace ShellDemo.Models
{
    public class Order
    {
        [PrimaryKey, AutoIncrement]
        
        public int ID { get; set; }

        [JsonProperty("id")]
        public int IDAtDatabase { get; set; }

        [JsonProperty("barcodeId")]
        public int barCode { get; set; }

        [JsonProperty("boxSize")]
        public string BoxSize { get; set; }

        [JsonProperty("orderDate")]
        public DateTime OrderDate { get; set; }

        [JsonProperty("errorOccurred")]
        public bool ErrorOccurred { get; set; }

        [JsonProperty("locked")]
        public bool Locked { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [Ignore]
        public List<OrderAction> OrderActions { get; set; }

        [JsonProperty("nextPrderActionIndex")]
        public int NextOrderActionIndex { get; set; }  //Indicate which OrderAction has not been complete
                                                       // should be -1 if has complete all
        
        public int currentOrderActionIndex()
        {
            for(int i = 0; i < OrderActions.Count; i++)
            {
                var ordAct = OrderActions[i] as OrderAction;
                if( in current section, and has not complete)
                {
                    return i;
                }

            }
            return -1; // indicate that 
        }
        public Order()
        {
            OrderActions = new List<OrderAction>();
        }
    }
}
