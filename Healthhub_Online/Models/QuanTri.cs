namespace Healthhub_Online.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QuanTri")]
    public partial class QuanTri
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public QuanTri()
        {
            DanhGias = new HashSet<DanhGia>();
            HoiDaps = new HashSet<HoiDap>();
            LichKhams = new HashSet<LichKham>();
        }

        [Key]
        public int IDQuanTri { get; set; }

        [StringLength(50)]
        public string TaiKhoan { get; set; }

        [StringLength(20)]
        public string MatKhau { get; set; }

        public string ThongTinBacSi { get; set; }

        [StringLength(50)]
        public string TrinhDo { get; set; }

        public int? IDKhoa { get; set; }

        public string HoTen { get; set; }

        public int? VaiTro { get; set; }

        [StringLength(50)]
        public string AnhBia { get; set; }

        [StringLength(100)]
        public string ThongtinZoom { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DanhGia> DanhGias { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoiDap> HoiDaps { get; set; }

        public virtual Khoa Khoa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LichKham> LichKhams { get; set; }
    }
}
