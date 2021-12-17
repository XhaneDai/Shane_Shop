using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using ShaneShop.Models.Util;
using ShaneShop.Models.Orders;

namespace ShaneShop.Controllers
{
    public class OrdersController : BaseController
    {

        public ActionResult ShoppingPage()
        {

            return View();
        }


        [HttpPost]
        public ActionResult Index()
        {


            return View();
        }

        public ActionResult CreateOrders(CreateOrder createOrder)
        {
            AdoCommand adoCommand = new AdoCommand(SqlConnectString);

            //新增訂單主檔
            var OrderID = adoCommand.InsOrders(createOrder);

            //新增訂單明細
            var createResult = adoCommand.InsOrdersDetails(OrderID, createOrder.ProductID, createOrder.Quantity);


            return View();
        }

    }
}
