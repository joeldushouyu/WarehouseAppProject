using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShellDemo.Models
{   
    // This class is used in receiving respond from server when user request to Pick up a order
    public class GetOrderRequest
    {
        private string _session;

        [JsonProperty("session")]
        public string Session
        {
            get
            {
                return _session;
            }
            set
            {
                _session = value;
            }
        }
        private long _orderID;
        [JsonProperty("orderID")]
        public long OrderID
        {
            get
            {
                return _orderID;
            }
            set
            {
                _orderID = value;
            }
        }

        [JsonProperty("assignedBarcode")]
        public long Barcode
        {
            set;
            get;
        }
    }
}
