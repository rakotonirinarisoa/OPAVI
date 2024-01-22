namespace apptab
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OPA_CRYPTOHIST
    {
        public int ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CRYPTODATE { get; set; }

        [StringLength(50)]
        public string CRYPTOPWD { get; set; }

        public int? IDSOCIETE { get; set; }

        public int? IDUSER { get; set; }
    }
}
