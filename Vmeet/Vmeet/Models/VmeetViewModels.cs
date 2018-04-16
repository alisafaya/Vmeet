using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vmeet.Models
{
    public class ToplantiViewModel
    {
        public ICollection<Mesaj> mesajlar { get; set; }
        public string ToplantiAdi { get; set; }
        public string ToplantiKonusu { get; set; }
        public DateTime ToplantiBaslamaZamani { get; set; }
        public string Yonetici { get; set; }
        public int YoneticiProfile { get; set; }
        public int SessionId { get; set; }
        public int ToplantiId { get; set; }
        public int profilResmi { get; set; }
        public string  KullaniciIsmi { get; set; }
    }

    public class BaslamamisToplantiViewModel
    {
        public string ToplantiAdi { get; set; }
        public string ToplantiKonusu { get; set; }
        public DateTime ToplantiBaslamaZamani { get; set; }
        public TimeSpan ToplantiSuresi { get; set; }
        public string Yonetici { get; set; }
    }

    public class ToplantilarViewModel
    {
        public ICollection<Toplanti> Toplantilar { get; set; }
        public ICollection<Avatar> Avatarlar { get; set; }
    }

    public class BitmisToplantiViewModel
    {
        public string Yonetici { get; set; }
        public string ToplantiAdi { get; set; }
        public string ToplantiKonusu { get; set; }
        public DateTime ToplantiBitisZamani { get; set; }
        public string ToplantiCiktisi { get; set; }
    }
    public class YonetViewModel
    {
        public int ToplantiId { get; set; }
        public List<Katilimci> Davetliler { get; set; }
        public List<LinkViewModel> Linkler { get; set; }

    }

    public class LinkViewModel
    {
        public bool ozelMi { get; set; }
        public string link { get; set; }
        public int ID { get; set; }
        public LinkViewModel(string anahtar, int toplantiId, bool ozelMi, int Id)
        {
            this.link = HttpContext.Current.Server.MapPath("~") + "/toplanti/" + toplantiId + "/" + anahtar;
            this.ozelMi = ozelMi;
            this.ID = Id;
        }
    }
}