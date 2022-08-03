namespace QuanLyDiemSinhVien.Models
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ApplicationUserClaim")]
    public partial class ApplicationUserClaim : IdentityUserClaim
    {

        public virtual ApplicationUser User { get; set; }

    }
}
