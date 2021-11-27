using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ShellDemo.Models
{
    public class UpdateOrderRequest
    {
        private string _session;
        private List<Order> _orders;
        
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

        [JsonProperty("OrderList")]
        public List<Order> Orders
        {
            get => _orders;
            set => _orders = value;
        }
    }
}
