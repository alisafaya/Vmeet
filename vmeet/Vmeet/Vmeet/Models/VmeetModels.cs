using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Attribute tanımlamaları için.
using System.Linq;
using System.Web;

namespace Vmeet.Models
{
    public class Dosya
    {
        public int Id { get; set;}
        [Required(ErrorMessage = "Lütfen dosyanın ismini giriniz.")]
        [StringLength(256, ErrorMessage = "Dosyanın ismi 256 karakterden uzun olamaz.")]
        public string DosyaIsmi { get; set; }
        [Required(ErrorMessage = "Lütfen dosyanın kodunu giriniz.")]
        [StringLength(256, ErrorMessage = "Dosyanın kodu 256 karakterden uzun olamaz.")]
        public string DosyaKodu { get; set; }
        public virtual List<Mesaj> Mesajs { get; set; } //Foreign key
    }

    public class Toplanti
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Lütfen toplantının adını giriniz.")]
        [StringLength(64, MinimumLength = 3, ErrorMessage = "Toplantının  adı 3-64 karakter arasında olmalıdır.")]
        public string ToplantiAdi { get; set; }
        [Required(ErrorMessage = "Lütfen toplantının konusunu giriniz.")]
        [StringLength(64, ErrorMessage = "Toplantının konusu 64 karakterden uzun olamaz.")]
        public string Konu { get; set; }
        [Required(ErrorMessage = "Lütfen Başlama tarihini giriniz.")]
        //Girilen tarihin, geçerli bir tarih ve saat formatında girilmesini sağlıyoruz.
        [DataType(DataType.DateTime, ErrorMessage = "Lütfen başlama tarihini, doğru bir şekilde giriniz.")]
        public DateTime BaslamaZamani { get; set; }
        [Required(ErrorMessage = "Lütfen Bitiş tarihini giriniz.")]
        [DataType(DataType.DateTime, ErrorMessage = "Lütfen Bitiş tarihini, doğru bir şekilde giriniz.")]
        public DateTime BitisZamani { get; set; }
        [Required(ErrorMessage = "Lütfen Özel mi bilgisini giriniz.")]
        public Boolean OzelMi { get; set; }
        [StringLength(256, ErrorMessage = "Çıktı 256 karakterden uzun olamaz.")]
        public string Cikti{ get; set; }
        //public virtual Uye Yonetici { get; set; }   //Foreign key 
        public virtual List<Katilimci> Katilimcis { get; set; }
        public virtual List<Mesaj> Mesajs { get; set; }
        public virtual List<Link> Links { get; set; }
    }
    public class Katilimci
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Lütfen izini giriniz.")]
        [StringLength(1, ErrorMessage = "Izın 1 karakterden uzun olamaz.")]
        public string Izın { get; set; }
        //public virtual Uye UyeId { get; set; }
        public virtual Toplanti ToplantiId { get; set; }
        

    }
    public class Mesaj
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Lütfen Mesaj türünü giriniz.")]
        public int MesajTuru { get; set; }
        [Required(ErrorMessage = "Lütfen Tarihi giriniz.")]
        [DataType(DataType.DateTime, ErrorMessage = "Lütfen tarihi, doğru bir şekilde giriniz.")]
        public DateTime Tarih { get; set; }
        public virtual Toplanti ToplantiId { get; set; }
        //public virtual Uye UyeId { get; set; }   
        public virtual Dosya Dosya { get; set; }

    }
    public class Link
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Lütfen Özel mi bilgisini giriniz.")]
        public Boolean OzelMi { get; set; }
        public virtual Toplanti ToplantiId { get; set; }
        public virtual Giris GirisId { get; set; }

    }
    public class Giris
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Lütfen Tarihi giriniz.")]
        [DataType(DataType.DateTime, ErrorMessage = "Lütfen tarihi, doğru bir şekilde giriniz.")]
        public DateTime Tarih {get;set;}
        [Required(ErrorMessage = "Lütfen Avatarınızı seçiniz.")]
        [StringLength(256, ErrorMessage = "Avatar 256 karakterden uzun olamaz.")]
        public string Avatar { get; set; }
        [Required(ErrorMessage = "Lütfen  isim giriniz.")]
        [StringLength(256, ErrorMessage = "İsim 256 karakterden uzun olamaz.")]
        public string Isim { get; set; }
        public virtual List<Link> Links { get; set; }


    }
}