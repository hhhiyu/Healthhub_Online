namespace Healthhub_Online.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TinhThanh")]
    public partial class TinhThanh
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TinhThanh()
        {
            NguoiDungs = new HashSet<NguoiDung>();
        }

        [Key]
        public int IDTinh { get; set; }

        [StringLength(30)]
        public string TenTinh { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NguoiDung> NguoiDungs { get; set; }
    }
}
