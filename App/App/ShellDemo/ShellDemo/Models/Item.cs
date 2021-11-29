using System;
using Newtonsoft.Json;

namespace ShellDemo.Models
{
    public class Item
    {   
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("itemBarcode")]
        public string ItemBarcode { get; set; }

        [JsonProperty("image")]
        public byte[] Image { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("LocationID")]
        public long LocationID { get; set; }

        [JsonProperty("notificationDate")]
        public DateTime NotificationDate { get; set; }

        [JsonProperty("notificationType")]
        public string NotificationType { get; set; }
    }
}