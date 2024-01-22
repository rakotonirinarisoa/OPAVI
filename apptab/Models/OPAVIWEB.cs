using apptab.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace apptab
{
    public partial class OPAVIWEB : DbContext
    {
        public OPAVIWEB()
            : base("name=OPAVIWEB")
        {
        }

        public virtual DbSet<OPA_ROLES> OPA_ROLES { get; set; }
        public virtual DbSet<OPA_USERS> OPA_USERS { get; set; }
        public virtual DbSet<OPA_SOCIETES> OPA_SOCIETES { get; set; }
        public virtual DbSet<OPA_CRYPTO> OPA_CRYPTO { get; set; }
        public virtual DbSet<OPA_CRYPTOHIST> OPA_CRYPTOHIST { get; set; }
        public virtual DbSet<OPA_DATABASE> OPA_DATABASE { get; set; }
        public virtual DbSet<OPA_DROITS> OPA_DROITS { get; set; }
        public virtual DbSet<OPA_FTP> OPA_FTP { get; set; }
        public virtual DbSet<OPA_FTPHIST> OPA_FTPHIST { get; set; }
        public virtual DbSet<OPA_MAPPAGES> OPA_MAPPAGES { get; set; }
        public virtual DbSet<OPA_ANOMALIE> OPA_ANOMALIE { get; set; }
        public virtual DbSet<OPA_DONNEURORDRE> OPA_DONNEURORDRE { get; set; }
        public virtual DbSet<OPA_HISTORIQUE> OPA_HISTORIQUE { get; set; }
        public virtual DbSet<OPA_BASE> OPA_BASE { get; set; }
        public virtual DbSet<OPA_REGLEMENT> OPA_REGLEMENT { get; set; }
		public virtual DbSet<OPA_ANOMALIEBR> OPA_ANOMALIEBR { get; set; }
		public virtual DbSet<OPA_REGLEMENTBR> OPA_REGLEMENTBR { get; set; }
		public virtual DbSet<OPA_HISTORIQUEBR> OPA_HISTORIQUEBR { get; set; }
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {   
            modelBuilder.Entity<OPA_ANOMALIE>()
                .Property(e => e.NUM)
                .HasPrecision(18, 0);

            modelBuilder.Entity<OPA_HISTORIQUE>()
                .Property(e => e.NUMENREG)
                .HasPrecision(18, 0);
            modelBuilder.Entity<OPA_REGLEMENT>()
                .Property(e => e.NUM)
                .HasPrecision(18, 0);

            modelBuilder.Entity<OPA_REGLEMENT>()
                .Property(e => e.MONTANT)
                .HasPrecision(18, 0);
			modelBuilder.Entity<OPA_REGLEMENTBR>()
				.Property(e => e.ID)
				.HasPrecision(18, 0);
			modelBuilder.Entity<OPA_REGLEMENTBR>()
				.Property(e => e.MONTANT)
				.HasPrecision(18, 0);
		}
    }
}
