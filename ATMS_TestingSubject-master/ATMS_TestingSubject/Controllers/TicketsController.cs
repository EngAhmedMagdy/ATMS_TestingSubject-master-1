﻿using System;
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
    public class TicketsController : Controller
    {
        private ATMS_Model db = new ATMS_Model();
        private static int TicketNum;

        public ActionResult GetTicket(int Id)
        {
            var alltikects = db.Tickets.ToArray();
            TimeSpan TicketAvailable;
            TimeSpan? TimePassed;
            TimeSpan.TryParse("24:00:00", out TicketAvailable); //time needed to get another code
            Ticket Newticket = new Ticket();
            if (alltikects.Length > 0)
            {
                TicketNum = alltikects.Last().RollNo;
            }
            var OldTickets = alltikects.Where(x => x.Id == Id).ToArray();
            if (OldTickets.Length > 0)
            {
                TimePassed = DateTime.Now.TimeOfDay - OldTickets.Last().time;

                if (TimePassed < TicketAvailable)
                {
                    Response.Write("wait " + (TicketAvailable - TimePassed) + " to get a second barcode");
                    Response.Write("your last barcode is");

                    return View(OldTickets.Last());
                }

            }
            Newticket.RollNo = ++TicketNum;
            Newticket.time = DateTime.Now.TimeOfDay;
            Newticket.date = DateTime.Now.ToLocalTime();
            Newticket.Id = Id;
            db.Tickets.Add(Newticket);
            db.SaveChanges();
            Response.Write("your ticket number is " + TicketNum);
            return View(Newticket);
        }

        // GET: Tickets
        public async Task<ActionResult> Index()
        {
            var tickets = db.Tickets.Include(t => t.UserInfo);
            return View(await tickets.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.UserInfoes, "Id", "Type");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RollNo,Id,date,time")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Tickets.Add(ticket);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.UserInfoes, "Id", "Type", ticket.Id);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.UserInfoes, "Id", "Type", ticket.Id);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RollNo,Id,date,time")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.UserInfoes, "Id", "Type", ticket.Id);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Ticket ticket = await db.Tickets.FindAsync(id);
            db.Tickets.Remove(ticket);
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
