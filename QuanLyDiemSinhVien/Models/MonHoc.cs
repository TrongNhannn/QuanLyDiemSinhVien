namespace QuanLyDiemSinhVien.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MonHoc")]
    public partial class MonHoc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MonHoc()
        {
            LopTinChis = new HashSet<LopTinChi>();
            NganhDaoTao_MonHoc = new HashSet<NganhDaoTao_MonHoc>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MonHocID { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("Mã môn học")]
        public string MaMonHoc { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("Tên môn học")]
        public string TenMonHoc { get; set; }
        [Required]
        [DisplayName("Số tín chỉ")]
        public int? SoTinChi { get; set; }
        [DisplayName("Loại môn học")]
        public int? LoaiMonHoc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LopTinChi> LopTinChis { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NganhDaoTao_MonHoc> NganhDaoTao_MonHoc { get; set; }
    }
}
