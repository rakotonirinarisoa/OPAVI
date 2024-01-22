namespace apptab
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OPA_CRYPTO
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string CRYPTPWD { get; set; }

        public int? IDSOCIETE { get; set; }
    }
}
