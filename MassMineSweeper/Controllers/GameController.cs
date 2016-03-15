using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MassMineSweeper.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace MassMineSweeper.Controllers
{
    public class GameController : Controller
    {
        private MassMineSweeperContext db = new MassMineSweeperContext();

         private readonly UserManager<Member> userManager;

        public GameController()
            : this(Startup.UserManagerFactory.Invoke())
        {
        }

        public GameController(UserManager<Member> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Search()
        {
            if (!User.IsInRole("Alive"))
                return RedirectToAction("YouAreDead");

            var games = (from g in db.MineSweeperGames
                         select g).ToList();
            return View(games);
        }

        [HttpPost, ActionName("Search")]
        [Authorize]
        public ActionResult Search(String searchTerm){
            if (!User.IsInRole("Alive"))
                return RedirectToAction("YouAreDead");
            var games = (from g in db.MineSweeperGames
                         where g.GameName.Contains(searchTerm)
                         select g).ToList();
            return View(games);
        }

        // GET: /Game/Play/5
        [Authorize]
        public ActionResult Play(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Search", new { searchTerm = "" });
            }

            if (!User.IsInRole("Alive"))
                return RedirectToAction("YouAreDead");
            
            
            MineSweeperGame minesweepergame = db.MineSweeperGames.Find(id);
            minesweepergame.Tiles = (from t in db.GameTiles
                                     where t.MineSweeperGameID == minesweepergame.MineSweeperGameID
                                     select t).ToList();
            int count = minesweepergame.Tiles.Count;
            if (minesweepergame == null)
            {
                return HttpNotFound();
            }
            return View(minesweepergame);
        }

        // GET: /Game/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Game/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include="GameName,GameHeight,GameWidth,NumMines")] MineSweeperGame minesweepergame)
        {
            
            if (ModelState.IsValid)
            {
                minesweepergame.Initialize();
                minesweepergame.DateCreated = DateTime.Now;
                db.MineSweeperGames.Add(minesweepergame);
                db.SaveChanges();
                return RedirectToAction("Search");
            }

            return View(minesweepergame);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MineSweeperGame game = db.MineSweeperGames.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: /Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            MineSweeperGame game = db.MineSweeperGames.Find(id);
            db.MineSweeperGames.Remove(game);
            //TODO: remove players assigned to game
            db.SaveChanges();
            return RedirectToAction("Search");
        }

        [HttpPost]
        [Authorize]
        public ActionResult Reveal([Bind(Include = "MineSweeperGameID,XPos,YPos")] GameTile gameTile)
        {
            MineSweeperGame model = (from g in db.MineSweeperGames
                                     where g.MineSweeperGameID == gameTile.MineSweeperGameID
                                     select g).FirstOrDefault();

            

            if (model != null)
            {
                model.Tiles = (from g in db.GameTiles
                               where g.MineSweeperGameID == model.MineSweeperGameID
                               select g).ToList();
                GameTile tile = model.RevealTile(gameTile.XPos, gameTile.YPos);
                if (tile.HasMine && !User.IsInRole("Admin"))
                {
                    Member user = (Member)db.Users.Where(u => u.UserName.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                    if (user != null && userManager.IsInRole(user.Id, "Alive"))
                    {
                        userManager.RemoveFromRole(user.Id, "Alive");
                    }
                    return RedirectToAction("YouAreDead");
                }
                else if (model.IsGameCleared())
                {
                    Member user = (Member)db.Users.Where(u => u.UserName.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                    userManager.AddToRole(user.Id, "Admin");

                    return RedirectToAction("YouWin");
                }
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Play", new { id = model.MineSweeperGameID });
            }
            else
            {
                return RedirectToAction("YouWin");
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Flag([Bind(Include = "MineSweeperGameID,XPos,YPos")] GameTile gameTile)
        {
            MineSweeperGame model = (from g in db.MineSweeperGames
                                     where g.MineSweeperGameID == gameTile.MineSweeperGameID
                                     select g).FirstOrDefault();

           

            if (model != null)
            {
                model.Tiles = (from g in db.GameTiles
                               where g.MineSweeperGameID == model.MineSweeperGameID
                               select g).ToList();

                model.ToggleTileFlag(gameTile.XPos, gameTile.YPos);
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                if (model.IsGameCleared())
                {
                    Member user = (Member)db.Users.Where(u => u.UserName.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                    userManager.AddToRole(user.Id, "Admin");

                    return RedirectToAction("YouWin");
                }
                return RedirectToAction("Play", new { id = model.MineSweeperGameID });
            }
            else
                return RedirectToAction("Play", new { id = model.MineSweeperGameID });
        }

        [HttpGet]
        [Authorize]
        public ActionResult YouAreDead()
        {
            
            return View();
        }

        [HttpGet]
        [Authorize(Roles="Admin")]
        public ActionResult YouWin()
        {
            return View();
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
