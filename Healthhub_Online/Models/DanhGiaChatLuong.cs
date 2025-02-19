namespace Healthhub_Online.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DanhGiaChatLuong")]
    public partial class DanhGiaChatLuong
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DanhGiaChatLuong()
        {
            DanhGias = new HashSet<DanhGia>();
        }

        [Key]
        public int IDDanhGiaChatLuong { get; set; }

        [Column("DanhGiaChatLuong")]
        [StringLength(10)]
        public string DanhGiaChatLuong1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DanhGia> DanhGias { get; set; }
    }
}
