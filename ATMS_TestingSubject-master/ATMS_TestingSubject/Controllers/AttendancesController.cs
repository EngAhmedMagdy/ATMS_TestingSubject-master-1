using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ATMS_TestingSubject.Models;

namespace ATMS_TestingSubject.Controllers
{
    
    public class AttendancesController : Controller
    {
        private ATMS_Model db = new ATMS_Model();

        // GET: Attendances
        public async Task<ActionResult> Index()
        {
            var attendances = db.Attendances.Include(a => a.Ticket);
            return View(await attendances.ToListAsync());
        }

        // GET: Attendances/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = await db.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // GET: Attendances/Create
        [HttpGet]
        public ActionResult Create(int id)
        {
            
            TempData["CurrentUser"] = id;
           
            Response.Write(id);
            
            return View();
        }

        // POST: Attendances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RollNo")] Attendance attendance)
        {

            int CU = (int)TempData.Peek("CurrentUser");

            var LastTickets = db.Tickets.Where(x => x.Id == CU).ToArray();
            if (LastTickets.Length>0)
            {
                Ticket LastTicket = db.Tickets.Where(x => x.Id == CU).ToArray().Last();
                if (LastTicket.RollNo == attendance.RollNo)
                {

                        attendance.State = true;
                        attendance.Date = DateTime.Now.Date;
                        attendance.Time = DateTime.Now.TimeOfDay;
                        db.Attendances.Add(attendance);
                        db.UserInfoes.Single(x => x.Id == CU).Active = true;
                        await db.SaveChangesAsync();

                        return RedirectToAction("Details","UserInfo",new { id = CU});
                    
                }
                else
                {
                    Response.Write("wrong barcode");
                    return View();
                }

            }
            Response.Write("No barcode found! 404");
            return View();
        }

        // GET: Attendances/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = await db.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            ViewBag.RollNo = new SelectList(db.Tickets, "RollNo", "RollNo", attendance.RollNo);
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AttendId,RollNo,Date,Time,State")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attendance).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.RollNo = new SelectList(db.Tickets, "RollNo", "RollNo", attendance.RollNo);
            return View(attendance);
        }

        // GET: Attendances/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = await db.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Attendance attendance = await db.Attendances.FindAsync(id);
            db.Attendances.Remove(attendance);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
