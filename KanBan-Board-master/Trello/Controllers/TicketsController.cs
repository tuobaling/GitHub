using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Trello;
using Trello.BLL.Services;
using Trello.DAL.Models;

namespace Trello.Controllers
{
    public class TicketsController : Controller
    {
        private ITicketsService _service;

        public TicketsController(ITicketsService service)
        {
            _service = service;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Create([Bind(Include = "Name,BoardId")] Ticket ticket)
        {
            _service.Insert(ticket);
        }

        [HttpPost]
        public void UpdatePosition(List<Ticket> data)
        {
            _service.UpdatePosition(data);
        }


        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,BoardId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _service.Update(ticket);
                    return RedirectToAction("Details", "Boards", new { id = ticket.BoardId });
                }
                catch
                {
                    return new HttpNotFoundResult("Failed");
                }
            }
            //ViewBag.BoardId = new SelectList(db.Boards, "Id", "Name", ticket.BoardId);
            return View(ticket);
        }


        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "ticketId")]int ticketId)
        {
            try
            {
                _service.Delete(ticketId);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotModified);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
