namespace Healthhub_Online.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NguoiDung")]
    public partial class NguoiDung
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NguoiDung()
        {
            DanhGias = new HashSet<DanhGia>();
            HoiDaps = new HashSet<HoiDap>();
            LichKhams = new HashSet<LichKham>();
        }

        [Key]
        public int IDNguoiDung { get; set; }

        public string HoTen { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string DienThoai { get; set; }

        public string TaiKhoan { get; set; }

        public string MatKhau { get; set; }

        public int? IDGioiTinh { get; set; }

        public string DiaChiCuThe { get; set; }

        public int? SoCMND { get; set; }

        public int? IDTinh { get; set; }

        public string NhomMau { get; set; }

        public string ThongTinKhac { get; set; }

        public string AnhDaiDien { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DanhGia> DanhGias { get; set; }

        public virtual GioiTinh GioiTinh { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoiDap> HoiDaps { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LichKham> LichKhams { get; set; }

        public virtual TinhThanh TinhThanh { get; set; }
    }
}
