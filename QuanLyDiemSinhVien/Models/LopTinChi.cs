namespace QuanLyDiemSinhVien.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LopTinChi")]
    public partial class LopTinChi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LopTinChi()
        {
            BangDiems = new HashSet<BangDiem>();
            LopTinChi_SinhVien = new HashSet<LopTinChi_SinhVien>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LopTinChiID { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Mã lớp")]
        public string MaLopTinChi { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Tên lớp")]
        public string TenLopTinChi { get; set; }

        [DisplayName("Số lượng")]
        public int? SoLuongToiDa { get; set; }

        [Required]
        [DisplayName("TG bắt đầu")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? NgayBatDau { get; set; }

        [Required]
        [DisplayName("TG kết thúc")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? NgayKetThuc { get; set; }
        [DisplayName("Giảng viên")]
        public int GiangVienID { get; set; }
        [DisplayName("Môn học")]
        public int MonHocID { get; set; }
        [DisplayName("Ngành đào tạo")]
        public int? NganhDaoTaoID { get; set; }
        [DisplayName("Trạng thái")]
        public bool? KichHoat { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BangDiem> BangDiems { get; set; }

        public virtual MonHoc MonHoc { get; set; }

        public virtual NganhDaoTao NganhDaoTao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LopTinChi_SinhVien> LopTinChi_SinhVien { get; set; }
    }
}
