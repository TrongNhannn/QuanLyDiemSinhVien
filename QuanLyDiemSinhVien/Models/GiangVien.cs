namespace QuanLyDiemSinhVien.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GiangVien")]
    public partial class GiangVien
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Giảng viên")]
        public int GiangVienID { get; set; }

        [StringLength(50)]
        [DisplayName("Mã giảng viên")]
        public string MaGiangVien { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Họ và tên")]
        public string HoTen { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DisplayName("Ngày sinh")]
        public DateTime? NgaySinh { get; set; }
        [DisplayName("Giới tính")]
        public bool? GioiTinh { get; set; }

        [StringLength(200)]
        [DisplayName("Địa chỉ")]
        public string DiaChi { get; set; }

        [StringLength(50)]
        [DisplayName("Số điện thoại")]
        public string SoDienThoai { get; set; }

        [StringLength(50)]
        [DisplayName("Số CCCD")]
        public string SoCMT { get; set; }

        [StringLength(200)]
        [DisplayName("Quê quán")]
        public string QueQuan { get; set; }

        [DisplayName("Khoa")]
        public int? KhoaID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Ngày tạo")]
        public DateTime? NgayTao { get; set; }

        [StringLength(50)]
        [DisplayName("Email")]
        public string Email { get; set; }

        public virtual Khoa Khoa { get; set; }
    }
}
