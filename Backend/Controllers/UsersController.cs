using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing.Design;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Backend.Classes;
using Backend.Helpers;
using Backend.Models;
using Domains;

namespace Backend.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: Users
        public async Task<ActionResult> Index()
        {
            var users = db.Users.Include(u => u.FavoriteTeam).Include(u => u.UserType);
            return View(await users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.FavoriteLeagueId = new SelectList(db.Leagues.OrderBy(l => l.Name), "LeagueId", "Name");//Esto espor que ya hau una liga seleccionada
            ViewBag.FavoriteTeamId   = new SelectList(db.Teams, "TeamId", "Name");
            ViewBag.UserTypeId       = new SelectList(db.UserTypes, "UserTypeId", "Name");
           

          

            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserView view)
        {
            if (ModelState.IsValid)
            {
                var picture = string.Empty;
                var folder = "~/Content/Users";

                if (view.PictureFile != null)
                {
                    picture = FileHelper.UploadPhoto(view.PictureFile, folder);
                    picture = $"{folder}/{picture}";
                }

                var user = TuUser(view);
                user.Picture = picture;
                db.Users.Add(user);

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception)
                {

                    
                }
                //aqui creoel usuarioASP = de la clase user
                UsersHelper.CreateUserASP(view.Email, "User", view.Password);
                return RedirectToAction("Index");
            }

            ViewBag.FavoriteLeagueId = new SelectList(db.Leagues.OrderBy(l => l.Name), "LeagueId", "Name");//Esto espor que ya hau una liga seleccionada
            ViewBag.FavoriteTeamId = new SelectList(db.Teams, "TeamId", "Name", view.FavoriteTeamId);
            ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "Name", view.UserTypeId);

           

            return View(view);
        }

        private User TuUser(UserView view)
        {
            return new User()
            {
                Email = view.Email,
                FavoriteTeamId = view.FavoriteTeamId,
                FirstName = view.FirstName,
                LastName = view.LastName,
                NickName = view.NickName,
                Picture = view.Picture,
                UserId = view.UserId,
                UserTypeId = view.UserTypeId,
                Points = view.Points,
                
                
            };
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.FavoriteTeamId = new SelectList(db.Teams, "TeamId", "Name", user.FavoriteTeamId);
            ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "Name", user.UserTypeId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserId,FirstName,LastName,UserTypeId,Picture,Email,NickName,FavoriteTeamId,Points")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.FavoriteTeamId = new SelectList(db.Teams, "TeamId", "Name", user.FavoriteTeamId);
            ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "Name", user.UserTypeId);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User user = await db.Users.FindAsync(id);
            db.Users.Remove(user);
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
