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

        public async Task<ActionResult> CreateMatch(int? id)
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

            ViewBag.LocalLeagueId = new SelectList(db.Leagues.OrderBy(l => l.Name), "LeagueId", "Name");//Esto espor que ya hau una liga seleccionada
            ViewBag.LocalId = new SelectList(db.Teams.OrderBy(t => t.Name).Where(t => t.LeagueId == db.Leagues.FirstOrDefault().LeagueId).OrderBy(t => t.Name), "LeagueId", "Name");//aqui tomo solo el team de la liga seleccionada

            ViewBag.VisitorLeagueId = new SelectList(db.Leagues.OrderBy(l => l.Name), "LeagueId", "Name");//Esto espor que ya hau una liga seleccionada
            ViewBag.VisitorId = new SelectList(db.Teams.OrderBy(t => t.Name).Where(t => t.LeagueId == db.Leagues.FirstOrDefault().LeagueId).OrderBy(t => t.Name), "LeagueId", "Name");//aqui tomo solo el team de la liga seleccionada

            ViewBag.TournamentGroupId = new SelectList(db.TournamentGroups.Where(tg => tg.TournamentId.Equals(date.TournamentId)).OrderBy(tg=>tg.Name), "TournamentGroupId", "Name");

            var view = new MatchView()
            {
                DateId =  date.DateId,
            };

            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateMatch(MatchView  view)
        {
            if (ModelState.IsValid)
            {
                

                view.StatusId = 1;
                view.DateTime = Convert.ToDateTime($"{view.DateString} {view.TimeString}");

                var match = ToMach(view);

                db.Matches.Add(match);

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception)
                {

                    
                }

                return RedirectToAction($"DetailsDate/{match.DateId}");
            }

            var date = await db.Dates.FindAsync(view.DateId);

            ViewBag.LocalLeagueId = new SelectList(db.Leagues.OrderBy(l => l.Name), "LeagueId", "Name", view.LocalId);//Esto espor que ya hau una liga seleccionada
            ViewBag.LocalId = new SelectList(db.Teams.Where(t => t.LeagueId.Equals(view.LocalLeagueId)).OrderBy(t => t.Name), "LeagueId", "Name", view.LocalId);//aqui tomo solo el team de la liga seleccionada

            ViewBag.VisitorLeagueId = new SelectList(db.Leagues.OrderBy(l => l.Name), "LeagueId", "Name", view.VisitorId);//Esto espor que ya hau una liga seleccionada
            ViewBag.VisitorId = new SelectList(db.Teams.Where(t => t.LeagueId.Equals(view.VisitorLeagueId)).OrderBy(t => t.Name), "LeagueId", "Name", view.VisitorId);//aqui tomo solo el team de la liga seleccionada

            ViewBag.TournamentGroupId = new SelectList(db.TournamentGroups.Where(tg => tg.TournamentId.Equals(date.DateId)).OrderBy(tg=>tg.Name), "TournamentGroupId", "Name");

            return View(view);
        }

        private Match ToMach(MatchView view)
        {
            return  new Match()
            {
                DateTime = view.DateTime,
                DateId = view.DateId,
                LocalId = view.LocalId,
                VisitorId = view.VisitorId,
               StatusId = view.StatusId,
               TournamentGroupId = view.TournamentGroupId,

            };
        }

        public async Task<ActionResult> DetailsDate(int? id)
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

        public async Task<ActionResult> CreateTeam(int? id)
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


            ViewBag.LeagueId = new SelectList(db.Leagues.OrderBy(l => l.Name), "LeagueId", "Name");

            ViewBag.TeamId = new SelectList(db.Teams.OrderBy(t => t.Name).Where(t => t.LeagueId == db.Leagues.FirstOrDefault().LeagueId), "TeamId", "Name");

            var view = new TournamentTeamView()
            {
                TournamentGroupId = tournamentGroup.TournamentGroupId,
            };
              
            return View(view);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTeam(TournamentTeamView view)
        {
            if (ModelState.IsValid)
            {
                var tournamentTeam = ToTournamentTeam(view);

                db.TournamentTeams.Add(tournamentTeam);

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception)
                {


                }


                return RedirectToAction($"DetailsTournamentGroup/{tournamentTeam.TournamentGroupId}");
            }

            ViewBag.LeagueId = new SelectList(db.Leagues.OrderBy(l => l.Name), "LeagueId", "Name", view.LeagueId);//Esto espor que ya hau una liga seleccionada

            ViewBag.TeamId = new SelectList(db.Teams.OrderBy(t => t.Name).Where(t => t.LeagueId == view.LeagueId).OrderBy(t => t.Name), "TeamId", "Name", view.LeagueId);//aqui tomo solo el team de la liga seleccionada


            return View(view);
        }

        public async Task<ActionResult> EditTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

           var tournamentTeam = await db.TournamentTeams.FindAsync(id);

            if (tournamentTeam == null)
            {
                return HttpNotFound();
            }

            ViewBag.LeagueId = new SelectList(db.Leagues.OrderBy(l => l.Name), "LeagueId", "Name", tournamentTeam.Team.LeagueId);//Esto espor que ya hau una liga seleccionada

            ViewBag.TeamId = new SelectList(db.Teams.OrderBy(t => t.Name).Where(t => t.LeagueId == tournamentTeam.Team.LeagueId).OrderBy(t => t.Name), "TeamId", "Name", tournamentTeam.Team.LeagueId);//aqui tomo solo el team de la liga seleccionada

            var view = ToView(tournamentTeam);

            return View(view);
        }

        public async Task<ActionResult> DeleteTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tournamentTeam = await db.TournamentTeams.FindAsync(id);

            if (tournamentTeam == null)
            {
                return HttpNotFound();
            }

             db.TournamentTeams.Remove(tournamentTeam);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                
            }

            return RedirectToAction($"DetailsTournamentGroup/{tournamentTeam.TournamentGroupId}");
        }

        private TournamentTeamView ToView(TournamentTeam tournamentTeam)
        {
            return  new TournamentTeamView()
            {
                LeagueId = tournamentTeam.Team.LeagueId,
                Team = tournamentTeam.Team,
                TournamentGroup = tournamentTeam.TournamentGroup,
                Position = tournamentTeam.Position,
                TeamId = tournamentTeam.TeamId,
                TournamentGroupId = tournamentTeam.TournamentGroupId,
                TournamentTeamId = tournamentTeam.TournamentTeamId,
                AgainstGoals = tournamentTeam.AgainstGoals,
                FavorGoals = tournamentTeam.FavorGoals,
                MatchesLost = tournamentTeam.MatchesLost,
                MatchesPlayed = tournamentTeam.MatchesPlayed,
                MatchesTied = tournamentTeam.MatchesTied,
                MatchesWon = tournamentTeam.MatchesWon,
                Points = tournamentTeam.Points,
                
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditTeam(TournamentTeamView view)
        {
            if (ModelState.IsValid)
            {

                var tournamentTeam = ToTournamentTeam(view);

                db.Entry(tournamentTeam).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception)
                {

                   
                }

                return RedirectToAction($"DetailsTournamentGroup/{tournamentTeam.TournamentGroupId}");
            }

            ViewBag.LeagueId = new SelectList(db.Leagues.OrderBy(l => l.Name), "LeagueId", "Name",view.LeagueId);//Esto espor que ya hau una liga seleccionada

            ViewBag.TeamId  = new SelectList(db.Teams.OrderBy(t => t.Name).Where(t => t.LeagueId == view.LeagueId).OrderBy(t =>t.Name), "TeamId", "Name", view.LeagueId);//aqui tomo solo el team de la liga seleccionada

            return View(view);
        }

        private TournamentTeam ToTournamentTeam(TournamentTeamView view)
        {
            return  new TournamentTeam()
            {
               
                Team = view.Team,
                TournamentGroup = view.TournamentGroup,
                Position = view.Position,
                TeamId = view.TeamId,
                TournamentGroupId = view.TournamentGroupId,
                TournamentTeamId = view.TournamentTeamId,
                AgainstGoals = view.AgainstGoals,
                FavorGoals = view.FavorGoals,
                MatchesLost = view.MatchesLost,
                MatchesPlayed = view.MatchesPlayed,
                MatchesTied = view.MatchesTied,
                MatchesWon = view.MatchesWon,
                Points = view.Points,
            };
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
