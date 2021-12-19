using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShaneShop.Models.Util;

namespace ShaneShop.Controllers
{
    public class BaseController : Controller
    {
        public static string SqlConnectString = ConfigurationUtility.GetAppSetting("SqlConnectString");

        public ActionResult CreateOrderFail()
        {
            return View();
        }

    }
}