using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace apptab.Models
{
	public partial class Model1 : DbContext
	{
		public Model1()
			: base("name=Model1")
		{
		}

		public virtual DbSet<OPA_ANOMALIEBR> OPA_ANOMALIEBR { get; set; }
		public virtual DbSet<OPA_REGLEMENTBR> OPA_REGLEMENTBR { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<OPA_REGLEMENTBR>()
				.Property(e => e.MONTANT)
				.HasPrecision(18, 0);
		}
	}
}
