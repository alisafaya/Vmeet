using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Vmeet.Models;
using Vmeet.Utility;

namespace Vmeet.Controllers
{
    public class ToplantilarController : Controller
    {
        private VmeetDbContext db = new VmeetDbContext();
        private DosyaYoneticisi dy;

        public ToplantilarController()
        {
            dy = new DosyaYoneticisi(db);
        }

        // GET: Toplantilar
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {   
                return View("UyeIndex", db.Toplantilar.ToList());
            }
            else
            {
                var model = new ToplantilarViewModel
                {
                    Toplantilar = db.Toplantilar.Where(x => x.OzelMi.Equals(false)).ToList(),
                    Avatarlar = db.Avatarlar.ToList()
                };
                return View(model);             
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

            var toplanti = db.Toplantilar.Find(id);
            if (toplanti == null || User.Identity.IsAuthenticated)
            {
                //hata
            }
            if (User.Identity.GetUserId() != db.Toplantilar.Find(id).YoneticiID )
            {
                //hata
            }

            ///
            var link = new Link()
            {
                ToplantiID = toplanti.ID,
                OzelMi = ozel.Value
            };

            db.Linkler.Add(link);
            db.SaveChanges();

            return RedirectToAction("Yonet");
        }
        public ActionResult CreateDavet(int toplantiId, string mail, bool konusmaciMi )
        {
           
            if (true)
            {   

                //db.Katilimcilar.Add(uye);
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
                Linkler = new List<LinkViewModel>()
                //new List<LinkViewModel>
                //{
                //    new LinkViewModel("asdascxlak-asdkl-dasd-asd-masld",2,false,2),
                //    new LinkViewModel("asdascxlakasdasd-asd-asdklmasld", 2,true,1),
                //    new LinkViewModel("asdascklmasld",2,true,2),
                //    new LinkViewModel("asdascxlak-asd-asdxczc-klmasld",2,false,3)
                //}
            };

            foreach (var item in db.Linkler.ToList())
                model.Linkler.Add(new LinkViewModel(item.Anahtar, item.ToplantiID,item.OzelMi, item.ID));


            return View(model);
        }

        public ActionResult Avatar(int? avatarId)
        {
            if (avatarId != null && db.Dosyalar.Find(avatarId) != null)
            {
                var dosya = db.Avatarlar.Find(avatarId).Dosya;
                return File(dy.DosyaGetir(dosya), "image", dosya.DosyaIsmi);
            }
            else
            {
                string path = System.Web.HttpContext.Current.Server.MapPath("~/Content") + "/images/avatar-user-default.png";
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                StreamReader sw = new StreamReader(fs);
                byte[] photo = new byte[fs.Length];
                fs.Read(photo, 0, (int)fs.Length);
                return File(photo, "image/png", "avatar-user-default.png");
            }
        }

    }
}
