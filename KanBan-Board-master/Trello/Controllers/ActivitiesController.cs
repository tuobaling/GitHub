using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Trello;
using Trello.DAL.Models;

namespace Trello.Controllers
{
    public class ActivitiesController : Controller
    {
        private DataModel db = new DataModel();


        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpStatusCode Create([Bind(Include = "Name,CardId")] Activity activity)
        {
            activity.CreatedOn = DateTime.Now;

            db.Activities.Add(activity);
            db.SaveChanges();
            return HttpStatusCode.OK;
        }


        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CardId")] Activity activity)
        {
            var task = db.Activities.FirstOrDefault(i => i.Id == activity.Id);
            if (task == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            db.SaveChanges();

            ViewBag.Activities = db.Activities.Where((i) => i.CardId == activity.CardId);
            return PartialView("_ActivitiesList");
        }


        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Activities.Remove(db.Activities.Find(id));
            db.SaveChanges();
            return View();
        }

        [HttpPost]
        public HttpStatusCode UpdatePosition(List<Activity> data)
        {
            var cardId = data[0].CardId;
            var activities = db.Activities.Where((i) => i.CardId == cardId).ToList();

            foreach (Activity t in data)
            {
                var activity = activities.First((i) => i.Id == t.Id);
                activity.PositionNo = t.PositionNo;
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();
            }
            return HttpStatusCode.OK;
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
