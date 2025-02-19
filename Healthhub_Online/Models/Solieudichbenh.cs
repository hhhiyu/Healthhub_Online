namespace Healthhub_Online.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Solieudichbenh")]
    public partial class Solieudichbenh
    {
        [Key]
        public int IDThongke { get; set; }

        [StringLength(50)]
        public string Quocgia { get; set; }

        public int? Nam { get; set; }

        [StringLength(50)]
        public string Dichbenh { get; set; }

        public int? Canhiem { get; set; }

        public int? Tuvong { get; set; }
    }
}
