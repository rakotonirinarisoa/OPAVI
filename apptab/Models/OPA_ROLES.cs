namespace apptab
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OPA_ROLES
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string INTITULES { get; set; }
    }

    public enum Role
    {
        SuperAdministrateur, Administrateur, Utilisateur
    }
}
