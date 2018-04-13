using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Vmeet.Models;

namespace Vmeet.Controllers
{
    public class ToplantiController : Controller
    {
        VmeetDbContext db = new VmeetDbContext();
        // GET: Toplanti
        public ActionResult Index(int? id)
        {
            if (id == null || db.Toplantilar.Find(id) == null)
            {
                return RedirectToAction("Index", "Toplantilar");
            }
            var toplanti = db.Toplantilar.Find(id);

            if (toplanti.BaslamaZamani > DateTime.Now)
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
                return View("bitmis",model);
            }
            else
            {

                return View();
            }


        }
    }
}