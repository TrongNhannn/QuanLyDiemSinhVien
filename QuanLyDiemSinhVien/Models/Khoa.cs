namespace QuanLyDiemSinhVien.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Khoa")]
    public partial class Khoa
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Khoa()
        {
            GiangViens = new HashSet<GiangVien>();
            LopHocs = new HashSet<LopHoc>();
            NganhDaoTaos = new HashSet<NganhDaoTao>();
            SinhViens = new HashSet<SinhVien>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Khoa")]
        public int KhoaID { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Mã khoa")]
        public string MaKhoa { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Tên khoa")]
        public string TenKhoa { get; set; }

        [StringLength(10)]
        [DisplayName("Ký hiệu")]
        public string KiHieu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiangVien> GiangViens { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LopHoc> LopHocs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NganhDaoTao> NganhDaoTaos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SinhVien> SinhViens { get; set; }
    }
}
