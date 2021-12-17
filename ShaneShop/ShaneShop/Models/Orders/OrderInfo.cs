using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShaneShop.Models.Orders
{
    public class OrderInfo
    {
        /// <summary>
        /// 訂單編號
        /// </summary>
        public int? OrderID { get; set; }

        /// <summary>
        /// 顧客ID
        /// </summary>
        public string CustomerID { get; set; }

        /// <summary>
        /// 訂單成立時間
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 寄送地址
        /// </summary>
        public string ShipAddress { get; set; }

        /// <summary>
        /// 寄送城市
        /// </summary>
        public string ShipCity { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品單價
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 訂購數量
        /// </summary>
        public int Quantity { get; set; }


        /// <summary>
        /// 訂單狀態
        /// </summary>
        public string OrderStatus { get; set; }
    }
}