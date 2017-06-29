using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace collectBooksManageList.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult BookList()
        {
            return View();
        }
        public ActionResult Registered()
        {
            return View();
        }
        public ActionResult EditBook()
        {
            return View();
        }
    }
}