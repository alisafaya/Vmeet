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
        List<string> imageExt = new List<string> { "bmp", "jpeg", "jpg", "gif", "tiff", "png" };
        VmeetDbContext db = new VmeetDbContext();

        private readonly static Dictionary<string, HashSet<string>> groupsHolder = new Dictionary<string, HashSet<string>>();
        private readonly static Dictionary<string, string> connectionToSession = new Dictionary<string, string>();
        private readonly static Dictionary<int, string> channels = new Dictionary<int, string>();
        private readonly static Dictionary<int, List<string>> listeners = new Dictionary<int, List<string>>();
        private static int ChannelId = 0;

        public override Task OnConnected()
        {
            this.Clients.Client(Context.ConnectionId).getSessionInfo();
            return base.OnConnected();
        }

        public void Katil(int toplantiID, string sessionId)
        {
            this.Groups.Add(Context.ConnectionId, toplantiID.ToString());
            if (!groupsHolder.Keys.Contains(toplantiID.ToString()))
            {
                groupsHolder.Add(toplantiID.ToString(), new HashSet<string>());
            }
            groupsHolder[toplantiID.ToString()].Add(Context.ConnectionId);
            if (!connectionToSession.ContainsKey(Context.ConnectionId))
            {
                if (Context.User.Identity.IsAuthenticated)
                {
                    connectionToSession.Add(Context.ConnectionId, Context.User.Identity.GetUserId());
                }
                else
                {
                    connectionToSession.Add(Context.ConnectionId, sessionId);
                }
            }
            this.Clients.Group(toplantiID.ToString()).triggerRefreshList();
        }

        public override Task OnReconnected()
        {
            this.Clients.Client(Context.ConnectionId).getSessionInfo();
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var connection = this.Context.ConnectionId;
            var toplantilar = new List<string>();
            foreach (var key in groupsHolder.Keys)
            {
                if (groupsHolder[key].Contains(connection))
                {
                    groupsHolder[key].Remove(connection);
                    this.Groups.Remove(connection, key);
                    toplantilar.Add(key);
                }
            }
            if (connectionToSession.ContainsKey(connection))
            {
                connectionToSession.Remove(connection);
            }
            foreach (var item in toplantilar.Distinct())
            {
                this.Clients.Group(item).triggerRefreshList();
            }
            return base.OnConnected();
        }

        public void Speak(int ChannelId, float[] buffer)
        {
            var connection = Context.ConnectionId;

            if (!channels.ContainsKey(ChannelId) || channels[ChannelId] != connection || buffer == null  || !listeners.ContainsKey(ChannelId))
            {
                return;
            }

            foreach (var client in listeners[ChannelId])
                Clients.Client(client).play(ChannelId, buffer);

        }

        public void StartSpeaking(int ToplantiId)
        {
            var connection = Context.ConnectionId;

            if (!groupsHolder.ContainsKey(ToplantiId.ToString()) || !groupsHolder[ToplantiId.ToString()].Contains(connection) || !Context.User.Identity.IsAuthenticated)
            {
                return;
            }
            var userID = Context.User.Identity.GetUserId();
            var katilimci = db.Katilimcilar.Where(x => x.ApplicationUserID == userID && x.ToplantiID == ToplantiId).FirstOrDefault();
            if ((katilimci != null && katilimci.Izin == Izin.Konusmaci) || db.Toplantilar.Find(ToplantiId).YoneticiID == userID)
            {
                channels.Add(++ChannelId, connection);
                //add on client side
                Clients.Client(connection).defineChannelId(ChannelId);
                Clients.Group(ToplantiId.ToString(), connection).addSpeakerChannel(ChannelId,db.Users.Find(userID).Ad);
            }
        }

        public void StopSpeaking(int ToplantiId, int ChannelId)
        {
            var connection = Context.ConnectionId;

            if (!groupsHolder.ContainsKey(ToplantiId.ToString()) || !groupsHolder[ToplantiId.ToString()].Contains(connection) || !Context.User.Identity.IsAuthenticated)
            {
                return;
            }
            var userID = Context.User.Identity.GetUserId();
            var katilimci = db.Katilimcilar.Where(x => x.ApplicationUserID == userID && x.ToplantiID == ToplantiId).FirstOrDefault();
            if ((katilimci != null && katilimci.Izin == Izin.Konusmaci) || db.Toplantilar.Find(ToplantiId).YoneticiID == userID)
            {
                if (channels.ContainsKey(ChannelId))
                {
                    channels.Remove(ChannelId);
                    if (listeners.ContainsKey(ChannelId))
                    {
                        listeners.Remove(ChannelId);
                    }
                    Clients.Group(ToplantiId.ToString(), connection).removeSpeakerChannel(ChannelId);
                }
            }
        }

        public void Dinle(int ToplantiId, int ChannelId)
        {
            var connection = Context.ConnectionId;

            if (!groupsHolder.ContainsKey(ToplantiId.ToString()) || !groupsHolder[ToplantiId.ToString()].Contains(connection) || !channels.ContainsKey(ChannelId))
            {
                return;
            }

            if (!listeners.ContainsKey(ChannelId))
            {
                listeners.Add(ChannelId, new List<string>());
            }
            listeners[ChannelId].Add(connection);

        }

        public void Durdur(int ToplantiId, int ChannelId)
        {
            var connection = Context.ConnectionId;

            if (!groupsHolder.ContainsKey(ToplantiId.ToString()) || !groupsHolder[ToplantiId.ToString()].Contains(connection) || !channels.ContainsKey(ChannelId))
                return;

            if (!listeners.ContainsKey(ChannelId))
                return;

            if (listeners[ChannelId].Contains(connection))
            {
                listeners[ChannelId].Remove(connection);
            }

        }

        public void Send(int session, int ToplantiId, string message, bool dosyaVarMi, int dosyaId)
        {
            int Id, profil, img = -1, file = -1;
            string ad, time, msg = message, fileName = "";

            var connection = Context.ConnectionId;

            if (!groupsHolder.ContainsKey(ToplantiId.ToString()) || !groupsHolder[ToplantiId.ToString()].Contains(connection))
            {
                return;
            }

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
                profil = avat == null ? -1 : avat.DosyaID;
                ad = giris.Isim;
                mesaj.GirisID = giris.ID;
            }

            time = mesaj.Tarih.ToString("HH:mm");

            if (dosyaVarMi && db.Dosyalar.Find(dosyaId) != null)
            {
                var dosya = db.Dosyalar.Find(dosyaId);
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
                    Clients.Group(ToplantiId.ToString()).addNewMessageToPage(Id, profil, ad, null, time, msg, null, null);
                    break;
                case MesajTuru.Resim:
                    Clients.Group(ToplantiId.ToString()).addNewMessageToPage(Id, profil, ad, img, time, msg, null, null);
                    break;
                case MesajTuru.Dosya:
                    Clients.Group(ToplantiId.ToString()).addNewMessageToPage(Id, profil, ad, null, time, msg, file, fileName);
                    break;
            }

        }

        public void getConnectedUserList(int ToplantiId)
        {
            var connection = Context.ConnectionId;
            if (!groupsHolder.ContainsKey(ToplantiId.ToString()) || !groupsHolder[ToplantiId.ToString()].Contains(connection))
            {
                return;
            }
            List<string> isimler = new List<string>();
            List<int> izinler = new List<int>();

            foreach (var item in groupsHolder[ToplantiId.ToString()])
            {
                var session = connectionToSession[item];
                try
                {
                    int sessionId = Convert.ToInt32(session);
                    if (sessionId == 0)
                    {
                        throw new FormatException();
                    }
                    var giris = db.Girisler.Find(sessionId);
                    if (giris == null)
                        continue;
                    isimler.Add(giris.Isim);
                    izinler.Add((int)Izin.Dinleyici);
                }
                catch (FormatException)
                {
                    var user = db.Users.Find(session);
                    if (user == null)
                        continue;
                    var katilimci = db.Katilimcilar.Where(x => x.ApplicationUserID == session && x.ToplantiID == ToplantiId).Count() > 0 ?
                        db.Katilimcilar.Where(x => x.ApplicationUserID == session && x.ToplantiID == ToplantiId).First() : null;
                    if (katilimci == null)
                    {
                        if (db.Toplantilar.Find(ToplantiId).YoneticiID == user.Id)
                        {
                            isimler.Add(user.Ad + " " + user.Soyad);
                            izinler.Add((int)2);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        isimler.Add(user.Ad + " " + user.Soyad);
                        izinler.Add((int)katilimci.Izin);
                    }

                }
            }

            this.Clients.Client(connection).RefreshList(isimler, izinler);
        }

    }
}