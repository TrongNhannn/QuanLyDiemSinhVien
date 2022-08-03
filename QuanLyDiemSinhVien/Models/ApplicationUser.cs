namespace QuanLyDiemSinhVien.Models
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Securities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Security.Claims;
    using System.Threading.Tasks;

    [Table("ApplicationUser")]
    public partial class ApplicationUser : IdentityUser
    {

        public ApplicationUser() : base()
        {
            UserGroups = new HashSet<ApplicationUserGroup>();
        }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? GiangVienID { get; set; }

        public int? SinhVienID { get; set; }

        public bool? IsSystemAdmin { get; set; }
     
        public virtual ICollection<ApplicationUserGroup> UserGroups { get; set; }

      
        [NotMapped]
        public string Discriminator { get; set; }

        public ClaimsIdentity GenerateUserIdentity(ApplicationUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = manager.CreateIdentity(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            return Task.FromResult(GenerateUserIdentity(manager));
        }
    }
}
