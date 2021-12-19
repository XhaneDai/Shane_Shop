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
        /// <summary>
        /// 選購商品頁
        /// </summary>
        /// <returns></returns>
        public ActionResult ShoppingPage()
        {
            return View();
        }

        /// <summary>
        /// 訂單建立頁
        /// </summary>
        /// <param name="createOrder"></param>
        /// <returns></returns>
        public ActionResult CreateOrder(CreateOrder createOrder)
        {
            AdoCommand adoCommand = new AdoCommand(SqlConnectString);

            //新增訂單主檔
            var OrderID = adoCommand.InsOrders(createOrder);
            //新增訂單明細
            int createResult = adoCommand.InsOrdersDetails(OrderID, createOrder.ProductID, createOrder.Quantity);

            if (createResult > 0)
            {
                var orderInfo = adoCommand.GetOrderByOrderID<OrderInfo>(OrderID);
                if (orderInfo.Count > 0)
                {
                    return View(orderInfo[0]);
                }
                else
                {
                    return RedirectToAction("CreateOrderFail", "Base");
                }
            }
            else
            {
                return RedirectToAction("CreateOrderFail", "Base");
            }

        }

        /// <summary>
        /// 綠界回Call接收頁
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        public ActionResult OrderPayResult(FormCollection formCollection)
        {
            Dictionary<string, string> form = formCollection.AllKeys.ToDictionary(k => k, v => formCollection[v]);

            AdoCommand adoCommand = new AdoCommand(SqlConnectString);

            string OrderID = form["MerchantTradeNo"].Replace($"ShaneShop{DateTime.Now:MMdd}", ""); //把剛剛Post去綠界前的訂單編號去掉
            ViewData["OrderID"] = OrderID;

            if (form["RtnCode"] == "1")
            {
                adoCommand.UpdOrderStatusByRtnCode(OrderID, "Approved");
                ViewData["PayResult"] = "交易成功";
            }
            else
            {
                adoCommand.UpdOrderStatusByRtnCode(OrderID, "Failed");
                ViewData["PayResult"] = "交易失敗，請重新確認訂單狀態";
            }

            return View();
        }

        public ActionResult QueryOrderByOrderID()
        {
            return View();
        }

        /// <summary>
        /// 查詢訂單輸入訂單編號頁面
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public ActionResult QueryOrder(string orderID)
        {
            AdoCommand adoCommand = new AdoCommand(SqlConnectString);

            if (adoCommand.GetOrderByOrderID<OrderInfo>(orderID).Count > 0)
            {
                OrderInfo orderInfo = adoCommand.GetOrderByOrderID<OrderInfo>(orderID)[0];
                orderInfo.OrderStatus = Operation.GetOrderStatusInCH(orderInfo.OrderStatus.Trim()); //取得訂單狀態中文

                return View(orderInfo);
            }
            else
            {
                return Content("<script>alert('查無此訂單');location.href='QueryOrderByOrderID';</script>");
            }
        }

        /// <summary>
        /// 更新訂單內容
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        public ActionResult UpdateOrder(OrderInfo orderInfo)
        {
            AdoCommand adoCommand = new AdoCommand(SqlConnectString);
            int updateResult = adoCommand.UpdOrderWithOrderInfo(orderInfo.OrderID.ToString(), orderInfo.ShipCity, orderInfo.ShipAddress);

            if (updateResult != 0)
            {
                return Content($"<script>alert('更新成功');location.href='QueryOrder?OrderID={orderInfo.OrderID}';</script>");
            }
            else
            {
                return Content("<script>alert('更新失敗，請確認訂單狀態');</script>");
            }
        }

        /// <summary>
        /// 刪除訂單
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public ActionResult DeleteOrder(string OrderID)
        {
            AdoCommand adoCommand = new AdoCommand(SqlConnectString);

            bool DelOrderResult = false;

            var orderInfo = adoCommand.GetOrderByOrderID<OrderInfo>(OrderID);
            if (orderInfo.Count > 0)
            {
                if (!string.IsNullOrWhiteSpace(adoCommand.InsOrdersHistory(orderInfo[0])))
                {
                    if (adoCommand.DelOrder(OrderID) > 0)
                    {

                        DelOrderResult = true;
                    };
                }
            }

            if (DelOrderResult)
            {
                return Content("<script>alert('訂單刪除成功');location.href='QueryOrderByOrderID';</script>");
            }
            else
            {
                return Content("<script>alert('訂單刪除失敗，請重新確認訂單狀態');location.href='QueryOrderByOrderID';</script>");
            }
        }
    }
}
