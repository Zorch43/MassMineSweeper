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
    public class GameController : Controller
    {
        private MassMineSweeperContext db = new MassMineSweeperContext();

        [HttpGet]
        public ActionResult Search()
        {
            var games = (from g in db.MineSweeperGames
                         select g).ToList();
            return View(games);
        }

        [HttpPost, ActionName("Search")]
        public ActionResult Search(String searchTerm){
            var games = (from g in db.MineSweeperGames
                         where g.GameName.Contains(searchTerm)
                         select g).ToList();
            return View(games);
        }

        // GET: /Game/Play/5
        public ActionResult Play(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Search");
            }
            MineSweeperGame minesweepergame = db.MineSweeperGames.Find(id);
            if (minesweepergame == null)
            {
                return HttpNotFound();
            }
            return View(minesweepergame);
        }

        // GET: /Game/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Game/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="GameName,RespawnLength,GameHeight,GameWidth,NumMines")] MineSweeperGame minesweepergame)
        {
            minesweepergame.DateCreated = DateTime.Now;
            if (ModelState.IsValid)
            {
                minesweepergame.DateCreated = DateTime.Now;
                db.MineSweeperGames.Add(minesweepergame);
                db.SaveChanges();
                return RedirectToAction("Search");
            }

            return View(minesweepergame);
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
