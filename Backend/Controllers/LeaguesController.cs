﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Tokenizer;
using Backend.Helpers;
using Backend.Models;
using Domains;

namespace Backend.Controllers
{
    [Authorize]
    public class LeaguesController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: Leagues
        public async Task<ActionResult> Index()
        {
            return View(await db.Leagues.ToListAsync());
        }

        // GET: Leagues/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            League league = await db.Leagues.FindAsync(id);

            if (league == null)
            {
                return HttpNotFound();
            }
            return View(league);
        }

        // GET: Leagues/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Leagues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LeagueView view)
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

                var league = ToLeague(view);

                league.Logo = picture;

                db.Leagues.Add(league);


                try
                {
                    await db.SaveChangesAsync();
                }
                catch 
                {

                   
                }

                return RedirectToAction("Index");
            }

            return View(view);
        }

        private League ToLeague(LeagueView view)
        {
            return  new League
            {
                LeagueId = view.LeagueId,
                Logo =  view.Logo,
                Name = view.Name,
                Teams = view.Teams,

            };
        }


        // GET: Leagues/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            League league = await db.Leagues.FindAsync(id);

            if (league == null)
            {
                return HttpNotFound();
            }

            var view = ToView(league);

            return View(view);
        }

        private LeagueView ToView(League league)
        {
            return new LeagueView()
            {
                Name = league.Name,
                Logo = league.Logo,
                LeagueId =  league.LeagueId,
                Teams = league.Teams,
            };
        }


        // POST: Leagues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LeagueView view)
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

                var league = ToLeague(view);

                league.Logo = picture;

                db.Entry(league).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch
                {


                }
                    
                return RedirectToAction("Index");

            }
            return View(view);
        }

        // GET: Leagues/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            League league = await db.Leagues.FindAsync(id);
            if (league == null)
            {
                return HttpNotFound();
            }
            return View(league);
        }

        // POST: Leagues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            League league = await db.Leagues.FindAsync(id);
            db.Leagues.Remove(league);
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
