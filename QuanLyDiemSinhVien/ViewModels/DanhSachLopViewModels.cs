using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QuanLyDiemSinhVien.ViewModels
{
    public class DanhSachLopViewModels
    {
        public int? BangDiemID { get; set; }
        public int LopTinChiID { get; set; }
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
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }

        [DisplayName("Điểm thành phần")]
        public double? DiemThanhPhan { get; set; }
        [DisplayName("Điểm thi")]
        public double? DiemThi { get; set; }
        [DisplayName("Điểm trung bình")]
        public double? DiemTrungBinh { get; set; }

        [StringLength(10)]
        [DisplayName("Điểm chữ")]
        public string DiemChu { get; set; }
    }
}