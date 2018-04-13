using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Vmeet.Models;

namespace Vmeet.Utility
{
    public class DosyaYoneticisi
    {
        private VmeetDbContext db;
        public string DosyaYolu { get; set; }

        public DosyaYoneticisi(VmeetDbContext dbContext)
        {
            db = dbContext;
            DosyaYolu = HttpContext.Current.Server.MapPath("~/App_Data/Vmeet_Data");
        }
        public byte[] DosyaGetir(Dosya dosya)
        {
            if (File.Exists(Path.Combine(DosyaYolu,dosya.DosyaKodu)))
            {
                byte[] allBytes = File.ReadAllBytes(Path.Combine(DosyaYolu, dosya.DosyaKodu));
                return allBytes;
            }
            return null;
        }

        public string DosyaYoluGetir(Dosya dosya)
        {
            if (File.Exists(Path.Combine(DosyaYolu, dosya.DosyaKodu)))
            {
                var dosyaYolu = Path.Combine(DosyaYolu, dosya.DosyaKodu);
                return dosyaYolu;
            }
            return null;
        }

        public Dosya DosyaKaydet(byte[] veri, string dosyaIsmi)
        {
            if (veri != null && veri.Length != 0)
            {
                var dosya = new Dosya(dosyaIsmi);
                var dosyaYolu = Path.Combine(DosyaYolu, dosya.DosyaKodu);
                File.WriteAllBytes(dosyaYolu, veri);
                db.Dosyalar.Add(dosya);
                db.SaveChanges();
                return dosya;
            }
            return null;
        }

        public bool DosyaSil(Dosya dosya)
        {
            if (File.Exists(Path.Combine(DosyaYolu, dosya.DosyaKodu)))
            {
                var dosyaYolu = Path.Combine(DosyaYolu, dosya.DosyaKodu);
                File.Delete(Path.Combine(DosyaYolu, dosya.DosyaIsmi));
                db.Dosyalar.Remove(dosya);
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}