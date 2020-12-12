using ATMS_TestingSubject.Classes;
using ATMS_TestingSubject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ATMS_TestingSubject.Controllers
{
    public class AdminController : Controller
    {
        // you can use 'db' object to database connection
        private ATMS_Model db = new ATMS_Model();
        // home dashboard 
        public ActionResult Index()
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View();
            }
        }
        // Details Me 
        [HttpGet]
        public ActionResult DetailsMe()
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                int id = int.Parse(Session["AdminId"].ToString());
                var myAcc = db.UserInfoes.Single(a => a.Id == id);
                return View(myAcc);
            }
        }
        // view for edit my account
        [HttpGet]
        public ActionResult EditMe()
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                int id = int.Parse(Session["AdminId"].ToString());
                var myAcc = db.UserInfoes.Single(a => a.Id == id);
                myAcc.Passward = "";
                return View(myAcc);
            }
        }
        // button for edit me
        [HttpPost]
        public ActionResult EditMe(UserInfo user)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {

                    int id = int.Parse(Session["AdminId"].ToString());
                    var myAcc = db.UserInfoes.Single(a => a.Id == id);
                    string passwordCome = CryptPassword.Hash(user.Passward);
                    if (myAcc.Passward == passwordCome)
                    {
                        if (myAcc.Email != user.Email)
                        {
                            if (db.UserInfoes.Any(e => e.Email == user.Email))
                            {
                                ViewBag.msg = "This Email is already in use";

                            }
                            else
                            {
                                myAcc.Name = user.Name;
                                myAcc.Gender = user.Gender;
                                myAcc.Email = user.Email;
                                db.SaveChanges();
                                Session["AdminName"] = myAcc.Name;
                                return RedirectToAction("Index", "Admin");
                            }
                        }
                        else
                        {
                            myAcc.Name = user.Name;
                            myAcc.Gender = user.Gender;
                            db.SaveChanges();
                            return RedirectToAction("Index", "Admin");
                        }
                    }
                    else
                    {
                        ViewBag.msg = "Password in Incorrect";
                    }
                }
                else
                {
                    return View();
                }
                return View();
            }
        }
        // view for edit my Password
        [HttpGet]
        public ActionResult ChangePassword()
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View();
            }
        }
        // button for edit my password
        [HttpPost]
        public ActionResult ChangePassword(ChangePassword changepass)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    int id = int.Parse(Session["AdminId"].ToString());
                    var myAcc = db.UserInfoes.Single(a => a.Id == id);
                    if (myAcc.Passward == CryptPassword.Hash(changepass.OldPassword))
                    {
                        myAcc.Passward = CryptPassword.Hash(changepass.NewPassword);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        ViewBag.msg = "Old Passoed is InCorrect";
                    }
                }
                else
                {
                    return View();
                }
                return View();
            }
        }
        // Depatments
        public ActionResult Departments()
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View(db.Departments.ToList());
            }
        }
        // add department view
        [HttpGet]
        public ActionResult AddDepartment()
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewBag.Heads = new SelectList(db.UserInfoes.Where(a => a.Type == "Head" && a.DepId == null), "Id", "Name");
                return View();
            }
        }
        // button add department 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDepartment(Department dept)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {

                    if (db.Departments.Any(a => a.DepName == dept.DepName))
                    {
                        ViewBag.msg = "This Name is here, enter other name";
                    }
                    else
                    {
                        Department d = new Department();
                        d.DepName = dept.DepName;
                        db.Departments.Add(d);
                        db.SaveChanges();
                        return RedirectToAction("Departments", "Admin");
                    }
                }
                else
                {
                    return View();
                }
            }
            return View();
        }
        // detils for department 
        [HttpGet]
        public ActionResult DetailsDepartment(int? id)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Department dept = db.Departments.Find(id);
                if (dept == null)
                {
                    return HttpNotFound();
                }
                return View(dept);
            }
        }
        // Edit for department  view
        [HttpGet]
        public ActionResult EditDepartment(int? id)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Department dept = db.Departments.Find(id);
                if (dept == null)
                {
                    return HttpNotFound();
                }
                return View(dept);
            }
        }
        // button edit department 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDepartment(Department dept)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {

                    if (db.Departments.Any(a => a.DepName == dept.DepName))
                    {
                        ViewBag.msg = "This Name is here, enter other name";
                    }
                    else
                    {
                        db.Entry(dept).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Departments", "Admin");
                    }
                }
                else
                {
                    return View();
                }
            }
            return View();
        }
        // detils for detele department 
        [HttpGet]

        public ActionResult DeleteDepartment(int? id)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Department dept = db.Departments.Find(id);
                if (dept == null)
                {
                    return HttpNotFound();
                }
                return View(dept);
            }
        }
        // button delete department 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDepartment(Department dept)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                db.Entry(dept).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Departments", "Admin");
            }
        }
        // requests for emp
        public ActionResult Requests()
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View(db.UserInfoes.Where(a => a.Type == "Employee" && a.Accepted == false));
            }
        }
        // accepted for emp
        [HttpGet]
        public ActionResult Approve(int? id)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UserInfo user = db.UserInfoes.Where(a => a.Type == "Employee" && a.Accepted == false && a.Id == id).FirstOrDefault();
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
        }
        // button approve emp  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(UserInfo user)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                var us = db.UserInfoes.Single(a => a.Id == user.Id);
                us.Accepted = true;
                db.SaveChanges();
                return RedirectToAction("Requests", "Admin");
            }
        }
        // reject for emp
        [HttpGet]
        public ActionResult Reject(int? id)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UserInfo user = db.UserInfoes.Where(a => a.Type == "Employee" && a.Accepted == false && a.Id == id).FirstOrDefault();
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reject(UserInfo user)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                db.Entry(user).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Requests", "Admin");
            }
        }

        // Employees
        public ActionResult Users(string Type)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                int id = int.Parse(Session["AdminId"].ToString());
                if (Type == "Head")
                {
                    return View(db.UserInfoes.Where(a => a.Type.Equals("Head") && a.Accepted == null).ToList());
                }
                else if (Type == "Employee")
                {
                    return View(db.UserInfoes.Where(a => a.Type.Equals("Employee") && a.Accepted == true).ToList());
                }
                else if (Type == "Admin")
                {
                    return View(db.UserInfoes.Where(a => a.Type.Equals("Admin") && a.Accepted == null && a.Id != id).ToList());
                }
                return View(db.UserInfoes.Where(a => a.Accepted == true && a.Accepted == null).ToList());
            }

        }
        // view for add emp
        [HttpGet]
        public ActionResult AddEmp()
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewBag.DepId = new SelectList(db.Departments, "DepId", "DepName");
                return View();
            }
        }
        // button for add emp
        [HttpPost]
        public ActionResult AddEmp(UserRegister userInfo)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (db.UserInfoes.Any(e => e.Email == userInfo.Email))
                {
                    ModelState.AddModelError("Email", "Email is already in use");
                }
                if (ModelState.IsValid)
                {
                    UserInfo ui = new UserInfo();
                    ui.Type = userInfo.Type;
                    ui.Name = userInfo.Name;
                    ui.Passward = CryptPassword.Hash(userInfo.Passward);
                    ui.Email = userInfo.Email;
                    ui.Gender = userInfo.Gender;
                    ui.DepId = userInfo.DepId;
                    ui.Active = false;
                    ui.Accepted = true;
                    ui.AbsenceHours = 0;
                    db.UserInfoes.Add(ui);
                    db.SaveChangesAsync();
                    return RedirectToAction("Users", "Admin");
                }
                ViewBag.DepId = new SelectList(db.Departments, "DepId", "DepName");
                return View(userInfo);
            }
        }
       
        // view for add head
        [HttpGet]
        public ActionResult AddHead()
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewBag.DepId = new SelectList(db.Departments, "DepId", "DepName");
                return View();
            }
        }
        // button for add head
        [HttpPost]
        public ActionResult AddHead(UserRegister userInfo)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (db.UserInfoes.Any(e => e.Email == userInfo.Email))
                {
                    ModelState.AddModelError("Email", "Email is already in use");
                }
                if (ModelState.IsValid)
                {
                    UserInfo ui = new UserInfo();
                    ui.Type = userInfo.Type;
                    ui.Name = userInfo.Name;
                    ui.Passward = CryptPassword.Hash(userInfo.Passward);
                    ui.Email = userInfo.Email;
                    ui.Gender = userInfo.Gender;
                    ui.DepId = userInfo.DepId;
                    ui.Active = false;
                    ui.Accepted = null;
                    ui.AbsenceHours = 0;
                    db.UserInfoes.Add(ui);
                    db.SaveChangesAsync();
                    return RedirectToAction("Users", "Admin");
                }
                ViewBag.DepId = new SelectList(db.Departments, "DepId", "DepName");
                return View(userInfo);
            }
        }        
        // view for add admin
        [HttpGet]
        public ActionResult AddAdmin()
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View();
            }
        }
        // button for add Admin
        [HttpPost]
        public ActionResult AddAdmin(UserRegister userInfo)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (db.UserInfoes.Any(e => e.Email == userInfo.Email))
                {
                    ModelState.AddModelError("Email", "Email is already in use");
                }
                if (ModelState.IsValid)
                {
                    UserInfo ui = new UserInfo();
                    ui.Type = userInfo.Type;
                    ui.Name = userInfo.Name;
                    ui.Passward = CryptPassword.Hash(userInfo.Passward);
                    ui.Email = userInfo.Email;
                    ui.Gender = userInfo.Gender;
                    ui.DepId = null;
                    ui.Active = null;
                    ui.Accepted = null;
                    ui.AbsenceHours = null;
                    db.UserInfoes.Add(ui);
                    db.SaveChangesAsync();
                    return RedirectToAction("Users", "Admin");
                }
                return View(userInfo);
            }
        }
        // details for head & admin & emp
        [HttpGet]
        public ActionResult DetailsUser(int? id)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UserInfo usre = db.UserInfoes.Find(id);
                if (usre == null)
                {
                    return HttpNotFound();
                }
                return View(usre);
            }
        }
        // detils for detele head & admin & emp 
        [HttpGet]

        public ActionResult DeleteUser(int? id)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UserInfo user = db.UserInfoes.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
        }
        // button delete users 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUser(UserInfo user)
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                db.Entry(user).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Users", "Admin");
            }
        }
        // view for online emp
        public ActionResult OnlineUsers()
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View(db.UserInfoes.Where(a => a.Type == "Employee" && a.Accepted == true && a.Active == true));
            }
        }
        // view for online emp
        public ActionResult LeavingRequest()
        {
            if (Session["AdminId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                DateTime d = DateTime.Now.Date;
                return View(db.Leavings.Where(a=>a.Date == d));
            }
        }

    }
}