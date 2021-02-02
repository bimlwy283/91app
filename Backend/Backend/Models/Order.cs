using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.Models
{
    public class Order
    {
        public int id { get; set; }
        public string orderId { get; set; }
        public List<Item> itemList { get; set; }
        public int price { get; set; }
        public int cost { get; set; }
        public string status { get; set; }
    }

    public class Item
    {
        public int id { get; set; }
        public string itemName { get; set; }
        public int price { get; set; }
        public int cost { get; set; }
        public int num { get; set; }
    }
}