using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


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
            
            return View();
        }
    }
}