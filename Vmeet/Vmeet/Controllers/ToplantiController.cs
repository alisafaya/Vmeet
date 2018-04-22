using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Vmeet.Models;
using Vmeet.Utility;

namespace Vmeet.Controllers
{
    public class ToplantiController : Controller
    {
        VmeetDbContext db = new VmeetDbContext();
        DosyaYoneticisi dy;
        public ToplantiController()
        {
            dy = new DosyaYoneticisi(db);
        }

        public ActionResult Index(int? id,string key)
        {
            if (id == null || db.Toplantilar.Find(id) == null)
            {
                return RedirectToAction("Index", "Toplantilar");
            }
            var toplanti = db.Toplantilar.Find(id);

            if (toplanti.OzelMi)
            {
                if (string.IsNullOrEmpty(key))
                {
                    var currentUser = User.Identity.GetUserId();
                    if ((!User.Identity.IsAuthenticated) ||( db.Katilimcilar.Where(x=>x.ApplicationUserID == currentUser && x.ToplantiID == toplanti.ID).Count() == 0 && toplanti.YoneticiID != currentUser ) )
                    {
                        return RedirectToAction("Index", "Toplantilar");
                    }
                }
                else
                {
                    var link = db.Linkler.Where(x => x.ToplantiID == toplanti.ID && x.Anahtar == key).FirstOrDefault();
                    if (link == null || (link.OzelMi && link.Giriss.Count > 0) )
                    {
                        return RedirectToAction("Index", "Toplantilar");
                    }
                }
            }
            

            if (toplanti.BaslamaZamani > DateTime.Now)
            {
                var model = new BaslamamisToplantiViewModel()
                {
                    Yonetici = toplanti.Yonetici.Ad + " " + toplanti.Yonetici.Soyad,
                    ToplantiAdi = toplanti.ToplantiAdi,
                    ToplantiBaslamaZamani = toplanti.BaslamaZamani,
                    ToplantiKonusu = toplanti.Konu,
                    ToplantiSuresi = (toplanti.BitisZamani - toplanti.BaslamaZamani)
                };
                return View("baslamamis", model);
            }
            else if (toplanti.BitisZamani < DateTime.Now)
            {
                var model = new BitmisToplantiViewModel()
                {
                    Yonetici = toplanti.Yonetici.Ad + " " + toplanti.Yonetici.Soyad,
                    ToplantiAdi = toplanti.ToplantiAdi,
                    ToplantiBitisZamani = toplanti.BitisZamani,
                    ToplantiKonusu = toplanti.Konu,
                    ToplantiCiktisi = toplanti.Cikti
                };
                return View("bitmis", model);
            }
            else
            {
                if (User.Identity.IsAuthenticated)
                {
                    var user = db.Users.Find(User.Identity.GetUserId());
                    var model = new ToplantiViewModel()
                    {
                        ToplantiId = toplanti.ID,
                        mesajlar = db.Mesajlar.Where(x => x.ToplantiID == toplanti.ID).ToList(),
                        Yonetici = toplanti.Yonetici.Ad + " " + toplanti.Yonetici.Soyad,
                        ToplantiAdi = toplanti.ToplantiAdi,
                        ToplantiBaslamaZamani = toplanti.BaslamaZamani,
                        ToplantiKonusu = toplanti.Konu,
                        KullaniciIsmi = user.Ad + " " + user.Soyad,
                        YoneticiProfile = toplanti.Yonetici.DosyaID.HasValue ? toplanti.Yonetici.DosyaID.Value : -1,
                        profilResmi = user.DosyaID.HasValue ? user.DosyaID.Value : -1
                    };
                    return View("Toplanti", model);
                }
                else
                {
                    var model = new ToplantiyaKatilViewModel()
                    {
                        ToplantiID = toplanti.ID,
                        anahtar= key,
                        Avatarlar = db.Avatarlar.ToList()
                    };
                    return View("Index", model);
                }
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Join()
        {

            var form = Request.Form;

            var TopId = form["toplantiId"];
            var avaId = form["avatar"];
            int toplantiId, avatarId;

            try
            {
                toplantiId = Convert.ToInt32(TopId);
                avatarId = Convert.ToInt32(avaId);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index","Toplantilar");
            }

            var isim = form["isim"];
            if (string.IsNullOrEmpty( isim))
            {
                isim = "Anonymous";
            }

            if ( db.Toplantilar.Find(toplantiId) == null)
            {
                return RedirectToAction("Index", "Toplantilar");
            }

            var toplanti = db.Toplantilar.Find(toplantiId);

            if (User.Identity.IsAuthenticated )
            {
                return RedirectToAction("Index", "Toplantilar");
            }
            if (toplanti.OzelMi && form["anahtar"] != null)
            {
                var anahtar = form["anahtar"];
                var link = db.Linkler.Where(x => x.ToplantiID == toplanti.ID && x.Anahtar ==anahtar).FirstOrDefault();
                if (link == null || (link.OzelMi && link.Giriss.Count > 0))
                {
                    return RedirectToAction("Index", "Toplantilar");
                }
            }
            else if(toplanti.OzelMi)
            {
                return RedirectToAction("Index", "Toplantilar");
            }

            if (toplanti.BaslamaZamani > DateTime.Now)
            {
                var model = new BaslamamisToplantiViewModel()
                {
                    Yonetici = toplanti.Yonetici.Ad + " " + toplanti.Yonetici.Soyad,
                    ToplantiAdi = toplanti.ToplantiAdi,
                    ToplantiBaslamaZamani = toplanti.BaslamaZamani,
                    ToplantiKonusu = toplanti.Konu,
                    ToplantiSuresi = (toplanti.BitisZamani - toplanti.BaslamaZamani)
                };
                return View("baslamamis", model);
            }
            else if (toplanti.BitisZamani < DateTime.Now)
            {
                var model = new BitmisToplantiViewModel()
                {
                    Yonetici = toplanti.Yonetici.Ad + " " + toplanti.Yonetici.Soyad,
                    ToplantiAdi = toplanti.ToplantiAdi,
                    ToplantiBitisZamani = toplanti.BitisZamani,
                    ToplantiKonusu = toplanti.Konu,
                    ToplantiCiktisi = toplanti.Cikti
                };
                return View("bitmis", model);
            }
            else
            {
                var session = new Giris()
                {
                    AvatarID = avatarId,
                    Isim = isim,
                    Tarih= DateTime.Now,
                };
                db.Girisler.Add(session);
                session.Avatar = db.Avatarlar.Find(avatarId);
                db.SaveChanges();
                var model = new ToplantiViewModel()
                {
                    ToplantiId = toplanti.ID,
                    mesajlar = db.Mesajlar.Where(x => x.ToplantiID == toplanti.ID).ToList(),
                    Yonetici = toplanti.Yonetici.Ad + " " + toplanti.Yonetici.Soyad,
                    ToplantiAdi = toplanti.ToplantiAdi,
                    ToplantiBaslamaZamani = toplanti.BaslamaZamani,
                    ToplantiKonusu = toplanti.Konu,
                    SessionId = session.ID,
                    KullaniciIsmi =session.Isim,
                    profilResmi = session.Avatar.DosyaID
                };
                return View("Toplanti", model);
            }

        }

        public ActionResult Toplanti()
        {
            return View();
        }

        //POST : Upload File
        [HttpPost]
        public string UploadFile()
        {
            HttpPostedFileBase file = Request.Files["file"];
            if (file != null)
            {
                MemoryStream ms = new MemoryStream();
                file.InputStream.CopyTo(ms);
                byte[] array = ms.GetBuffer();
                //Dosya yoneticisi kullanimi
                //DosyaYoneticisi dy = new DosyaYoneticisi(db);
                Dosya dosya = dy.DosyaKaydet(array, file.FileName);
                db.SaveChanges();
                return dosya.ID.ToString();
            }
            else
                return null;
        }

        public ActionResult Image(int? dosyaId)
        {
            if (dosyaId != null && db.Dosyalar.Find(dosyaId) != null)
            {
                var dosya = db.Dosyalar.Find(dosyaId);
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

        public ActionResult Avatar(int? avatarId)
        {
            if (avatarId != null && db.Avatarlar.Find(avatarId) != null)
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
        public ActionResult Dosya(int? dosyaId)
        {
            if (dosyaId != null && db.Dosyalar.Find(dosyaId) != null)
            {
                var dosya = db.Dosyalar.Find(dosyaId);
                return File(dy.DosyaGetir(dosya), System.Net.Mime.MediaTypeNames.Application.Octet, dosya.DosyaIsmi);
            }
            else
            {
                return null;
            }
        }
    }
}