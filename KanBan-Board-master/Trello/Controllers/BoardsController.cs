using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Trello;
using Trello.DAL;
using Trello.DAL.Models;
using WebGrease.Css.Extensions;

namespace Trello.Controllers
{
    [Authorize]
    public class BoardsController : Controller
    {
        private DataModel db = new DataModel();
        public enum BoardType
        {
            None = 0,
            Normal = 1,
            Template = 2
        }
        // GET: Boards
        public ActionResult Index()
        {

            //using (var context = new DataModel())
            //{
            //    var board = context.Boards.Find(1);
            //    context.Entry(board).Collection("Tickets").Load();
            //}
            var selectList = new List<SelectListItem>()
            {
                new SelectListItem {Text="All", Value="All" },
                new SelectListItem {Text="Board", Value="Board" },
                new SelectListItem {Text="Card", Value="Card" },
            };

            //預設選擇哪一筆
            selectList.Where(q => q.Value == "Board").First().Selected = true;

            ViewBag.SearchType = selectList;

            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var userBoardsIds = db.UserBoards.Where((i) => i.UserId == userId).Select((i) => i.BoardId);
            var userBoards = db.Boards.Where((i) => userBoardsIds.Contains(i.Id));
            var ownedBoards = db.Boards.Where((i) => i.UserId == userId);

            var allBoards = userBoards.Union(ownedBoards).ToList();

            return View(allBoards);
        }

        // GET: Boards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Board board = db.Boards.Find(id);
            var tickets = db.Tickets.Where(i => i.BoardId == board.Id).OrderBy((j) => j.PositionNo).ToList();

            tickets.ForEach((i) =>
            {
                i.Cards = i.Cards.OrderBy((j) => j.PositionNo).ToList();
            });


            if (board == null)
            {
                return HttpNotFound();
            }
            board.Tickets = tickets.ToList();
            return View(board);
        }


        // POST: Boards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] Board board)
        {
            var selectList = new List<SelectListItem>()
            {
                new SelectListItem {Text="All", Value="All" },
                new SelectListItem {Text="Board", Value="Board" },
                new SelectListItem {Text="Card", Value="Card" },
            };

            //預設選擇哪一筆
            selectList.Where(q => q.Value == "Board").First().Selected = true;

            ViewBag.SearchType = selectList;

            board.UserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            board.CreatedOn = DateTime.Now;
            board.BoardType = (int)BoardType.Normal;

            if (ModelState.IsValid)
            {
                db.Boards.Add(board);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(board);
        }

        //// POST: Boards/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Name")] string action, string boardName)
        //{
        //    Board board = new Board();
        //    board.Name = boardName;
        //    board.UserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
        //    board.CreatedOn = DateTime.Now;
        //    board.BoardType = (int)BoardType.Normal;

        //    if (ModelState.IsValid)
        //    {
        //        db.Boards.Add(board);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(board);
        //}

        // GET: Boards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Board board = db.Boards.Find(id);
            if (board == null)
            {
                return HttpNotFound();
            }
            return View(board);
        }

        // POST: Boards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Board board)
        {
            if (ModelState.IsValid)
            {
                var boards = db.Boards.FirstOrDefault(i => i.Id == board.Id);
                if (boards == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                boards.Name = board.Name;

                db.Entry(boards).State = EntityState.Modified;
                db.SaveChanges();

                return View("Details", boards);
            }
            return View(board);
        }


        // POST: Boards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "boardId")]int boardId)
        {
            var selectList = new List<SelectListItem>()
            {
                new SelectListItem {Text="All", Value="All" },
                new SelectListItem {Text="Board", Value="Board" },
                new SelectListItem {Text="Card", Value="Card" },
            };

            //預設選擇哪一筆
            selectList.Where(q => q.Value == "Board").First().Selected = true;

            ViewBag.SearchType = selectList;

            try
            {
                Board board = db.Boards.Find(boardId);

                board.Tickets.ForEach((i) =>
                {
                    i.Cards.ForEach((j) =>
                    {
                        db.TaskItems.RemoveRange(j.TaskItems);
                    });

                    db.Cards.RemoveRange(i.Cards);
                });

                db.Tickets.RemoveRange(board.Tickets);
                db.Boards.Remove(board);
                db.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotModified);
            }
        }

        //
        // Post: /Boards/Search
        [HttpPost]
        public ActionResult Search(string keyword, string searchtype)
        {
            var selectList = new List<SelectListItem>()
            {
                new SelectListItem {Text="All", Value="All" },
                new SelectListItem {Text="Board", Value="Board" },
                new SelectListItem {Text="Card", Value="Card" },
            };

            //預設選擇哪一筆
            selectList.Where(q => q.Value == "Board").First().Selected = true;

            ViewBag.SearchType = selectList;

            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var userBoardsIds = db.UserBoards.Where((i) => i.UserId == userId).Select((i) => i.BoardId);
            var userBoards = db.Boards.Where((i) => userBoardsIds.Contains(i.Id));
            var ownedBoards = db.Boards.Where((i) => i.UserId == userId);

            var allBoards = userBoards.Union(ownedBoards).ToList();

            if (keyword != null && keyword != "" && searchtype == "Board")
            {
                var boards = allBoards.Where((i) => i.Name.Contains(keyword)).OrderBy((i) => i.Id).ToList();
                return View("Index", boards);
            }
            else if (keyword != null && keyword != "" && searchtype == "Ticket")
            {
                var tickets = db.Tickets.Where((i) => i.Name.Contains(keyword)).Select((i) => i.BoardId).ToList();
                var boards = allBoards.Where((i) => tickets.Contains(i.Id)).OrderBy((i) => i.Id).ToList();
                return View("Index", boards);
            }
            else if (keyword != null && keyword != "" && searchtype == "Card")
            {
                var cards = db.Cards.Where((i) => i.Name.Contains(keyword)).Select((i) => i.TicketId).ToList();
                var tickets = db.Tickets.Where((i) => cards.Contains(i.Id)).Select((i) => i.BoardId).ToList();
                var boards = allBoards.Where((i) => tickets.Contains(i.Id)).OrderBy((i) => i.Id).ToList();
                return View("Index", boards);
            }
            return View("Index", allBoards);
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
