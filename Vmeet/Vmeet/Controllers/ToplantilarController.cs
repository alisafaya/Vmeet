using Microsoft.AspNet.Identity;
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

       

        

        // GET: Toplantilar
        public ActionResult Index()
        {   if(User.Identity.IsAuthenticated)
            {   
                return View("UyeIndex", db.Toplantilar.ToList());
            }
            else
            {
                var model = db.Toplantilar.Where(x => x.OzelMi.Equals(false));
                return View(model.ToList());             
            }
           
        }
        [Authorize]
        public ActionResult UyeIndex()
        {
            return View(db.Toplantilar.ToList());
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
        [Authorize]  //yetkilendirme
        public ActionResult Create()
        {
            return View();
        }

        // POST: Toplantilar/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        // POST: Toplantilar/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,YoneticiID,ToplantiAdi,Konu,BaslamaZamani,BitisZamani,OzelMi")] Toplanti toplanti)
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
        [Authorize]
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
        [Authorize]
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

        public ActionResult CreateLink(int? id,bool? ozel)
        {
            if (id == null || ozel == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ///

            ///
            var link = new Link()
            {
                ToplantiID = id.Value,
                OzelMi = ozel.Value
            };

            db.Linkler.Add(link);
            db.SaveChanges();

            return RedirectToAction("Yonet");
        }
        public ActionResult CreateDavet([Bind(Include = "Ad,Soyad,Email,İzin")] Katilimci uye )
        {
           
            if (true)
            {   

                db.Katilimcilar.Add(uye);
                db.SaveChanges();
               
            }



            return RedirectToAction("Yonet");
        }
        public ActionResult Yonet()
        {
            var model = new YonetViewModel()
            {
                ToplantiId = 2,
                Davetliler = db.Katilimcilar.ToList(),
                Linkler = new List<LinkViewModel>
                {
                    new LinkViewModel("asdascxlak-asdkl-dasd-asd-masld",2,false,2),
                    new LinkViewModel("asdascxlakasdasd-asd-asdklmasld", 2,true,1),
                    new LinkViewModel("asdascklmasld",2,true,2),
                    new LinkViewModel("asdascxlak-asd-asdxczc-klmasld",2,false,3)
                }
            };


            return View(model);
        }

    }
}
