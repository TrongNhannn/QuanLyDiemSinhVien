namespace QuanLyDiemSinhVien.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NganhDaoTao_MonHoc
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? NganhDaoTaoID { get; set; }

        public int? MonHocID { get; set; }

        public virtual MonHoc MonHoc { get; set; }

        public virtual NganhDaoTao NganhDaoTao { get; set; }
    }
}
