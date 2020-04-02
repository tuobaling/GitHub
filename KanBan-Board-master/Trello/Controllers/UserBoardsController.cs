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
            var ownerId = _service.GetBoardById(id).UserId;

            Task<IEnumerable<UserBoard>> userBoards = Task.Factory.StartNew(
               () => _service.Get(id)
            );

            await Task.WhenAll(userBoards);

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
                _service.Insert(userBoard);
                return RedirectToAction("Index", new { id = userBoard.BoardId });
            }

            return new HttpNotFoundResult("Failed");
        }

        // POST: UserBoards/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var boardId = _service.GetById(id).BoardId;
            _service.Delete(id);
           
            return RedirectToAction("Index", "UserBoards", new { id = boardId });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
