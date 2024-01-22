using apptab.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace apptab
{
    public partial class OPAVITOMATE : DbContext
    {

        public OPAVITOMATE()
            : base(connex)
        {
        }
        public static string connex = "name=OPAVITOMATE";
		public virtual DbSet<MCOMPTA> MCOMPTA { get; set; }
        public virtual DbSet<RJL1> RJL1 { get; set; }
        public virtual DbSet<RPROJET> RPROJET { get; set; }
        public virtual DbSet<RTIERS> RTIERS { get; set; }
        public virtual DbSet<tmp_bulletin> tmp_bulletin { get; set; }
        public virtual DbSet<tpa_BanqueSalaries> tpa_BanqueSalaries { get; set; }
        public virtual DbSet<tpa_salaries> tpa_salaries { get; set; }
        public virtual DbSet<FCOMPTA> FCOMPTA { get; set; }
        public virtual DbSet<FOP> FOP { get; set; }
        public virtual DbSet<MOP> MOP { get; set; }
        public virtual DbSet<OP_CHAINETRAITEMENT> OP_CHAINETRAITEMENT { get; set; }
        public virtual DbSet<RCOMPTATRAIT> RCOMPTATRAIT { get; set; }
        public virtual DbSet<tpa_preparations> tpa_preparations { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MCOMPTA>()
                .Property(e => e.NUMENREG)
                .HasPrecision(30, 0);

            modelBuilder.Entity<MCOMPTA>()
                .Property(e => e.MONTANT)
                .HasPrecision(30, 6);

            modelBuilder.Entity<MCOMPTA>()
                .Property(e => e.MONTDEV)
                .HasPrecision(30, 6);

            modelBuilder.Entity<MCOMPTA>()
                .Property(e => e.MTREPORT)
                .HasPrecision(30, 6);

            modelBuilder.Entity<MCOMPTA>()
                .Property(e => e.NLET)
                .HasPrecision(10, 0);

            modelBuilder.Entity<MCOMPTA>()
                .Property(e => e.NBORD)
                .HasPrecision(30, 6);

            modelBuilder.Entity<MCOMPTA>()
                .Property(e => e.RELEVE)
                .HasPrecision(10, 0);

            modelBuilder.Entity<MCOMPTA>()
                .Property(e => e.ANCIENDRF)
                .HasPrecision(10, 0);

            modelBuilder.Entity<MCOMPTA>()
                .Property(e => e.NOUVDRF)
                .HasPrecision(10, 0);

            modelBuilder.Entity<MCOMPTA>()
                .Property(e => e.MONTEMIS)
                .HasPrecision(30, 6);

            modelBuilder.Entity<MCOMPTA>()
                .Property(e => e.NPRELET)
                .HasPrecision(10, 0);

            modelBuilder.Entity<MCOMPTA>()
                .Property(e => e.QTE)
                .HasPrecision(30, 6);

            modelBuilder.Entity<MCOMPTA>()
                .Property(e => e.PU)
                .HasPrecision(30, 6);

            modelBuilder.Entity<MCOMPTA>()
                .Property(e => e.COURSREP)
                .HasPrecision(18, 6);

            modelBuilder.Entity<MCOMPTA>()
                .Property(e => e.COURSDEV)
                .HasPrecision(18, 6);

            modelBuilder.Entity<MCOMPTA>()
                .Property(e => e.NUMENGAG)
                .HasPrecision(18, 0);

            modelBuilder.Entity<MCOMPTA>()
                .Property(e => e.NOUVDRFCAA)
                .HasPrecision(10, 0);

            modelBuilder.Entity<MCOMPTA>()
                .Property(e => e.IMPORTIDH)
                .HasPrecision(18, 0);

            modelBuilder.Entity<MCOMPTA>()
                .Property(e => e.NUMENREGSITE)
                .HasPrecision(30, 0);

            modelBuilder.Entity<RJL1>()
                .Property(e => e.NUMEROBR)
                .HasPrecision(18, 0);

            modelBuilder.Entity<RJL1>()
                .Property(e => e.INCREMENTATIONAUTO)
                .HasPrecision(10, 0);

            modelBuilder.Entity<RJL1>()
                .HasMany(e => e.MCOMPTA)
                .WithOptional(e => e.RJL1)
                .HasForeignKey(e => e.JL);

            modelBuilder.Entity<RPROJET>()
                .Property(e => e.IDPROJET)
                .HasPrecision(30, 9);

            modelBuilder.Entity<RPROJET>()
                .Property(e => e.NOIMMO)
                .HasPrecision(10, 0);

            modelBuilder.Entity<RPROJET>()
                .Property(e => e.COURSREP)
                .HasPrecision(30, 9);

            modelBuilder.Entity<RPROJET>()
                .Property(e => e.NBJOURMIN1)
                .HasPrecision(10, 0);

            modelBuilder.Entity<RPROJET>()
                .Property(e => e.NBJOURMAX1)
                .HasPrecision(10, 0);

            modelBuilder.Entity<RPROJET>()
                .Property(e => e.NBJOURMIN2)
                .HasPrecision(10, 0);

            modelBuilder.Entity<RPROJET>()
                .Property(e => e.NBJOURMAX2)
                .HasPrecision(10, 0);

            modelBuilder.Entity<RPROJET>()
                .Property(e => e.NBJOURMIN3)
                .HasPrecision(10, 0);

            modelBuilder.Entity<RPROJET>()
                .Property(e => e.NBJOURMAX3)
                .HasPrecision(10, 0);

            modelBuilder.Entity<RPROJET>()
                .Property(e => e.NBJOURMIN4)
                .HasPrecision(10, 0);

            modelBuilder.Entity<RPROJET>()
                .Property(e => e.NBJOURMAX4)
                .HasPrecision(10, 0);

            modelBuilder.Entity<RPROJET>()
                .Property(e => e.NBJOURMIN5)
                .HasPrecision(10, 0);

            modelBuilder.Entity<RPROJET>()
                .Property(e => e.NBJOURMAX5)
                .HasPrecision(10, 0);

            modelBuilder.Entity<RPROJET>()
                .Property(e => e.NIVACTI)
                .HasPrecision(10, 0);

            modelBuilder.Entity<RPROJET>()
                .Property(e => e.INCREMENTAUTOMARCHE)
                .HasPrecision(2, 0);

            modelBuilder.Entity<RPROJET>()
                .Property(e => e.TAUX_TVA)
                .HasPrecision(30, 9);

            modelBuilder.Entity<RPROJET>()
                .Property(e => e.TAUX_AIR)
                .HasPrecision(30, 9);

            modelBuilder.Entity<RPROJET>()
                .Property(e => e.INCREMENTAUTOBC)
                .HasPrecision(2, 0);

            modelBuilder.Entity<RTIERS>()
                .Property(e => e.NBJOURS)
                .HasPrecision(10, 0);

            modelBuilder.Entity<RTIERS>()
                .Property(e => e.JOURREF)
                .HasPrecision(10, 0);

            modelBuilder.Entity<RTIERS>()
                .Property(e => e.IMPORTID)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tmp_bulletin>()
                .Property(e => e._base)
                .HasPrecision(30, 9);

            modelBuilder.Entity<tmp_bulletin>()
                .Property(e => e.taux)
                .HasPrecision(30, 9);

            modelBuilder.Entity<tmp_bulletin>()
                .Property(e => e.gain)
                .HasPrecision(30, 9);

            modelBuilder.Entity<tmp_bulletin>()
                .Property(e => e.retenue)
                .HasPrecision(30, 9);

            modelBuilder.Entity<tmp_bulletin>()
                .Property(e => e.taux1)
                .HasPrecision(30, 9);

            modelBuilder.Entity<tmp_bulletin>()
                .Property(e => e.montant)
                .HasPrecision(30, 9);

            modelBuilder.Entity<tmp_bulletin>()
                .Property(e => e.typeligne)
                .IsFixedLength();

            modelBuilder.Entity<tpa_salaries>()
                .Property(e => e.sexe)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tpa_salaries>()
                .Property(e => e.charge)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tpa_salaries>()
                .Property(e => e.partIR)
                .HasPrecision(18, 5);

            modelBuilder.Entity<tpa_salaries>()
                .Property(e => e.salIndice)
                .HasPrecision(18, 5);

            modelBuilder.Entity<tpa_salaries>()
                .Property(e => e.salaireBase)
                .HasPrecision(18, 5);

            modelBuilder.Entity<tpa_salaries>()
                .Property(e => e.tauxHoraire)
                .HasPrecision(18, 5);
            modelBuilder.Entity<FCOMPTA>()
               .Property(e => e.NUMEROCHEQUE)
               .HasPrecision(18, 0);

            modelBuilder.Entity<FOP>()
                .Property(e => e.MONTANT)
                .HasPrecision(30, 6);

            modelBuilder.Entity<FOP>()
                .Property(e => e.MONTANTTOTALOPAVECOPENCOURSEXO)
                .HasPrecision(30, 6);

            modelBuilder.Entity<FOP>()
                .Property(e => e.MONTANTTOTALOPSANSOPENCOURSEXO)
                .HasPrecision(30, 6);

            modelBuilder.Entity<FOP>()
                .Property(e => e.MONTANTBUDGETDISPOEXO)
                .HasPrecision(30, 6);

            modelBuilder.Entity<FOP>()
                .Property(e => e.MONTANTBUDGETDISPOEXOAVANTOP)
                .HasPrecision(30, 6);

            modelBuilder.Entity<FOP>()
                .Property(e => e.MONTANTBUDGETDISPOPERIODE)
                .HasPrecision(30, 6);

            modelBuilder.Entity<FOP>()
                .Property(e => e.MONTANTBUDGETDISPOPERIODEAVANTOP)
                .HasPrecision(30, 6);

            modelBuilder.Entity<FOP>()
                .Property(e => e.MONTANTBUDGETDISPOSURMARCHE)
                .HasPrecision(30, 6);

            modelBuilder.Entity<FOP>()
                .Property(e => e.MONTANTMARCHEDISPOSUROP)
                .HasPrecision(30, 6);

            modelBuilder.Entity<FOP>()
                .Property(e => e.MONTANTMARCHEDISPOSUROPPERIODE)
                .HasPrecision(30, 6);

            modelBuilder.Entity<FOP>()
                .Property(e => e.MONTANTCOMMISSION)
                .HasPrecision(30, 6);

            modelBuilder.Entity<FOP>()
                .Property(e => e.TAXECOMMISSION)
                .HasPrecision(18, 6);

            modelBuilder.Entity<FOP>()
                .Property(e => e.COURSRAP)
                .HasPrecision(18, 6);

            modelBuilder.Entity<FOP>()
                .Property(e => e.COURSDEV)
                .HasPrecision(18, 6);

            modelBuilder.Entity<FOP>()
                .Property(e => e.MONTANTRAP)
                .HasPrecision(30, 6);

            modelBuilder.Entity<FOP>()
                .Property(e => e.MONTANTDEV)
                .HasPrecision(30, 6);

            modelBuilder.Entity<FOP>()
                .Property(e => e.MONTANTBUDGETEXO)
                .HasPrecision(30, 6);

            modelBuilder.Entity<FOP>()
                .Property(e => e.ENGAGEMENTSANTERIEURS)
                .HasPrecision(30, 6);

            modelBuilder.Entity<FOP>()
                .Property(e => e.MONTANTBUDGETPERIODE)
                .HasPrecision(30, 6);

            modelBuilder.Entity<FOP>()
                .Property(e => e.MONTANTTOTALOPSANSOPENCOURSPERIODE)
                .HasPrecision(30, 6);

            modelBuilder.Entity<FOP>()
                .Property(e => e.MONTANTTOTALOPAVECOPENCOURSPERIODE)
                .HasPrecision(30, 6);

            modelBuilder.Entity<MOP>()
                .Property(e => e.MONTANTLOC)
                .HasPrecision(30, 6);

            modelBuilder.Entity<MOP>()
                .Property(e => e.MONTANTDEV)
                .HasPrecision(30, 6);

            modelBuilder.Entity<MOP>()
                .Property(e => e.MONTANTRAP)
                .HasPrecision(30, 6);

            modelBuilder.Entity<MOP>()
                .Property(e => e.MONTANTTVA)
                .HasPrecision(30, 6);

            modelBuilder.Entity<MOP>()
                .Property(e => e.MONTANTAUTRETAXE)
                .HasPrecision(30, 6);

            modelBuilder.Entity<MOP>()
                .Property(e => e.MONTANTRETENUE)
                .HasPrecision(30, 6);

            modelBuilder.Entity<MOP>()
                .Property(e => e.MONTANTTVADEV)
                .HasPrecision(30, 6);

            modelBuilder.Entity<MOP>()
                .Property(e => e.MONTANTTVARAP)
                .HasPrecision(30, 6);

            modelBuilder.Entity<MOP>()
                .Property(e => e.MONTANTAUTRETAXEDEV)
                .HasPrecision(30, 6);

            modelBuilder.Entity<MOP>()
                .Property(e => e.MONTANTAUTRETAXERAP)
                .HasPrecision(30, 6);

            modelBuilder.Entity<MOP>()
                .Property(e => e.MONTANTRETENUEDEV)
                .HasPrecision(30, 6);

            modelBuilder.Entity<MOP>()
                .Property(e => e.MONTANTRETENUERAP)
                .HasPrecision(30, 6);

            modelBuilder.Entity<MOP>()
                .Property(e => e.MONTANTCOMMISSION)
                .HasPrecision(30, 6);

            modelBuilder.Entity<MOP>()
                .Property(e => e.TAXECOMMISSION)
                .HasPrecision(30, 6);
            modelBuilder.Entity<tpa_preparations>()
                .Property(e => e.numerodordre)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tpa_preparations>()
                .Property(e => e._base)
                .HasPrecision(30, 9);

            modelBuilder.Entity<tpa_preparations>()
                .Property(e => e.taux)
                .HasPrecision(30, 9);

            modelBuilder.Entity<tpa_preparations>()
                .Property(e => e.valeur)
                .HasPrecision(30, 9);
        }
    }
}
