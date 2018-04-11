using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Vmeet.Models;

namespace Vmeet.Controllers
{
    public class ToplantilarController : Controller
    {
        private VmeetDbContext db = new VmeetDbContext();

        List<Toplanti> toplanti = new List<Toplanti>
            {
            new Toplanti { ToplantiAdi="Yazılım",Konu="Proje",BaslamaZamani=DateTime.Parse("2018-04-11 13:00"),BitisZamani=DateTime.Parse("2018-04-11 14:00")},
            new Toplanti{ToplantiAdi="Yazılım",Konu="Proje2",BaslamaZamani=DateTime.Parse("2018-04-12 15:00"),BitisZamani=DateTime.Parse("2018-04-11 16:00")}
            };

        

        // GET: Toplantilar
        public ActionResult Index()
        {   if(User.Identity.IsAuthenticated)
            {   
                return View("UyeIndex", db.Toplantilar.ToList());
            }
            else
            {
               return View(toplanti.ToList());
               
            }
           
        }
        public ActionResult UyeIndex()
        {
            return View(toplanti.ToList());
        }

        // GET: Toplantilar/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Toplanti toplanti = db.Toplantilar.Find(id);
            if (toplanti == null)
            {
                return HttpNotFound();
            }
            return View(toplanti);
        }

        // GET: Toplantilar/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Toplantilar/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,YoneticiID,ToplantiAdi,Konu,BaslamaZamani,BitisZamani,OzelMi")] Toplanti toplanti)
        {
            if (ModelState.IsValid)
            {
                db.Toplantilar.Add(toplanti);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(toplanti);
        }

        // GET: Toplantilar/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Toplanti toplanti = db.Toplantilar.Find(id);
            if (toplanti == null)
            {
                return HttpNotFound();
            }
            return View(toplanti);
        }

        // POST: Toplantilar/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,YoneticiID,ToplantiAdi,Konu,BaslamaZamani,BitisZamani,OzelMi,Cikti")] Toplanti toplanti)
        {
            if (ModelState.IsValid)
            {
                db.Entry(toplanti).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(toplanti);
        }

        // GET: Toplantilar/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Toplanti toplanti = db.Toplantilar.Find(id);
            if (toplanti == null)
            {
                return HttpNotFound();
            }
            return View(toplanti);
        }

        // POST: Toplantilar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Toplanti toplanti = db.Toplantilar.Find(id);
            db.Toplantilar.Remove(toplanti);
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
