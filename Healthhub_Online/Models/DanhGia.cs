namespace Healthhub_Online.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DanhGia")]
    public partial class DanhGia
    {
        [Key]
        public int IDDanhGia { get; set; }

        public string NoiDung { get; set; }

        public int? IDDanhGiaChatLuong { get; set; }

        public int? IDNguoiDung { get; set; }

        public int? IDQuanTri { get; set; }

        public int? IDLichKham { get; set; }

        public virtual DanhGiaChatLuong DanhGiaChatLuong { get; set; }

        public virtual LichKham LichKham { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }

        public virtual QuanTri QuanTri { get; set; }
    }
}
