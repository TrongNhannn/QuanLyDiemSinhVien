using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuanLyDiemSinhVien.Models
{

    public class ApplicationRoleGroup
    {
        [Required]
        public virtual string RoleId { get; set; }
        [Required]
        public virtual int GroupId { get; set; }

        public virtual ApplicationRole Role { get; set; }
        public virtual ApplicationGroup Group { get; set; }
    }
}