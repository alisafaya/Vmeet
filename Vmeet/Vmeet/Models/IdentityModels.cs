using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vmeet.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public ApplicationUser()
        {
            this.Katilimcis = new HashSet<Katilimci>();
            this.Mesajs = new HashSet<Mesaj>();
        }

        [MaxLength(32)]
        public string Ad { get; set; }

        [MaxLength(32)]
        public string Soyad { get; set; }

        public int? DosyaID { get; set; }

        [ForeignKey("DosyaID")]
        public virtual Dosya Dosya { get; set; }
        public virtual ICollection<Katilimci> Katilimcis { get; set; }
        public virtual ICollection<Mesaj> Mesajs { get; set; }

    }

    public class VmeetDbContext : IdentityDbContext<ApplicationUser>
    {
        public VmeetDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new Utility.VmeetInitializer());
            InitializeDatabase();
        }
        
        public DbSet<Toplanti> Toplantilar { get; set; }
        public DbSet<Dosya> Dosyalar { get; set; }
        public DbSet<Katilimci> Katilimcilar { get; set; }
        public DbSet<Mesaj> Mesajlar { get; set; }
        public DbSet<Link> Linkler { get; set; }
        public DbSet<Giris> Girisler { get; set; }
        public DbSet<Avatar> Avatarlar { get; set; }

        public static VmeetDbContext Create()
        {
            return new VmeetDbContext();
        }

        protected virtual void InitializeDatabase()
        {
            if (!Database.Exists())
            {
                Database.Initialize(true);
            }
        }
    }
}