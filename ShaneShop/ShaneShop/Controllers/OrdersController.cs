﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using ShaneShop.Models.Util;

namespace ShaneShop.Controllers
{
    public class OrdersController : BaseController
    {
        // GET: Orders
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateOrders()
        {
            AdoCommand adoCommand = new AdoCommand(SqlConnectString);
            string OrderID = "10418";
            var testConnect = adoCommand.GetOrderByOrderID(OrderID);

            return View();
        }
    }
}