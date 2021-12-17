using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShaneShop.Models.Util;


namespace ShaneShop.Models.Orders
{
    public class DoPaymentController : Controller
    {
        public ActionResult DoECPay(OrderInfo orderInfo)
        {

            var ReturnURL = Url.Action("SuccessPage", "Sponsor", null, "https");
            // 客戶點返回的時候的 連結
            var ClientBackURL = Url.Action("Index", "Sponsor", null, "https");
            // 付款完成的返回連結, 建議先空白
            var OrderResultURL = Url.Action("SuccessPage", "Sponsor", null, "https");
            ////// 綠界Server 付款方式確認時 Call 的網址, 會給付款方式的資訊
            ////var PaymentInfoURL = Url.Action("Index", "Sponsor", null, "https"); ;

            var postData = new Dictionary<string, string>();

            var EcpayHashKey = ConfigurationUtility.GetAppSetting("EcpayHashKey");
            var EcpayHashIV = ConfigurationUtility.GetAppSetting("EcpayHashIV");

            postData.Add("MerchantID", ConfigurationUtility.GetAppSetting("MerchantID"));

            postData.Add("ReturnURL", ReturnURL); //付款完成通知回傳的網址
            postData.Add("ClientBackURL", ClientBackURL); //瀏覽器端返回的廠商網址
            postData.Add("OrderResultURL", OrderResultURL); //瀏覽器端回傳付款結果網址

            ////postData.Add("PaymentInfoURL", PaymentInfoURL); //瀏覽器端回傳付款結果網址

            //postData.Add("MerchantTradeNo", DateTimeOffset.Now.ToOffset(new TimeSpan(8, 0, 0)).ToUnixTimeSeconds().ToString()); // new Random().Next(0, 99).ToString());//廠商的交易編號);
            postData.Add("MerchantTradeNo", $"ShaneShop{orderInfo.OrderID}"); //這邊有點偷吃步，因為NorthWind的訂單編號格式太容易重複，所以加上ShaneShop與他人區隔;

            postData.Add("MerchantTradeDate", orderInfo.OrderDate.ToString("yyyy/MM/dd HH:mm:ss"));

            postData.Add("TotalAmount", Convert.ToString(Convert.ToInt32(orderInfo.UnitPrice) * orderInfo.Quantity));

            postData.Add("TradeDesc", "Shane'sShop"); //交易描述

            postData.Add("ItemName", orderInfo.ProductName); // 名稱

            postData.Add("PaymentType", "aio"); //使用的付款方式

            postData.Add("ChoosePayment", "ALL"); //使用的付款方式

            postData.Add("NeedExtraPaidInfo", "N");  //是否需要額外的付款資訊

            string _CheckMacValue = Operation.GetCheckMacValue(postData, EcpayHashKey, EcpayHashIV); //組檢查碼

            postData.Add("CheckMacValue", _CheckMacValue); //檢查碼

            ViewBag.PostURL = ConfigurationUtility.GetAppSetting("ECPayUrl"); //綠界目標網址
            ViewBag.postData = postData;
            return View();
        }


    }
}