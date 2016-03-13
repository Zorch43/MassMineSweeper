using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MassMineSweeper.Models;

namespace MassMineSweeper.Controllers
{
    public class UserController : Controller
    {
        private MassMineSweeperContext db = new MassMineSweeperContext();

        // GET: /User/Details/5
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GamePlayer gameplayer = db.GamePlayers.Find(id);
            if (gameplayer == null)
            {
                return HttpNotFound();
            }
            return View(gameplayer);
        }

        // GET: /User/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: /User/Register
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include="UserName,Email,Password")] GamePlayer gameplayer)
        {
            if (ModelState.IsValid)
            {
                gameplayer.IsAdmin = false;
                gameplayer.MineSweeperGameId = null;
                gameplayer.RespawnAt = null;
                gameplayer.XPos = 0;
                gameplayer.YPos = 0;
                db.GamePlayers.Add(gameplayer);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(gameplayer);
        }

        // GET: /User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GamePlayer gameplayer = db.GamePlayers.Find(id);
            if (gameplayer == null)
            {
                return HttpNotFound();
            }
            return View(gameplayer);
        }

        // POST: /User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GamePlayerId,MineSweeperGameId,UserName,Email,Password,RespawnAt,XPos,YPos,IsAdmin")] GamePlayer gameplayer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gameplayer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Detail", new { id = gameplayer.GamePlayerId });
            }
            return View(gameplayer);
        }

        //GET
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost, ActionName("Login")]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "UserName,Password")] GamePlayer gameplayer)
        {
            if (ModelState.IsValid)
            {
                GamePlayer player = (from p in db.GamePlayers
                                     where p.UserName == gameplayer.UserName
                                     && p.Password == gameplayer.Password
                                     select p).FirstOrDefault();
                
                return RedirectToAction("Detail", new { id = player.GamePlayerId });
            }
            return View(gameplayer);
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
