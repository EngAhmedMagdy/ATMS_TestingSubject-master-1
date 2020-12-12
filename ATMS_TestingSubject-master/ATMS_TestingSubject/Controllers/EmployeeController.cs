using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ATMS_TestingSubject.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        // home dashboed
        public ActionResult Index()
        {
            if (Session["EmpId"] == null)
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