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
                    Yonetici = toplanti.Yonetici.Ad +" " + toplanti.Yonetici.Soyad,
                    ToplantiAdi =toplanti.ToplantiAdi,
                    ToplantiBaslamaZamani = toplanti.BaslamaZamani,
                    ToplantiKonusu = toplanti.Konu,
                    ToplantiSuresi = (toplanti.BitisZamani - toplanti.BaslamaZamani)
                };
                return View("baslamamis",model);
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
                return View("bitmis",model);
            }
            else
            {
                var model = new ToplantiViewModel()
                {
                    mesajlar = toplanti.Mesajs.ToList(),
                    Yonetici = toplanti.Yonetici.Ad + " " + toplanti.Yonetici.Soyad,
                    ToplantiAdi = toplanti.ToplantiAdi,
                    ToplantiBaslamaZamani = toplanti.BaslamaZamani,
                    ToplantiKonusu = toplanti.Konu
                };
                return View("Toplanti",model);
            }

        }
        public ActionResult Toplanti()
        {
            return View();
        }

        public ActionResult Image(int? dosyaId)
        {
            if (dosyaId != null && db.Dosyalar.Find(dosyaId) != null)
            {
                return File(dy.DosyaGetir(db.Dosyalar.Find(dosyaId)), "image/jpg", "ProfilePhoto.jpg");
            }
            else
            {
                string path = Server.MapPath("..") + Url.Content("~/Content/iTasksTemplate") + "/images/avatar-user-default.png";
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                StreamReader sw = new StreamReader(fs);

                byte[] photo = new byte[fs.Length];
                fs.Read(photo, 0, (int)fs.Length);
                return File(photo, "image/png", "default.png");
            }
        }
    }
}