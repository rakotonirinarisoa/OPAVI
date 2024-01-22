namespace apptab
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OPA_FTPHIST
    {
        public int ID { get; set; }

        public int? IDUSER { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DATESEND { get; set; }

        public int? IDSOCIETE { get; set; }
    }
}
