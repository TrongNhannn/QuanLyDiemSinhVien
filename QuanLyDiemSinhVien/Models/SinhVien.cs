namespace QuanLyDiemSinhVien.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SinhVien")]
    public partial class SinhVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SinhVien()
        {
            Lop_SinhVien = new HashSet<Lop_SinhVien>();
            LopTinChi_SinhVien = new HashSet<LopTinChi_SinhVien>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SinhVienID { get; set; }

        [Required]
        [StringLength(30)]
        [DisplayName("Mã sinh viên")]
        public string MaSinhVien { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Họ và tên")]
        public string HoTen { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("Ngày sinh")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }

        [DisplayName("Giới tính")]
        public bool? GioiTinh { get; set; }
        [DisplayName("Dân tộc")]
        public int? DanToc { get; set; }

        [StringLength(50)]
        [DisplayName("Số CCCD")]
        public string SoCMT { get; set; }

        [StringLength(50)]
        [DisplayName("Tôn gáo")]
        public string TonGiao { get; set; }
        [DisplayName("Tình trạng")]
        public int? TinhTrang { get; set; }

        [StringLength(50)]
        [DisplayName("Điện thoại gia đình")]
        public string SoDTGD { get; set; }

        [StringLength(50)]
        [DisplayName("Điện thoại di động")]
        public string SoDTDiDong { get; set; }

        [StringLength(50)]
        [DisplayName("Email")]
        public string Email { get; set; }

        [StringLength(200)]
        [DisplayName("Địa chỉ")]
        public string DiaChi { get; set; }

        [StringLength(200)]
        [DisplayName("Ghi chú")]
        public string GhiChu { get; set; }
        [DisplayName("Khoa")]
        public int? KhoaID { get; set; }
        [DisplayName("Ngành đạo tạo")]
        public int? NganhDaoTaoID { get; set; }

        public virtual Khoa Khoa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Lop_SinhVien> Lop_SinhVien { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LopTinChi_SinhVien> LopTinChi_SinhVien { get; set; }
    }
}
