namespace QuanLyDiemSinhVien.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LopHoc")]
    public partial class LopHoc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LopHoc()
        {
            Lop_SinhVien = new HashSet<Lop_SinhVien>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LopHocID { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Tên lớp")]
        public string TenLop { get; set; }

        public int KhoaID { get; set; }

        public int KhoaHocID { get; set; }

        public int? GiangVienID { get; set; }

        public virtual Khoa Khoa { get; set; }

        public virtual KhoaHoc KhoaHoc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Lop_SinhVien> Lop_SinhVien { get; set; }
    }
}
