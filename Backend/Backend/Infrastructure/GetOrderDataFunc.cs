using Backend.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Backend.Infrastructure
{
    public class GetOrderDataFunc
    {
        public List<Order> listOrders()
        {
            List<Order> orderList = getOrders();
            Dictionary<string, List<Item>> itemDict = getItems();
            foreach (Order order in orderList)
            {
                order.itemList = itemDict[order.orderId];
            }
            return orderList;
        }

        public List<Order> getOrders()
        {
            List<Order> orderList = new List<Order>();
            string sqlStr = "SELECT o.id, o.orderId, o.price, o.cost, s.status FROM [Order] as o left join [Status] as s ON o.statusId = s.id";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TYBridgeConnectionString"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Order order = new Order();
                    order.id = int.Parse(dr["id"].ToString());
                    order.orderId = dr["orderId"].ToString();
                    order.price = int.Parse(dr["price"].ToString());
                    order.cost = int.Parse(dr["cost"].ToString());
                    order.status = dr["status"].ToString();
                    orderList.Add(order);
                }
                dr.Close(); dr.Dispose();
            }
            return orderList;
        }

        public Dictionary<string, List<Item>> getItems()
        {
            Dictionary<string, List<Item>> keyValuePairs = new Dictionary<string, List<Item>>();
            List<Item> itemList = new List<Item>();
            string sqlStr = "SELECT oi.orderId, oi.num, i.id, i.itemName, i.price, i.cost FROM [Order_Item] as oi left join [Item] as i ON oi.itemId=i.id";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TYBridgeConnectionString"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Item item = new Item();
                    item.id = int.Parse(dr["id"].ToString());
                    item.itemName = dr["itemName"].ToString();
                    item.price = int.Parse(dr["price"].ToString());
                    item.cost = int.Parse(dr["cost"].ToString());
                    string orderId = dr["orderId"].ToString();
                    if (!keyValuePairs.ContainsKey(orderId))
                    {
                        keyValuePairs.Add(orderId, new List<Item>());
                    }
                    keyValuePairs[orderId].Add(item);
                }
                dr.Close(); dr.Dispose();
            }
            return keyValuePairs;
        }

        public string changeStatus(List<string> orderList)
        {
            //string sqlStr = "UPDATE [Order] SET statusId=3 WHERE orderId in (@orderId)";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TYBridgeConnectionString"].ConnectionString))
            {
                string sqlStr = "UPDATE [Order] SET statusId=3 WHERE orderId in ( " + String.Join(",", orderList.Select(order => "'" + order + "'")) + " )";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                conn.Open();
                //cmd.Parameters.AddWithValue("@orderId", String.Join(",", orderList.Select(order => "'" + order + "'")));
                cmd.ExecuteNonQuery();
            }
            return "Change Completed";
        }

        public List<Item> listItems(string orderId)
        {
            List<Item> itemList = new List<Item>();
            string sqlStr = "SELECT oi.orderId, oi.num, i.id, i.itemName, i.price, i.cost FROM [Order_Item] as oi left join [Item] as i ON oi.itemId=i.id where oi.orderId=@orderId";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TYBridgeConnectionString"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                conn.Open();
                cmd.Parameters.AddWithValue("@orderId", orderId);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Item item = new Item();
                    item.id = int.Parse(dr["id"].ToString());
                    item.itemName = dr["itemName"].ToString();
                    item.price = int.Parse(dr["price"].ToString());
                    item.cost = int.Parse(dr["cost"].ToString());
                    item.num = int.Parse(dr["num"].ToString());
                    itemList.Add(item);
                }
            }
            return itemList;
        }
    }
}