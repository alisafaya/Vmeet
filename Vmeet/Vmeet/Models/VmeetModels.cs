using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Vmeet.Models
{
    public enum Izin { Konusmaci, Dinleyici }

    public enum MesajTuru { Metin, Resim, Dosya }

    public class Dosya
    {
        public Dosya(string dosyaIsmi)
        {
            DosyaKodu = "FILE_" + Guid.NewGuid().ToString("N");
            DosyaIsmi = dosyaIsmi;
        }

        public Dosya() {

        }

        public int ID { get; set;}

        [MaxLength(256)]
        public string DosyaIsmi { get; set; }

        public string DosyaKodu { get; set; }
    }

    public class Toplanti
    {
        public Toplanti()
        {
            this.Katilimcis = new HashSet<Katilimci>();
            this.Mesajs = new HashSet<Mesaj>();
            this.Links = new HashSet<Link>();
        }
        [Key]
        public int ID { get; set; }

        public string YoneticiID { get; set; }

        [ForeignKey("YoneticiID")]
        public virtual ApplicationUser Yonetici { get; set; }

        [Required(ErrorMessage = "Lütfen toplantının adını giriniz.")]
        [StringLength(64, MinimumLength = 3, ErrorMessage = "Toplantının  adı 3-64 karakter arasında olmalıdır.")]
        public string ToplantiAdi { get; set; }

        [Required(ErrorMessage = "Lütfen toplantının konusunu giriniz.")]
        [StringLength(256, ErrorMessage = "Toplantının konusu 256 karakterden uzun olamaz.")]
        public string Konu { get; set; }

        [Required(ErrorMessage = "Lütfen Başlama tarihini giriniz.")]
        //Girilen tarihin, geçerli bir tarih ve saat formatında girilmesini sağlıyoruz.
        [DataType(DataType.DateTime, ErrorMessage = "Lütfen başlama tarihini, doğru bir şekilde giriniz.")]
        public DateTime BaslamaZamani { get; set; }

        [Required(ErrorMessage = "Lütfen Bitiş tarihini giriniz.")]
        [DataType(DataType.DateTime, ErrorMessage = "Lütfen Bitiş tarihini, doğru bir şekilde giriniz.")]
        public DateTime BitisZamani { get; set; }

        [Required(ErrorMessage = "Lütfen Özel mi bilgisini giriniz.")]
        public bool OzelMi { get; set; }

        [StringLength(1024, ErrorMessage = "Çıktı 256 karakterden uzun olamaz.")]
        public string Cikti{ get; set; }

        public virtual ICollection<Katilimci> Katilimcis { get; set; }
        public virtual ICollection<Mesaj> Mesajs { get; set; }
        public virtual ICollection<Link> Links { get; set; }
    }
    public class Katilimci
    {
        [Key]
        public int ID { get; set; }

        public Izin Izin { get; set; }

        public int ToplantiID { get; set; }
        public string ApplicationUserID { get; set; }

        [ForeignKey("ToplantiID")]
        public virtual Toplanti Toplanti { get; set; }

        [ForeignKey("ApplicationUserID")]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
    public class Mesaj
    {
        public int ID { get; set; }

        public string Metin { get; set; }

        public MesajTuru MesajTuru { get; set; }

        public int ToplantiID { get; set; }
        public string ApplicationUserID { get; set; }
        public int? DosyaID { get; set; }

        public DateTime Tarih { get; set; }

        [ForeignKey("ToplantiID")]
        public virtual Toplanti Toplanti { get; set; }

        [ForeignKey("ApplicationUserID")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("DosyaID")]
        public virtual Dosya Dosya { get; set; }
    }

    public class Link
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Lütfen Özel mi bilgisini giriniz.")]
        public bool OzelMi { get; set; }

        public int ToplantiID { get; set; }

        public string Anahtar { get; set; }

        public Link()
        {
            Anahtar = Guid.NewGuid().ToString("N");
            this.Giriss = new HashSet<Giris>();
        }

        [ForeignKey("ToplantiID")]
        public virtual Toplanti Toplanti { get; set; }

        public virtual ICollection<Giris> Giriss { get; set; }

    }
    public class Giris
    {
        public int ID { get; set; }

        public DateTime Tarih {get;set;}

        public int AvatarID { get; set; }

        [ForeignKey("AvatarID")]
        public Avatar Avatar { get; set; }

        [Required(ErrorMessage = "Lütfen  isim giriniz.")]
        [StringLength(256, ErrorMessage = "İsim 256 karakterden uzun olamaz.")]
        public string Isim { get; set; }

        public int LinkID { get; set; }

        [ForeignKey("LinkID")]
        public virtual Link Link { get; set; }
    }

    public class Avatar
    {
        public int ID { get; set; }

        public int DosyaID { get; set; }

        [ForeignKey("DosyaID")]
        public virtual Dosya Dosya { get; set; }
    }

    public class BaslamamisToplantiViewModel
    {
        public string ToplantiAdi { get; set; }
        public string ToplantiKonusu { get; set; }
        public DateTime ToplantiBaslamaZamani { get; set; }
        public TimeSpan ToplantiSuresi { get; set; }
        public string Yonetici { get; set; }
    }

    public class BitmisToplantiViewModel
    {
        public string Yonetici { get; set; }
        public string ToplantiAdi { get; set; }
        public string ToplantiKonusu { get; set; }
        public DateTime ToplantiBitisZamani { get; set; }
        public string ToplantiCiktisi { get; set; }
    }
}