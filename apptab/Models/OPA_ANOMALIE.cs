namespace apptab
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OPA_ANOMALIE
    {
        [Key]
        [Column(TypeName = "numeric")]
        public decimal NUM { get; set; }
        public int? IDSOCIETE { get; set; }
        public string LIBELLE { get; set; }
    }
}
