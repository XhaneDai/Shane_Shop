using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShaneShop.Models.Orders
{
    public class CreateOrder
    {
        /// <summary>
        /// 顧客ID
        /// </summary>
        public string CustomerID { get; set; }

        /// <summary>
        /// 寄送城市
        /// </summary>
        public string ShipCity { get; set; }

        /// <summary>
        /// 寄送地址
        /// </summary>
        public string ShipAddress { get; set; }

        /// <summary>
        /// 訂購商品ID
        /// </summary>
        public string ProductID { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public string Quantity { get; set; }
    }
}