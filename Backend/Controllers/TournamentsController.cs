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

        #region Action from TournamentGroups
                                               
        public ActionResult CreateTeam()
        {
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "Name");
            ViewBag.TournamentGroupId = new SelectList(db.TournamentGroups, "TournamentGroupId", "Name");
            return View();
        }

       [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTeam([Bind(Include = "TournamentTeamId,TournamentGroupId,TeamId,MatchesPlayed,MatchesWon,MatchesLost,MatchesTied,FavorGoals,AgainstGoals,Points,Position")] TournamentTeam tournamentTeam)
        {
            if (ModelState.IsValid)
            {
                db.TournamentTeams.Add(tournamentTeam);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "Name", tournamentTeam.TeamId);
            ViewBag.TournamentGroupId = new SelectList(db.TournamentGroups, "TournamentGroupId", "Name", tournamentTeam.TournamentGroupId);
            return View(tournamentTeam);
        }

        public async Task<ActionResult> DetailsTournamentGroup(int? id)
        {
            if (id == null)
            {
               return  new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tournamentGroup = await db.TournamentGroups.FindAsync(id);

            if (tournamentGroup == null)
            {
                return HttpNotFound();
            }

            return View(tournamentGroup);

        }

        // GET: Dates/Create
        public async Task<ActionResult> CreateDate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tournament = await db.Tournaments.FindAsync(id);

            if (tournament == null)
            {
                return HttpNotFound();
            }

            var view = new Date()
            {
                TournamentId = tournament.TournamentId,
            };

            ViewData["Tournament"] = tournament.Name;

            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateDate(Date date)
        {
            if (ModelState.IsValid)
            {
                db.Dates.Add(date);

                try
                {
                    await db.SaveChangesAsync();

                }
                catch (Exception)
                {

                }

                return RedirectToAction($"Details/{date.TournamentId}");
            }

            return View(date);
        }

        public async Task<ActionResult> EditDate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var date = await db.Dates.FindAsync(id);

            if (date == null)
            {
                return HttpNotFound();
            }

         
                
            return View(date);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditDate(Date date)
        {
            if (ModelState.IsValid)
            {
                db.Entry(date).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception)
                {

                   
                }

                return RedirectToAction($"Details/{date.TournamentId}");
            }


            return View(date);
        }

        public async Task<ActionResult> DeleteDate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var date = await db.Dates.FindAsync(id);

            if (date == null)
            {
                return HttpNotFound();
            }

             db.Dates.Remove(date);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {

            }     

            return RedirectToAction($"Details/{date.TournamentId}");
        }

       
        public async Task<ActionResult> CreateTournamenGroup(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tournament = await db.Tournaments.FindAsync(id);

            if (tournament == null)
            {
                return HttpNotFound();
            }

            ViewData["Tournamet"] = tournament.Name;

            //aqui retorono all object have this  id  del tournametGrpup with your toournament;
            var view = new TournamentGroup()
            {
                TournamentId = tournament.TournamentId,
            };

            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTournamenGroup(TournamentGroup tournamentGroup)
        {
            if (ModelState.IsValid)
            {
                db.TournamentGroups.Add(tournamentGroup);

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception)
                {


                }

                return RedirectToAction($"Details/{tournamentGroup.TournamentId}");
            }

            return View(tournamentGroup);
        }


        public async Task<ActionResult> EditTournamentGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TournamentGroup tournamentGroup = await db.TournamentGroups.FindAsync(id);

            ViewData["Tournamet"] = tournamentGroup.Tournament.Name;

            if (tournamentGroup == null)
            {
                return HttpNotFound();
            }
            return View(tournamentGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditTournamentGroup(TournamentGroup tournamentGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tournamentGroup).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception)
                {


                }

                return RedirectToAction($"Details/{tournamentGroup.TournamentId}");
            }
            ViewBag.TournamentId = new SelectList(db.Tournaments, "TournamentId", "Name", tournamentGroup.TournamentId);
            return View(tournamentGroup);
        }

        public async Task<ActionResult> DeleteTournamentGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tournamentGroup = await db.TournamentGroups.FindAsync(id);

            if (tournamentGroup == null)
            {
                return HttpNotFound();
            }

            db.TournamentGroups.Remove(tournamentGroup);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {

            }

            return RedirectToAction($"Details/{tournamentGroup.TournamentId}");
        }

        #endregion

        #region Action Natives from Controller

        // GET: Tournaments
        public async Task<ActionResult> Index()
        {
            return View(await db.Tournaments.OrderBy(t => t.Name).ToListAsync());
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
            return new Tournament()
            {
                Name = view.Name,
                Logo = view.Logo,
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
            return new TournamentsView()
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

        #endregion
    }
}
