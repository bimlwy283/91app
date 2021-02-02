using Backend.Infrastructure;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Backend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OrderController : ApiController
    {
        [HttpGet]
        public List<Order> listOrders()
        {
            GetOrderDataFunc getOrderDataFunc = new GetOrderDataFunc();
            return getOrderDataFunc.listOrders();
        }

        [HttpPut]
        public string changeStatus(List<string> orderList)
        {
            GetOrderDataFunc getOrderDataFunc = new GetOrderDataFunc();
            return getOrderDataFunc.changeStatus(orderList);
        }

        [HttpGet]
        public List<Item> listItems([FromUri]string orderId)
        {
            GetOrderDataFunc getOrderDataFunc = new GetOrderDataFunc();
            return getOrderDataFunc.listItems(orderId);
        }
    }
}
