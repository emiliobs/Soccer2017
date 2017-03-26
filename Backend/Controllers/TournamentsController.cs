using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Backend.Helpers;
using Backend.Models;
using Domains;

namespace Backend.Controllers
{
    [Authorize]
    public class TournamentsController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: Tournaments
        public async Task<ActionResult> Index()
        {
            return View(await db.Tournaments.OrderBy(t =>t.Name).ToListAsync());
        }

        // GET: Tournaments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = await db.Tournaments.FindAsync(id);

            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        // GET: Tournaments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tournaments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TournamentsView view)
        {
            if (ModelState.IsValid)
            {
                var picture = string.Empty;
                var folder = "~/Content/Logos";

                if (view.LogoFile != null)
                {
                    picture = FileHelper.UploadPhoto(view.LogoFile, folder);
                    picture = $"{folder}/{picture}";

                }

                var tournament = ToTournament(view);

                tournament.Logo = picture;

                db.Tournaments.Add(tournament);

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception)
                {

                    
                }

                return RedirectToAction("Index");
            }

            return View(view);
        }

        private Tournament ToTournament(TournamentsView view)
        {
             return  new Tournament()
             {
                 Name = view.Name,
                 Logo =  view.Logo,
                 IsActive = view.IsActive,
                 Order = view.Order,
                TournamentId = view.TournamentId,
                 
             };
        }

        // GET: Tournaments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Tournament tournament = await db.Tournaments.FindAsync(id);

            if (tournament == null)
            {
                return HttpNotFound();
            }

            var view = ToView(tournament);

            return View(view);
        }

        private TournamentsView ToView(Tournament tournament)
        {
            return  new TournamentsView()
            {
                IsActive = tournament.IsActive,
                Order = tournament.Order,
                Name = tournament.Name,
                Logo = tournament.Logo,
                TournamentId = tournament.TournamentId,
                
                
                
            };
        }

        // POST: Tournaments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TournamentsView view)
        {
            if (ModelState.IsValid)
            {
                var picture = view.Logo;
                var folder = "~/Content/Logos";

                if (view.LogoFile != null)
                {
                    picture = FileHelper.UploadPhoto(view.LogoFile, folder);
                    picture = $"{folder}/{picture}";

                }

                var tournament = ToTournament(view);

                tournament.Logo = picture;

                db.Entry(tournament).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception)
                {

                    
                }

                return RedirectToAction("Index");
            }
            return View(view);
        }

        // GET: Tournaments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = await db.Tournaments.FindAsync(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        // POST: Tournaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Tournament tournament = await db.Tournaments.FindAsync(id);
            db.Tournaments.Remove(tournament);
            await db.SaveChangesAsync();
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
