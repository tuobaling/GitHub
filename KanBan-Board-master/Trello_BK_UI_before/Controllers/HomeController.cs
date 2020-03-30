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
                return RedirectToAction("Index", "Boards");
            }

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

        ////
        //// Post: /Account/Search
        //[HttpPost]
        //public ActionResult Search(string keyword)
        //{
        //    var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
        //    var userBoardsIds = db.UserBoards.Where((i) => i.UserId == userId).Select((i) => i.BoardId);
        //    var userBoards = db.Boards.Where((i) => userBoardsIds.Contains(i.Id));
        //    var ownedBoards = db.Boards.Where((i) => i.UserId == userId);

        //    var allBoards = userBoards.Union(ownedBoards).ToList();

        //    if (keyword != null && keyword != "")
        //    {
        //        var boards = allBoards.Where((i) => i.Name.Contains(keyword));

        //        ViewBag.Boards = boards.OrderBy((i) => i.Id).ToList();
        //        return View("Index",ViewBag.Boards);
        //    }

        //    ViewBag.Boards = allBoards.OrderBy((i) => i.Id).ToList();
        //    return View("Index", ViewBag.Boards);
        //    //TempData["Keyword"] = keyword;
        //    //return RedirectToAction("Index", "Home");
        //    //return View();
        //}
    }
}