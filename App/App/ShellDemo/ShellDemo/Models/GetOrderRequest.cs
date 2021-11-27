using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShellDemo.Models
{
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
        public int Barcode
        {
            set;
            get;
        }
    }
}
