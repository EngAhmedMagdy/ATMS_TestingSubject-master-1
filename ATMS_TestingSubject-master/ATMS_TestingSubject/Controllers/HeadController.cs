using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ATMS_TestingSubject.Controllers
{
    public class HeadController : Controller
    {
        // GET: Head
        // home dashboard
        public ActionResult Index()
        {
            if (Session["HeadId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View();
            }
        }
    }
}