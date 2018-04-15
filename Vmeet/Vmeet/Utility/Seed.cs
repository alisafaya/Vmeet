using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.IO;
using Vmeet.Models;
using Vmeet.Utility;

namespace Vmeet.Utility
{
    public class VmeetInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<VmeetDbContext>
    {
        protected override void Seed(VmeetDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            //dosylarin eklenmesi
            string yol = HttpContext.Current.Server.MapPath("~/App_Data/seed");
            DosyaYoneticisi dosyaYoneticisi = new DosyaYoneticisi(context);
            var avatarDosyalari = new List<Dosya>{
                dosyaYoneticisi.DosyaKaydet(File.ReadAllBytes(Path.Combine(yol, "avatar-user-boy.png")), "avatar-user-boy.png"),
                dosyaYoneticisi.DosyaKaydet(File.ReadAllBytes(Path.Combine(yol, "avatar-user-business-man.png")), "avatar-user-business-man.png"),
                dosyaYoneticisi.DosyaKaydet(File.ReadAllBytes(Path.Combine(yol, "avatar-user-coder.png")), "avatar-user-coder.png"),
                dosyaYoneticisi.DosyaKaydet(File.ReadAllBytes(Path.Combine(yol, "avatar-user-default.png")), "avatar-user-default.png"),
                dosyaYoneticisi.DosyaKaydet(File.ReadAllBytes(Path.Combine(yol, "avatar-user-designer.png")), "avatar-user-designer.png"),
                dosyaYoneticisi.DosyaKaydet(File.ReadAllBytes(Path.Combine(yol, "avatar-user-girl.png")), "avatar-user-girl.png"),
                dosyaYoneticisi.DosyaKaydet(File.ReadAllBytes(Path.Combine(yol, "avatar-user-old-lady.png")), "avatar-user-old-lady.png"),
                dosyaYoneticisi.DosyaKaydet(File.ReadAllBytes(Path.Combine(yol, "avatar-user-older-oldman.png")), "avatar-user-older-oldman.png"),
                dosyaYoneticisi.DosyaKaydet(File.ReadAllBytes(Path.Combine(yol, "avatar-user-student.png")), "avatar-user-student.png"),
                dosyaYoneticisi.DosyaKaydet(File.ReadAllBytes(Path.Combine(yol, "avatar-user-teacher.png")), "avatar-user-teacher.png"),
                dosyaYoneticisi.DosyaKaydet(File.ReadAllBytes(Path.Combine(yol, "avatar-user-girl-1.png")), "avatar-user-girl-1.png")
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