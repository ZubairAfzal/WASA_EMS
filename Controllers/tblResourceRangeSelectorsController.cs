using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WASA_EMS.Models;

namespace WASA_EMS.Controllers
{
    public class tblResourceRangeSelectorsController : Controller
    {
        private WASA_EMS_Entities db = new WASA_EMS_Entities();

        // GET: tblResourceRangeSelectors
        public ActionResult Index()
        {
            var tblResourceRangeSelectors = db.tblResourceRangeSelectors.Include(t => t.tblResource);
            return View(tblResourceRangeSelectors.ToList());
        }

        // GET: tblResourceRangeSelectors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblResourceRangeSelector tblResourceRangeSelector = db.tblResourceRangeSelectors.Find(id);
            if (tblResourceRangeSelector == null)
            {
                return HttpNotFound();
            }
            return View(tblResourceRangeSelector);
        }

        // GET: tblResourceRangeSelectors/Create
        public ActionResult Create()
        {
            ViewBag.ResourceID = new SelectList(db.tblResources, "ResourceID", "MobileNumber");
            return View();
        }

        // POST: tblResourceRangeSelectors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RangeID,ResourceID,ParameterID,MinRange,MaxRange,CompanyID")] tblResourceRangeSelector tblResourceRangeSelector)
        {
            if (ModelState.IsValid)
            {
                db.tblResourceRangeSelectors.Add(tblResourceRangeSelector);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ResourceID = new SelectList(db.tblResources, "ResourceID", "MobileNumber", tblResourceRangeSelector.ResourceID);
            return View(tblResourceRangeSelector);
        }

        // GET: tblResourceRangeSelectors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblResourceRangeSelector tblResourceRangeSelector = db.tblResourceRangeSelectors.Find(id);
            if (tblResourceRangeSelector == null)
            {
                return HttpNotFound();
            }
            ViewBag.ResourceID = new SelectList(db.tblResources, "ResourceID", "MobileNumber", tblResourceRangeSelector.ResourceID);
            return View(tblResourceRangeSelector);
        }

        // POST: tblResourceRangeSelectors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RangeID,ResourceID,ParameterID,MinRange,MaxRange,CompanyID")] tblResourceRangeSelector tblResourceRangeSelector)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblResourceRangeSelector).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ResourceID = new SelectList(db.tblResources, "ResourceID", "MobileNumber", tblResourceRangeSelector.ResourceID);
            return View(tblResourceRangeSelector);
        }

        // GET: tblResourceRangeSelectors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblResourceRangeSelector tblResourceRangeSelector = db.tblResourceRangeSelectors.Find(id);
            if (tblResourceRangeSelector == null)
            {
                return HttpNotFound();
            }
            return View(tblResourceRangeSelector);
        }

        // POST: tblResourceRangeSelectors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblResourceRangeSelector tblResourceRangeSelector = db.tblResourceRangeSelectors.Find(id);
            db.tblResourceRangeSelectors.Remove(tblResourceRangeSelector);
            db.SaveChanges();
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
