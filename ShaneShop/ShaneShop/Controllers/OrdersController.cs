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



        public ActionResult CreateOrders(CreateOrder createOrder)
        {
            AdoCommand adoCommand = new AdoCommand(SqlConnectString);

            //新增訂單主檔
            var OrderID = adoCommand.InsOrders(createOrder);
            //新增訂單明細
            int createResult = adoCommand.InsOrdersDetails(OrderID, createOrder.ProductID, createOrder.Quantity);

            if (createResult != 0)
            {
                var orderInfo = adoCommand.GetOrderByOrderID<OrderInfo>(OrderID);
                if (orderInfo.Count > 0)
                {
                    return View(orderInfo[0]);
                }
                else
                {
                    return RedirectToAction("CreateFail", "Base");
                }
            }
            else
            {
                return RedirectToAction("CreateFail", "Base");
            }

        }

    }
}
