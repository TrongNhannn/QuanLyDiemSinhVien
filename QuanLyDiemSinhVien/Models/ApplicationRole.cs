namespace QuanLyDiemSinhVien.Models
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ApplicationRole")]
    public partial class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base()
        {

        }

        public ApplicationRole(string name, string description) : base(name)
        {
            this.Description = description;

        }

        public string Description { get; set; }

        [NotMapped]
        public string Discriminator { get; set; }
    }
}
