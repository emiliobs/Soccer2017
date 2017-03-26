using Backend.Models;
using Domains;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Backend.Helpers;

namespace Backend.Controllers
{
    [Authorize]
    public class TeamsController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: Teams
        public async Task<ActionResult> Index()
        {
            var teams = db.Teams.Include(t => t.League);
            return View(await teams.ToListAsync());
        }

        // GET: Teams/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // GET: Teams/Create
        public ActionResult Create()
        {
            ViewBag.LeagueId = new SelectList(db.Leagues, "LeagueId", "Name");

            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TeamView view)
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

                var team = ToTeam(view);

                team.Logo = picture;


                db.Teams.Add(team);

                try
                {
                    await db.SaveChangesAsync();
                }
                catch 
                {

                    
                }

                return RedirectToAction("Index");
            }

            ViewBag.LeagueId = new SelectList(db.Leagues, "LeagueId", "Name", view.LeagueId);

            return View(view);
        }

        private Team ToTeam(TeamView view)
        {
            return new Team
            {
                Initials = view.Initials,
                League = view.League,
                Name = view.Name,
                Logo = view.Logo,
                LeagueId = view.LeagueId,
                TeamId = view.TeamId,
            };
        }

        // GET: Teams/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Team team = await db.Teams.FindAsync(id);

          

            if (team == null)
            {
                return HttpNotFound();
            }

            

            ViewBag.LeagueId = new SelectList(db.Leagues, "LeagueId", "Name", team.LeagueId);

            var view = ToView(team);

            return View(view);
        }

        private TeamView ToView(Team team)
        {
           return new TeamView()
           {
               League = team.League,
               Name = team.Name,
               Logo = team.Logo,
               LeagueId = team.LeagueId,
               TeamId = team.TeamId,
               Initials = team.Initials,
           };
        }


        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TeamView view)
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

                var team = ToTeam(view);

                team.Logo = picture;


                db.Entry(team).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch 
                {

                   
                }

                return RedirectToAction("Index");
            }

            ViewBag.LeagueId = new SelectList(db.Leagues, "LeagueId", "Name", view.LeagueId);

            return View(view);
        }

        // GET: Teams/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Team team = await db.Teams.FindAsync(id);
            db.Teams.Remove(team);
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
