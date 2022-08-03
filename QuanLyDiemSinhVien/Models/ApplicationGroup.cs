namespace QuanLyDiemSinhVien.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ApplicationGroup")]
    public partial class ApplicationGroup
    {

        public ApplicationGroup()
        {
            RoleGroups = new HashSet<ApplicationRoleGroup>();
        }

        public ApplicationGroup(string name)
            : this()
        {
            RoleGroups = new HashSet<ApplicationRoleGroup>();
            Name = name;
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<ApplicationRoleGroup> RoleGroups { get; set; }

    }
}
