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

        // GET: Toplanti
        [Authorize]
        public ActionResult Index(int? id)
        {
            //Giris kontrolleri eklenecek
            if (id == null || db.Toplantilar.Find(id) == null)
            {
                return RedirectToAction("Index", "Toplantilar");
            }

            var toplanti = db.Toplantilar.Find(id);

            if (false/*toplanti.BaslamaZamani > DateTime.Now*/)
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
            else if (false/*toplanti.BitisZamani < DateTime.Now*/)
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
                var user = db.Users.Find(User.Identity.GetUserId());
                var model = new ToplantiViewModel()
                {
                    ToplantiId = toplanti.ID,
                    mesajlar = db.Mesajlar.Where(x => x.ToplantiID == toplanti.ID).ToList(),
                    Yonetici = toplanti.Yonetici.Ad + " " + toplanti.Yonetici.Soyad,
                    ToplantiAdi = toplanti.ToplantiAdi,
                    ToplantiBaslamaZamani = toplanti.BaslamaZamani,
                    ToplantiKonusu = toplanti.Konu,
                    SessionId = 5,
                    KullaniciIsmi = user.Ad + " " + user.Soyad
                };
                return View("Toplanti", model);
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

            //Giris kontrolleri eklenecek
            if ( db.Toplantilar.Find(toplantiId) == null)
            {
                return RedirectToAction("Index", "Toplantilar");
            }

            var toplanti = db.Toplantilar.Find(toplantiId);

            if (false/*toplanti.BaslamaZamani > DateTime.Now*/)
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
            else if (false/*toplanti.BitisZamani < DateTime.Now*/)
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