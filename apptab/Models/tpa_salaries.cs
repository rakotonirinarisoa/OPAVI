namespace apptab
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tpa_salaries
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string code_etablissement { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string matricule { get; set; }

        [StringLength(50)]
        public string nom { get; set; }

        [StringLength(1)]
        public string sexe { get; set; }

        [StringLength(4)]
        public string code_etatcivil { get; set; }

        [StringLength(5)]
        public string nationalite { get; set; }

        public DateTime? dateNaissance { get; set; }

        [StringLength(30)]
        public string lieuNaissance { get; set; }

        [StringLength(40)]
        public string adresse1 { get; set; }

        [StringLength(40)]
        public string adresse2 { get; set; }

        [StringLength(13)]
        public string cin { get; set; }

        [StringLength(20)]
        public string pere { get; set; }

        [StringLength(20)]
        public string mere { get; set; }

        [StringLength(5)]
        public string code_diplome { get; set; }

        [StringLength(10)]
        public string code_classeslegales { get; set; }

        public DateTime? dateEngagement { get; set; }

        public DateTime? dateDebutContrat { get; set; }

        public DateTime? dateFinContrat { get; set; }

        public DateTime? dateSortie { get; set; }

        [StringLength(13)]
        public string carteTravail { get; set; }

        [StringLength(13)]
        public string matriculeFop { get; set; }

        [StringLength(100)]
        public string fonction { get; set; }

        [StringLength(20)]
        public string qualification { get; set; }

        public bool? enDetachement { get; set; }

        public decimal? charge { get; set; }

        public bool? osie { get; set; }

        [StringLength(4)]
        public string code_typescontrat { get; set; }

        public decimal? partIR { get; set; }

        public decimal? salIndice { get; set; }

        [StringLength(50)]
        public string commentaires { get; set; }

        [StringLength(10)]
        public string code_analytique { get; set; }

        [StringLength(10)]
        public string categorie { get; set; }

        [StringLength(10)]
        public string code_geographique { get; set; }

        [StringLength(10)]
        public string code_budgetaire { get; set; }

        [StringLength(10)]
        public string code_cogeAvance { get; set; }

        [StringLength(10)]
        public string code_auxiAvance { get; set; }

        [StringLength(10)]
        public string code_cogePret { get; set; }

        [StringLength(10)]
        public string code_auxiPret { get; set; }

        [StringLength(10)]
        public string code_cogeSalBrut { get; set; }

        [StringLength(10)]
        public string code_cogeSalNet { get; set; }

        [StringLength(10)]
        public string code_auxiSalNet { get; set; }

        [StringLength(2)]
        public string code_conv { get; set; }

        [StringLength(3)]
        public string code_categorie { get; set; }

        [StringLength(3)]
        public string sousCategorie { get; set; }

        [StringLength(15)]
        public string code_plan6 { get; set; }

        public decimal? salaireBase { get; set; }

        public decimal? tauxHoraire { get; set; }

        [StringLength(20)]
        public string code_banque { get; set; }

        [StringLength(30)]
        public string rib_banque { get; set; }

        [StringLength(50)]
        public string code_bulletin { get; set; }

        [StringLength(50)]
        public string code_indice { get; set; }

        [StringLength(3)]
        public string mode { get; set; }

        [Column(TypeName = "image")]
        public byte[] photo { get; set; }

        public int nombredepart { get; set; }

        public bool? expatrie { get; set; }

        public DateTime? DATECRE { get; set; }

        public DateTime? DATEMAJ { get; set; }

        [StringLength(50)]
        public string USERCRE { get; set; }

        [StringLength(50)]
        public string USERMAJ { get; set; }

        [StringLength(255)]
        public string xlibelle1 { get; set; }

        [StringLength(255)]
        public string xlibelle2 { get; set; }

        [StringLength(255)]
        public string xlibelle3 { get; set; }

        [StringLength(255)]
        public string xlibelle4 { get; set; }

        [StringLength(255)]
        public string xlibelle5 { get; set; }

        [StringLength(255)]
        public string xlibelle6 { get; set; }

        [StringLength(255)]
        public string xlibelle7 { get; set; }

        [StringLength(255)]
        public string xlibelle8 { get; set; }

        [StringLength(255)]
        public string xlibelle9 { get; set; }

        [StringLength(255)]
        public string xlibelle10 { get; set; }

        [StringLength(255)]
        public string xdonnee1 { get; set; }

        [StringLength(255)]
        public string xdonnee2 { get; set; }

        [StringLength(255)]
        public string xdonnee3 { get; set; }

        [StringLength(255)]
        public string xdonnee4 { get; set; }

        [StringLength(255)]
        public string xdonnee5 { get; set; }

        [StringLength(255)]
        public string xdonnee6 { get; set; }

        [StringLength(255)]
        public string xdonnee7 { get; set; }

        [StringLength(255)]
        public string xdonnee8 { get; set; }

        [StringLength(255)]
        public string xdonnee9 { get; set; }

        [StringLength(255)]
        public string xdonnee10 { get; set; }
    }
}
