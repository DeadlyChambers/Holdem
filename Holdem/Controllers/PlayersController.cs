using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CommonCardLibrary.Entities;
using Holdem.Models;
using Holdem.Services;
using Microsoft.AspNet.Identity;

namespace Holdem.Controllers
{
    [Authorize]
    public class PlayersController : Controller
    {
        private GameContext db = new GameContext();

        // GET: Players
        public ActionResult Index()
        {
            var id = Guid.Parse(User.Identity.GetUserId());
            var player = db.Players.Find(id);
            var vm = new DisplayPlayerOnIndexViewModel
            {
                Players = db.Players.ToList(),
                Player = player
            };
 
            return View(vm);
        }

        // GET: Players/Details/5
        public ActionResult Details()
        {
            var id = Guid.Parse(User.Identity.GetUserId());
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }
       

       

        // POST: Players/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Player player)
        {
            var id = Guid.Parse(User.Identity.GetUserId());
            if (player.Cash > 0)
            {
                db.Players.Find(id).Cash = player.Cash;
                db.SaveChanges();
            }
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
