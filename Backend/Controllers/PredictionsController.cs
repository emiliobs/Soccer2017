using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Backend.Models;
using Domains;

namespace Backend.Controllers
{
    public class PredictionsController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: Predictions
        public async Task<ActionResult> Index()
        {
            var predictions = db.Predictions.Include(p => p.Match).Include(p => p.User);
            return View(await predictions.ToListAsync());
        }

        // GET: Predictions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Predictions predictions = await db.Predictions.FindAsync(id);
            if (predictions == null)
            {
                return HttpNotFound();
            }
            return View(predictions);
        }

        // GET: Predictions/Create
        public ActionResult Create()
        {
            ViewBag.MatchId = new SelectList(db.Matches, "MatchId", "MatchId");
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName");
            return View();
        }

        // POST: Predictions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PredictionId,UserId,MatchId,LocalGoals,VisitorGoals,Points")] Predictions predictions)
        {
            if (ModelState.IsValid)
            {
                db.Predictions.Add(predictions);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MatchId = new SelectList(db.Matches, "MatchId", "MatchId", predictions.MatchId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", predictions.UserId);
            return View(predictions);
        }

        // GET: Predictions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Predictions predictions = await db.Predictions.FindAsync(id);
            if (predictions == null)
            {
                return HttpNotFound();
            }
            ViewBag.MatchId = new SelectList(db.Matches, "MatchId", "MatchId", predictions.MatchId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", predictions.UserId);
            return View(predictions);
        }

        // POST: Predictions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PredictionId,UserId,MatchId,LocalGoals,VisitorGoals,Points")] Predictions predictions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(predictions).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MatchId = new SelectList(db.Matches, "MatchId", "MatchId", predictions.MatchId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", predictions.UserId);
            return View(predictions);
        }

        // GET: Predictions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Predictions predictions = await db.Predictions.FindAsync(id);
            if (predictions == null)
            {
                return HttpNotFound();
            }
            return View(predictions);
        }

        // POST: Predictions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Predictions predictions = await db.Predictions.FindAsync(id);
            db.Predictions.Remove(predictions);
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
