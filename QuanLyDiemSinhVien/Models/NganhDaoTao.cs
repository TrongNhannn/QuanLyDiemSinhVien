namespace QuanLyDiemSinhVien.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NganhDaoTao")]
    public partial class NganhDaoTao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NganhDaoTao()
        {
            LopTinChis = new HashSet<LopTinChi>();
            NganhDaoTao_MonHoc = new HashSet<NganhDaoTao_MonHoc>();
        }
        [DisplayName("Ngành đào tạo")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NganhDaoTaoID { get; set; }

        [StringLength(50)]
        [DisplayName("Tên ngành")]
        public string TenNganh { get; set; }

        public int? KhoaID { get; set; }

        public virtual Khoa Khoa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LopTinChi> LopTinChis { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NganhDaoTao_MonHoc> NganhDaoTao_MonHoc { get; set; }
    }
}
