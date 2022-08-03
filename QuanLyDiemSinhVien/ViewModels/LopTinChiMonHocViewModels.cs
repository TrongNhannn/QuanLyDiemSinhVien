using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QuanLyDiemSinhVien.ViewModels
{
    public class LopTinChiMonHocViewModels
    {
        public int GiangVienID { get; set; }
        public int LopTinChiID { get; set; }
        public string MaLopTinChi { get; set; }
        public string TenLopTinChi { get; set; }
        public string TenMonHoc { get; set; }
        public int? SoTinChi { get; set; }
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? NgayBatDau { get; set; }
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? NgayKetThuc { get; set; }
    }
}