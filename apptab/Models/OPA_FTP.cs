namespace apptab
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OPA_FTP
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string HOTE { get; set; }

        [StringLength(50)]
        public string IDENTIFIANT { get; set; }

        [StringLength(50)]
        public string FTPPWD { get; set; }

        public string PATH { get; set; }

        public int? IDSOCIETE { get; set; }
    }
}
