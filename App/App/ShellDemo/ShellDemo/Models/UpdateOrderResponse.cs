using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShellDemo.Models
{
    public class UpdateOrderResponse
    {
        private List<long> _errorOrderID;

        [JsonProperty("ErrorOrderID")]
        public List<long> ErrorOrderID
        {
            get
            {
                return _errorOrderID;
            }
            set
            {
                _errorOrderID = value;
            }
        }

        public UpdateOrderResponse()
        {
            this.ErrorOrderID = new List<long>();
        }

    }
}
