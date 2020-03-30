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
            _service.Create(ticket);
        }

        [HttpPost]
        public HttpStatusCode UpdatePosition(List<Ticket> data)
        {
            return _service.UpdatePosition(data);
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
                int result = _service.Update(ticket);

                if (result == 1)
                {
                    return RedirectToAction("Details", "Boards", new { id = ticket.BoardId });
                }
                else
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
                int result = _service.Delete(ticketId);
                if (result == 1)
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                else
                    return new HttpStatusCodeResult(HttpStatusCode.NotModified);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotModified);
            }
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    db.Dispose();
            //}
            base.Dispose(disposing);
        }
    }
}
