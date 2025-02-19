namespace Healthhub_Online.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LichKham")]
    public partial class LichKham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LichKham()
        {
            DanhGias = new HashSet<DanhGia>();
        }

        [Key]
        public int IDLichKham { get; set; }

        [StringLength(100)]
        public string ChuDe { get; set; }

        [StringLength(300)]
        public string MoTa { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? BatDau { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? KetThuc { get; set; }

        public int? TrangThai { get; set; }

        [StringLength(100)]
        public string ZoomInfo { get; set; }

        [StringLength(200)]
        public string KetQuaKham { get; set; }

        public int? IDNguoiDung { get; set; }

        public int? IDQuanTri { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DanhGia> DanhGias { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }

        public virtual QuanTri QuanTri { get; set; }
    }
}
