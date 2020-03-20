using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Trello.DAL.Models;

namespace Trello.Controllers
{
    public class HomeController : Controller
    {
        private DataModel db = new DataModel();

        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                if ((string)TempData["Keyword"] != null && (string)TempData["Keyword"] != "")
                {
                    //TempData["Keyword"] = "";
                    //TempData["Boards"] = TempData["Boards"];
                    return View();
                }

                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                var userBoardsIds = db.UserBoards.Where((i) => i.UserId == userId).Select((i) => i.BoardId);
                var userBoards = db.Boards.Where((i) => userBoardsIds.Contains(i.Id));
                var ownedBoards = db.Boards.Where((i) => i.UserId == userId);

                var allBoards = userBoards.Union(ownedBoards).ToList();

                TempData["Boards"] = allBoards;
            }
            else
                TempData["Boards"] = new List<Board>();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //
        // Post: /Account/Search
        [HttpPost]
        public ActionResult Search(string keyword)
        {
            var boards = db.Boards.Where((i) => i.Name == keyword);

            TempData["Keyword"] = keyword;
            TempData["Boards"] = boards.OrderBy((i) => i.Id).ToList();
            return RedirectToAction("Index", "Home");
            //return View();
        }
    }
}