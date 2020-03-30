using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Trello;
using Trello.DAL.Models;
using Trello.BLL.Services;

namespace Trello.Controllers
{
    public class UserBoardsController : AsyncController
    {
        private IUserBoardsService _service;

        public UserBoardsController(IUserBoardsService service)
        {
            _service = service;
        }

        // GET: UserBoards
        public async Task<ActionResult> Index(int id)
        {
            string ownerId = "";
            IEnumerable<UserBoard> userBoards = await _service.Get(id, ownerId);

            ViewBag.OwnerId = ownerId;
            ViewBag.BoardId = id;
            ViewBag.UserBoards = userBoards;
            return View();
        }

        // POST: UserBoards/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BoardId,UserId")] UserBoard userBoard)
        {
            if (ModelState.IsValid)
            {
                int result = _service.Create(userBoard);
                if (result == 1)
                {
                    return RedirectToAction("Index", new { id = userBoard.BoardId });

                }
                else
                {
                    return new HttpNotFoundResult("Failed");
                }
            }

            return new HttpNotFoundResult("Failed");
        }

        // POST: UserBoards/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            int boardId;
            int result = _service.Delete(id, out boardId);

            if (result == 1)
            {
                return RedirectToAction("Index", "UserBoards", new { id = boardId });
            }
            else
            {
                return new HttpNotFoundResult("Failed");
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
