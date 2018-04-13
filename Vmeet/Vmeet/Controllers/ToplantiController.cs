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
        // GET: Toplanti
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (/*toplanti daha baslamamissa*/false)
            {
                var model = new BaslamamisToplantiViewModel()
                {
                    Yonetici = "ali_safaya",
                    ToplantiAdi = "Toplanti 001",
                    ToplantiBaslamaZamani = DateTime.Now,
                    ToplantiKonusu = "Bu Toplantida Sed pharetra non sollicitudin nec libero curabitur dapibus ex non viverra scelerisque arcu nisl dignissim enim in lacinia dolor libero id urna duis sodales dignissim enim.",
                    ToplantiSuresi = TimeSpan.FromMinutes(124)
                };
                return View("baslamamis",model);
            }
            else if (/*toplanti bitmisse*/true)
            {
                var model = new BitmisToplantiViewModel()
                {
                    Yonetici = "ali_safaya",
                    ToplantiAdi = "Bitmis Toplanti 002",
                    ToplantiBitisZamani = (DateTime.Now - TimeSpan.FromMinutes(1234)),
                    ToplantiKonusu = "Bu Toplantida nisl dignissim enim in lacinia dolor libero id urna duis sodales dignissim enim.",
                    ToplantiCiktisi = "Sed pharetra non sollicitudin nec libero curabitur \n\t*dapibus ex non viverra Sed pharetra non sollicitudin nec libero \n\t*curabitur dapibus ex non viverra Sed pharetra non sollicitudin nec libero curabitur dapibus ex non viverra Sed pharetra non sollicitudin nec libero curabitur dapibus ex non viverra Sed pharetra non sollicitudin nec libero curabitur dapibus ex non viverra Sed pharetra non sollicitudin nec libero curabitur dapibus ex non viverra"
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