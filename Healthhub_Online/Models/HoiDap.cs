namespace Healthhub_Online.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HoiDap")]
    public partial class HoiDap
    {
        [Key]
        public int IDHoidap { get; set; }

        public string CauHoi { get; set; }

        public string TraLoi { get; set; }

        public int? IDNguoiDung { get; set; }

        public int? IDQuanTri { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? NgayGui { get; set; }

        public string GhiChu { get; set; }

        public int? TrangThai { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }

        public virtual QuanTri QuanTri { get; set; }
    }
}
