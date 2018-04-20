using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Vmeet.Models;
using Microsoft.AspNet.Identity;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Vmeet.Hubs
{
    public class ChatHub : Hub
    {

        Dictionary<int, List<string>> grouplar = new Dictionary<int, List<string>>();

        List<string> imageExt = new List<string> { "bmp", "jpeg", "gif", "tiff", "png" };
        VmeetDbContext db = new VmeetDbContext();
        public void Hello()
        {
            Clients.All.hello();
        }

        //public override Task OnConnected()
        //{
        //    string ID = Context.ConnectionId;

        //    grouplar.Add(name, Context.ConnectionId);

        //    return base.OnConnected();
        //}

        public void JoinGroup(int toplantiId, int sessionId)
        {
            var toplanti = db.Toplantilar.Find(toplantiId);
            if (toplantiId == null)
            {
                //HATA
            }
            if (toplanti.OzelMi)
            {
                if (Context.User.Identity.IsAuthenticated)
                {
                    var katilimci = db.Katilimcilar.FirstOrDefault(x => x.ToplantiID == toplantiId && x.ApplicationUserID == Context.User.Identity.GetUserId());
                    if (katilimci == null)
                    {
                        //HATA
                    }
                    Groups.Add(Context.ConnectionId, toplanti.ID.ToString());
                }
                else
                {

                }
            }
            else
            {

            }
            
        }

        public void Send(int session, int ToplantiId, string message, bool dosyaVarMi, int dosyaId)
        {
            int Id, profil, img=-1, file=-1;
            string ad, time, msg = message, fileName="";

            Mesaj mesaj = new Mesaj()
            {
                Metin = message,
                MesajTuru = MesajTuru.Metin,
                Tarih = DateTime.Now,
                ToplantiID = ToplantiId,
            };

            if (Context.User.Identity.IsAuthenticated)
            {
                var user = db.Users.Find(Context.User.Identity.GetUserId());
                profil = user.DosyaID ?? -1;
                ad = user.Ad;
                mesaj.ApplicationUserID = user.Id;
            }
            else
            {
                var giris = db.Girisler.Find(session);
                if (giris == null)
                    return;
                var avat = db.Avatarlar.Find(giris.AvatarID);
                profil = avat == null ? -1 : avat.DosyaID ;
                ad = giris.Isim;
                mesaj.GirisID = giris.ID;
            }

            time = mesaj.Tarih.ToString("HH:mm");

            if (dosyaVarMi && db.Dosyalar.Find(dosyaId) != null)
            {
                var dosya  = db.Dosyalar.Find(dosyaId);
                var fileext = dosya.DosyaIsmi.Split('.').Last();

                if (imageExt.Contains(fileext))
                {
                    mesaj.MesajTuru = MesajTuru.Resim;
                    img = dosya.ID;
                }
                else
                {
                    mesaj.MesajTuru = MesajTuru.Dosya;
                    file = dosya.ID;
                    fileName = dosya.DosyaIsmi;
                }
                mesaj.DosyaID = dosya.ID;
            }

            db.Mesajlar.Add(mesaj);
            db.SaveChanges();
            Id = mesaj.ID;
            switch (mesaj.MesajTuru)
            {
                case MesajTuru.Metin:
                    Clients.All.addNewMessageToPage(Id, profil, ad, null, time, msg, null, null);
                    break;
                case MesajTuru.Resim:
                    Clients.All.addNewMessageToPage(Id, profil, ad, img, time, msg, null, null);
                    break;
                case MesajTuru.Dosya:
                    Clients.All.addNewMessageToPage(Id, profil, ad, null, time, msg, file, fileName);
                    break;
            }

        }

    }
}