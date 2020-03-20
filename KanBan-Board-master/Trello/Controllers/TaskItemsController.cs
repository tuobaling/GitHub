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
    public class TaskItemsController : Controller
    {
        private DataModel db = new DataModel();


        // POST: TaskItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public HttpStatusCode Create([Bind(Include = "Name,CardId")] TaskItem taskItem)
        {
            taskItem.CreatedOn = DateTime.Now;
            taskItem.Status = "NOT DONE";

            db.TaskItems.Add(taskItem);
            db.SaveChanges();
            return HttpStatusCode.OK;
        }


        // POST: TaskItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Status,CardId")] TaskItem taskItem)
        {
            var task = db.TaskItems.FirstOrDefault(i => i.Id == taskItem.Id);
            if (task == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            task.Status = taskItem.Status;

            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();

            ViewBag.TaskItems = db.TaskItems.Where((i) => i.CardId == taskItem.CardId);
            return PartialView("_TaskItemsList");
        }


        // POST: TaskItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.TaskItems.Remove(db.TaskItems.Find(id));
            db.SaveChanges();
            return View();
        }

        [HttpPost]
        public HttpStatusCode UpdatePosition(List<TaskItem> data)
        {
            var cardId = data[0].CardId;
            var taskitems = db.TaskItems.Where((i) => i.CardId == cardId).ToList();

            foreach (TaskItem t in data)
            {
                var taskitem = taskitems.First((i) => i.Id == t.Id);
                taskitem.PositionNo = t.PositionNo;
                db.Entry(taskitem).State = EntityState.Modified;
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
