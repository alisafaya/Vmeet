using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Vmeet.Models;



namespace Vmeet.Seed
{
    public class VmeetInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<VmeetDbContext>
    {
        protected override void Seed(VmeetDbContext context)
        {
            var applicationusers = new List<ApplicationUser>
            {
            new ApplicationUser{Email="senakilic@hotmail.com",PasswordHash="123456",Ad="Sena", Soyad="Kilic",PhoneNumber="05065137238"},
            new ApplicationUser{Email="rana.uzekmek@gmail.com",PasswordHash="123456",Ad="Rana", Soyad="Uzekmek",PhoneNumber="05380497867"},
            new ApplicationUser{Email="alisafaya@gmail.com",PasswordHash="123456",Ad="Ali", Soyad="Safaya",PhoneNumber="05369537187"},
            new ApplicationUser{Email="hll.aksy29@gmail.com",PasswordHash="123456",Ad="Hilal", Soyad="Aksoy",PhoneNumber="05387285318"},
            new ApplicationUser{Email="yucebussra@gmail.com",PasswordHash="123456",Ad="Busra", Soyad="Yuce",PhoneNumber="05379927033"}
            };

            applicationusers.ForEach(s => context.Users.Add(s));
            context.SaveChanges();

            var avatar = new List<Avatar>
            {
            new Avatar{},
            new Avatar{},
            new Avatar{},
            new Avatar{},
            new Avatar{}
            };

            applicationusers.ForEach(s => context.Users.Add(s));
            context.SaveChanges();

        }
    }
}