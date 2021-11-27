using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Newtonsoft.Json;

namespace ShellDemo.Models
{
    public class OrderAction:IComparable<OrderAction>
    {
        [JsonIgnore]
        [PrimaryKey, AutoIncrement]
        
        public long ID { get; set; }

        [JsonProperty("id")]
        public long IDAtDatabase { get; set; }

        [JsonProperty("fromOrderID")]
        public long FromOrderId { get; set; }

        [JsonProperty("action")]
        public string Action { get; set;}

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("itemBarcode")]
        public long ItemBarcode { get; set; }

        [JsonProperty("locationId")]
        public int LocationId { get; set; }

        [JsonProperty("status")]
        public bool Completed { get; set; }

        [Ignore]
        [JsonIgnore]
        public bool Initialpick { get; set; }  // This variable use for indication in picking process.
                                               // When user start picking from location AA0, this app will first direct
                                               // user to pick up items for each supply action later on

        public int Compare(OrderAction x, OrderAction y)
        {
            if(x.LocationId < y.LocationId)
            {
                return -1;
            }else if(x.LocationId == y.LocationId)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public int CompareTo(OrderAction other)
        {
            if(this.LocationId < other.LocationId)
            {
                return -1;
            }else if(this.LocationId == other.LocationId)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public string WorkingSection()
        {
            int counter = 0;
            string [] locationSymbol = new string []{"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U",
              "V", "W", "X", "Y", "Z"};
            for (int i = 0; i < 26; i++)
            {
                for(int k = 0; k < 26; k++)
                {
                    for(int f = 0; f< 10; f++)
                    {
                        counter++;
                        if(counter == this.LocationId)
                        {
                            return locationSymbol[i] + locationSymbol[k] + f.ToString();
                        }
                    }
                }
            }
            return "Error";
        }
    }


}
