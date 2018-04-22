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
        private ApplicationUserManager userManager;

        public ToplantilarController()
        {
            dy = new DosyaYoneticisi(db);
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }

        // GET: Toplantilar
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = User.Identity.GetUserId();
                var ozelToplantilar = db.Katilimcilar.Include(x => x.Toplanti).Where(x => x.ApplicationUserID == currentUser).Select(x=>x.Toplanti);
                var yonettigiToplantilar = db.Toplantilar.Where(x => x.YoneticiID == currentUser);
                var Toplantilar = db.Toplantilar.Where(x => !x.OzelMi).Union(ozelToplantilar);
                Toplantilar = Toplantilar.Union(yonettigiToplantilar);
                Toplantilar = Toplantilar.OrderByDescending(x => x.BaslamaZamani);
                return View("UyeIndex", Toplantilar.ToList());
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
            toplanti.YoneticiID = User.Identity.GetUserId();

            if (ModelState.IsValid)
                {
                    db.Toplantilar.Add(toplanti);
                    db.SaveChanges();
                    return RedirectToAction("Yonet", new { id=toplanti.ID });
                }
   
            return View(toplanti);
        }
        

        // GET: Toplantilar/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            //burada
            Toplanti toplanti = db.Toplantilar.Find(id);
            toplanti.YoneticiID = User.Identity.GetUserId();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
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
        public ActionResult Edit([Bind(Include = "ID,ToplantiAdi,Konu,BaslamaZamani,BitisZamani,OzelMi")] Toplanti toplanti)
        {
            if (db.Toplantilar.Find(toplanti.ID) == null || User.Identity.GetUserId() != db.Toplantilar.Find(toplanti.ID).YoneticiID)
            {
                return RedirectToAction("Index");
            }
                        
            if (ModelState.IsValid)
            {
                db.Toplantilar.Find(toplanti.ID).BaslamaZamani = toplanti.BaslamaZamani;
                db.Toplantilar.Find(toplanti.ID).BitisZamani = toplanti.BitisZamani;
                db.Toplantilar.Find(toplanti.ID).Konu = toplanti.Konu;
                db.Toplantilar.Find(toplanti.ID).ToplantiAdi = toplanti.ToplantiAdi;
                db.Toplantilar.Find(toplanti.ID).OzelMi = toplanti.OzelMi;
                db.Toplantilar.Find(toplanti.ID).Cikti = toplanti.Cikti;
                db.SaveChanges();
                return RedirectToAction("Yonet", new { id = toplanti.ID });
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
            if (toplanti == null || User.Identity.GetUserId() != toplanti.YoneticiID)
            {
                return RedirectToAction("Index");
            }
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
            if (toplanti == null || User.Identity.GetUserId() != toplanti.YoneticiID)
            {
                return RedirectToAction("Index");
            }
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

        [Authorize]
        public ActionResult CreateLink(int? id,bool? ozel)
        {
            if (id == null || ozel == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var toplanti = db.Toplantilar.Find(id);
            
            if (toplanti == null || User.Identity.GetUserId() != db.Toplantilar.Find(id).YoneticiID )
            {
                return RedirectToAction("Index");
            }

            var link = new Link()
            {
                ToplantiID = toplanti.ID,
                OzelMi = ozel.Value
            };

            db.Linkler.Add(link);
            db.SaveChanges();

            return RedirectToAction("Yonet", new { id = toplanti.ID });
        }

        [Authorize]
        public ActionResult DavetSil(int? id)
        {
            if (id == null || db.Katilimcilar.Find(id) == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var davet = db.Katilimcilar.Find(id);
            
            var toplanti = db.Toplantilar.Find(davet.ToplantiID);

            if (toplanti == null || User.Identity.GetUserId() != toplanti.YoneticiID )
            {
                return RedirectToAction("Index");
            }


            db.Katilimcilar.Remove(davet);
            db.SaveChanges();
            return RedirectToAction("Yonet",  new { id= toplanti.ID } );

        }

        [Authorize]
        public ActionResult LinkSil(int? id)
        {
            if (id == null || db.Linkler.Find(id) == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var link = db.Linkler.Find(id);

            var toplanti = db.Toplantilar.Find(link.ToplantiID);

            if (toplanti == null || User.Identity.GetUserId() != toplanti.YoneticiID)
            {
                return RedirectToAction("Index");
            }


            db.Linkler.Remove(link);
            db.SaveChanges();
            return RedirectToAction("Yonet", new { id = toplanti.ID });

        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDavet()
        {
            var form = Request.Form;

            var TopId = form["toplantiId"];
            var mail = form["mail"];
            var konusmaci = form["konusmaciMi"];
            int toplantiId;
            bool konusmaciMi;

            try
            {
                toplantiId = Convert.ToInt32(TopId);
                konusmaciMi = konusmaci == null ? false : true  ;
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }


            if (db.Toplantilar.Find(toplantiId)==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var toplanti = db.Toplantilar.Find(toplantiId);
            if (User.Identity.GetUserId() != toplanti.YoneticiID)
            {
                return RedirectToAction("Index");
            }

            var davetli = userManager.FindByEmail(mail);
            if (davetli == null)
            {
                return RedirectToAction("yonet", new { id = toplantiId, err = "Kullanıcı bulunamadı, Link oluşturup kullanabilirsiniz" });
            }

            if (konusmaciMi && db.Katilimcilar.Where(x=>x.ToplantiID == toplanti.ID && x.Izin == Izin.Konusmaci).Count() >= 3)
            {
                return RedirectToAction("yonet", new { id = toplantiId, err = "En çok 3 konuşmacı ekleyebilirsiniz ." });
            }

            if (db.Katilimcilar.Where(x => x.ToplantiID == toplanti.ID && x.ApplicationUserID == davetli.Id).Count() > 0)
            {
                return RedirectToAction("yonet", new { id = toplantiId, err = "Kullanıcı zaten var ." });
            }

            var davet = new Katilimci { Izin = konusmaciMi ? Izin.Konusmaci : Izin.Dinleyici, ToplantiID = toplanti.ID, ApplicationUserID = davetli.Id };

            db.Katilimcilar.Add(davet);
            db.SaveChanges();

            return RedirectToAction("Yonet",new { id = toplantiId });
        }


        [Authorize]
        public ActionResult Yonet(int? id, string err )
        {
            if (id != null)
            {
                var yonelitecekToplanti = db.Toplantilar.Find(id);
                if (yonelitecekToplanti == null && yonelitecekToplanti.YoneticiID != User.Identity.GetUserId())
                {
                    return RedirectToAction("Index");
                }

                var model = new YonetViewModel()
                {
                    ToplantiId = id.Value,
                    Davetliler = new List<DavetliViewModel>(),
                    Linkler = new List<LinkViewModel>()

                };

                var linkler = db.Linkler.Where(x => x.ToplantiID == yonelitecekToplanti.ID).ToList();
                var katilimcilar = db.Katilimcilar.Include(x => x.ApplicationUser).Where(x => x.ToplantiID == yonelitecekToplanti.ID).ToList();

                foreach (var item in linkler)
                    model.Linkler.Add(new LinkViewModel(item.Anahtar,item.ToplantiID,item.OzelMi,item.ID));

                foreach (var item in katilimcilar)
                {
                    model.Davetliler.Add(new DavetliViewModel
                    {
                        KatilimciAd = item.ApplicationUser.Ad + " " + item.ApplicationUser.Soyad,
                        KatilimciEmail = item.ApplicationUser.Email,
                        KatilimciId = item.ID,
                        KatilimciIzin = item.Izin
                    });
                }
                if (!string.IsNullOrEmpty(err))
                {
                    ViewBag.Err = err ;
                }
                return View(model); 
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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


        protected void Seed(VmeetDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            //dosylarin eklenmesi
            string yol = HttpContext.Server.MapPath("~/App_Data/seed");
            DosyaYoneticisi dosyaYoneticisi = new DosyaYoneticisi(context);
            var avatarDosyalari = new List<Dosya>{
                dosyaYoneticisi.DosyaKaydet(System.IO.File.ReadAllBytes(Path.Combine(yol, "avatar-user-boy.png")), "avatar-user-boy.png"),
                dosyaYoneticisi.DosyaKaydet(System.IO.File.ReadAllBytes(Path.Combine(yol, "avatar-user-business-man.png")), "avatar-user-business-man.png"),
                dosyaYoneticisi.DosyaKaydet(System.IO.File.ReadAllBytes(Path.Combine(yol, "avatar-user-coder.png")), "avatar-user-coder.png"),
                dosyaYoneticisi.DosyaKaydet(System.IO.File.ReadAllBytes(Path.Combine(yol, "avatar-user-default.png")), "avatar-user-default.png"),
                dosyaYoneticisi.DosyaKaydet(System.IO.File.ReadAllBytes(Path.Combine(yol, "avatar-user-designer.png")), "avatar-user-designer.png"),
                dosyaYoneticisi.DosyaKaydet(System.IO.File.ReadAllBytes(Path.Combine(yol, "avatar-user-girl.png")), "avatar-user-girl.png"),
                dosyaYoneticisi.DosyaKaydet(System.IO.File.ReadAllBytes(Path.Combine(yol, "avatar-user-old-lady.png")), "avatar-user-old-lady.png"),
                dosyaYoneticisi.DosyaKaydet(System.IO.File.ReadAllBytes(Path.Combine(yol, "avatar-user-older-oldman.png")), "avatar-user-older-oldman.png"),
                dosyaYoneticisi.DosyaKaydet(System.IO.File.ReadAllBytes(Path.Combine(yol, "avatar-user-student.png")), "avatar-user-student.png"),
                dosyaYoneticisi.DosyaKaydet(System.IO.File.ReadAllBytes(Path.Combine(yol, "avatar-user-teacher.png")), "avatar-user-teacher.png"),
                dosyaYoneticisi.DosyaKaydet(System.IO.File.ReadAllBytes(Path.Combine(yol, "avatar-user-girl-1.png")), "avatar-user-girl-1.png")
            };

            //Kullanici eklenmesi
            var applicationUsers = new List<ApplicationUser>
            {
                new ApplicationUser{UserName="senakilic@hotmail.com", Email="senakilic@hotmail.com",Ad="Sena",EmailConfirmed=true, Soyad="Kilic",PhoneNumber="05065137238", Dosya=avatarDosyalari[5]},
                new ApplicationUser{UserName="rana.uzekmek@gmail.com", Email="rana.uzekmek@gmail.com",Ad="Rana",EmailConfirmed=true, Soyad="Uzekmek",PhoneNumber="05380497867",Dosya=avatarDosyalari[5]},
                new ApplicationUser{UserName="alisafaya@gmail.com", Email="alisafaya@gmail.com",Ad="Ali",EmailConfirmed=true, Soyad="Safaya",PhoneNumber="05369537187",Dosya=avatarDosyalari[2]},
                new ApplicationUser{UserName="hll.aksy29@gmail.com", Email="hll.aksy29@gmail.com",Ad="Hilal",EmailConfirmed=true, Soyad="Aksoy",PhoneNumber="05387285318",Dosya=avatarDosyalari[5]},
                new ApplicationUser{UserName="yucebussra@gmail.com", Email="yucebussra@gmail.com",Ad="Busra",EmailConfirmed=true, Soyad="Yuce",PhoneNumber="05379927033",Dosya=avatarDosyalari[5]}
            };


            foreach (var item in applicationUsers)
            {
                ApplicationUser user = userManager.FindByEmail(item.Email);
                if (user == null)
                {
                    IdentityResult userResult = userManager.Create(item, "1a4kvmeet");

                }
            }
            //context.SaveChanges();

            //Avatar eklenmesi
            var avatarlar = new List<Avatar>();
            for (int i = 1; i <= avatarDosyalari.Count; i++)
            {
                avatarlar.Add(new Avatar() { Dosya = context.Dosyalar.Find(i) });
            }
            avatarlar.ForEach(av => context.Avatarlar.Add(av));
            context.SaveChanges();

            //Toplanti olusturulmasi
            var toplantilar = new List<Toplanti>()
            {
                new Toplanti()
                {
                    ToplantiAdi ="Yazılım süreç planlama",
                    Konu ="Sed pharetra non sollicitudin nec libero curabitur dapibus ex non viverra scelerisque arcu nisl dignissim enim in lacinia dolor libero id urna duis sodales dignissim enim.",
                    Yonetici = userManager.FindByEmail("alisafaya@gmail.com"),
                    BaslamaZamani = DateTime.Parse("2018-04-14 13:00"),
                    BitisZamani = DateTime.Parse("2018-04-14 14:00"),
                    OzelMi = true
                },
                new Toplanti()
                {
                    ToplantiAdi ="İş mülakatı 1",
                    Konu ="libero curabitur dapibus ex non viverr dignissim enim in lacinia dignissim enim.",
                    Yonetici = userManager.FindByEmail("rana.uzekmek@gmail.com"),
                    BaslamaZamani = DateTime.Parse("2018-04-12 10:00"),
                    BitisZamani = DateTime.Parse("2018-04-12 12:00"),
                    OzelMi = true,
                    Cikti = "Interdum integer vestibulum venenatis justo id vulputate mi curabitur ac odio sed diam ullamcorper pulvinar proin tristique odio non suscipit venenatis magna enim convallis arcu vitae blandit turpis"
                },
                new Toplanti()
                {
                    ToplantiAdi ="İş mülakatı 2",
                    Konu ="libero curabitur dapibus ex non viverr dignissim enim in lacinia dignissim enim.",
                    Yonetici = userManager.FindByEmail("rana.uzekmek@gmail.com"),
                    BaslamaZamani = DateTime.Parse("2018-04-12 10:00"),
                    BitisZamani = DateTime.Parse("2018-04-12 12:00"),
                    OzelMi = true,
                    Cikti = "Interdum integer vestibulum venenatis justo id vulputate mi curabitur ac odio sed diam ullamcorper pulvinar proin tristique odio non suscipit venenatis magna enim convallis arcu vitae blandit turpis"
                },
                new Toplanti()
                {
                    ToplantiAdi ="İş mülakatı 3",
                    Konu ="libero curabitur dapibus ex non viverr dignissim enim in lacinia dignissim enim.",
                    Yonetici = userManager.FindByEmail("rana.uzekmek@gmail.com"),
                    BaslamaZamani = DateTime.Parse("2018-04-12 10:00"),
                    BitisZamani = DateTime.Parse("2018-04-12 12:00"),
                    OzelMi = true,
                    Cikti = "Interdum integer vestibulum venenatis justo id vulputate mi curabitur ac odio sed diam ullamcorper pulvinar proin tristique odio non suscipit venenatis magna enim convallis arcu vitae blandit turpis"
                },
                new Toplanti()
                {
                    ToplantiAdi ="1A+4K Takım toplantısı",
                    Konu ="libero curabitur dapibus ex non viverra dignissim enim in lacinia enim.",
                    Yonetici = userManager.FindByEmail("alisafaya@gmail.com"),
                    BaslamaZamani = DateTime.Parse("2018-04-20 13:00"),
                    BitisZamani = DateTime.Parse("2018-04-20 14:00"),
                    OzelMi = true
                },
                new Toplanti()
                {
                    ToplantiAdi ="Git ve versiyon kontrol kullanım avantajları",
                    Konu ="libero curabitur dapibus ex non viverra  dignissim enim in lacinia dignissim enim.",
                    Yonetici = userManager.FindByEmail("rana.uzekmek@gmail.com"),
                    BaslamaZamani = DateTime.Parse("2018-04-10 13:00"),
                    BitisZamani = DateTime.Parse("2018-04-10 14:00"),
                    OzelMi = false,
                    Cikti = "Interdum integer vestibulum venenatis justo id vulputate mi curabitur ac odio sed diam ullamcorper pulvinar proin tristique odio non suscipit venenatis magna enim convallis arcu vitae blandit turpis"
                },
                new Toplanti()
                {
                    ToplantiAdi ="Git ve versiyon kontrol kullanım dezavantajları ",
                    Konu ="libero curabitur dapibus ex non viverra  dignissim enim in lacinia dignissim enim.",
                    Yonetici = userManager.FindByEmail("rana.uzekmek@gmail.com"),
                    BaslamaZamani = DateTime.Parse("2018-04-15 13:00"),
                    BitisZamani = DateTime.Parse("2018-04-15 14:00"),
                    OzelMi = false,
                }
            };

            toplantilar.ForEach(top => context.Toplantilar.Add(top));
            context.SaveChanges();
            //Katilimci eklenmesi
            var Katilimcilar = new List<Katilimci>()
            {
                new Katilimci() { ApplicationUser = userManager.FindByEmail("rana.uzekmek@gmail.com"), ToplantiID=1   ,Izin = Izin.Konusmaci },
                new Katilimci() { ApplicationUser = userManager.FindByEmail("yucebussra@gmail.com"), ToplantiID=1    ,Izin = Izin.Konusmaci },
                new Katilimci() { ApplicationUser = userManager.FindByEmail("hll.aksy29@gmail.com"), ToplantiID= 1   ,Izin = Izin.Dinleyici },
                new Katilimci() { ApplicationUser = userManager.FindByEmail("alisafaya@gmail.com"), ToplantiID= 1  ,Izin = Izin.Konusmaci },
                new Katilimci() { ApplicationUser = userManager.FindByEmail("yucebussra9@gmail.com"), ToplantiID=1   ,Izin = Izin.Konusmaci },
                new Katilimci() { ApplicationUser = userManager.FindByEmail("senakilic@hotmail.com"), ToplantiID= 1  ,Izin = Izin.Konusmaci },
                new Katilimci() { ApplicationUser = userManager.FindByEmail("hll.aksy29@gmail.com"), ToplantiID=1   ,Izin = Izin.Dinleyici },
                new Katilimci() { ApplicationUser = userManager.FindByEmail("yucebussra9@gmail.com"), ToplantiID=1   ,Izin = Izin.Dinleyici },
                new Katilimci() { ApplicationUser = userManager.FindByEmail("senakilic@hotmail.com"), ToplantiID=1  , Izin = Izin.Dinleyici },
                new Katilimci() { ApplicationUser = userManager.FindByEmail("rana.uzekmek@gmail.com"), ToplantiID=1   ,Izin = Izin.Dinleyici }
            };
            Katilimcilar.ForEach(kat => context.Katilimcilar.Add(kat));
            context.SaveChanges();

            //Linkler eklenmesi
            var Linkler = new List<Link>()
            {
                new Link() { ToplantiID = 1 , OzelMi = true  },
                new Link() { ToplantiID = 2 , OzelMi = true  },
                new Link() { ToplantiID = 3 , OzelMi = true  },
                new Link() { ToplantiID = 2 , OzelMi = false  },
                new Link() { ToplantiID = 4 , OzelMi = false  }
            };

            Linkler.ForEach(link => context.Linkler.Add(link));
            context.SaveChanges();

            var mesajlar = new List<Mesaj>()
            {
                new Mesaj()
                {
                     ApplicationUser = userManager.FindByEmail("alisafaya@gmail.com"),
                     MesajTuru = MesajTuru.Metin,
                     Metin = "Merhaba",
                     ToplantiID = 5,
                     Tarih = DateTime.Parse("2018-04-20 13:01")
                },
                new Mesaj()
                {
                     ApplicationUser = userManager.FindByEmail("rana.uzekmek@gmail.com"),
                     MesajTuru = MesajTuru.Metin,
                     Metin = "Merhaba",
                     ToplantiID = 5,
                     Tarih = DateTime.Parse("2018-04-20 13:03")
                },
                new Mesaj()
                {
                     ApplicationUser = userManager.FindByEmail("alisafaya@gmail.com"),
                     MesajTuru = MesajTuru.Metin,
                     Metin = "Nasilsiniz ?",
                     ToplantiID = 5,
                     Tarih = DateTime.Parse("2018-04-20 13:04")
                },
                new Mesaj()
                {
                     ApplicationUser = userManager.FindByEmail("alisafaya@gmail.com"),
                     MesajTuru = MesajTuru.Resim,
                     DosyaID = 4,
                     Metin = "Buna bi bakin",
                     ToplantiID = 5,
                     Tarih = DateTime.Parse("2018-04-20 13:04")
                },
                new Mesaj()
                {
                     ApplicationUser = userManager.FindByEmail("hll.aksy29@gmail.com"),
                     MesajTuru = MesajTuru.Metin,
                     Metin = "axvghsagsv",
                     ToplantiID = 5,
                     Tarih = DateTime.Parse("2018-04-20 13:05")
                },
                new Mesaj()
                {
                     ApplicationUser = userManager.FindByEmail("senakilic@hotmail.com"),
                     MesajTuru = MesajTuru.Metin,
                     Metin = "HA HA HA",
                     ToplantiID = 5,
                     Tarih = DateTime.Parse("2018-04-20 13:07")
                }
            };
            mesajlar.ForEach(msj => context.Mesajlar.Add(msj));
            context.SaveChanges();


        }

    }
}
