using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LutrijaWpfEF.Model
{
    public partial class LutrijaContext : DbContext
    {
        public LutrijaContext()
        {
        }

        public LutrijaContext(DbContextOptions<LutrijaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ImpKlad2> ImpKlad2 { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=data source=192.168.1.213,1433;Network Library=DBMSSOCN;initial catalog=Lutrija;multipleactiveresultsets=True;application name=EntityFramework;User Id=sa;Password=Lutrija1");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ImpKlad2>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("IMP_KLAD2");

                entity.Property(e => e.AdresaObjekta)
                    .HasColumnName("ADRESA_OBJEKTA")
                    .HasMaxLength(50);

                entity.Property(e => e.Blagajna)
                    .HasColumnName("BLAGAJNA")
                    .HasColumnType("decimal(19, 4)");

                entity.Property(e => e.BrObj)
                    .HasColumnName("BR_OBJ")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BrojListica)
                    .HasColumnName("BROJ_LISTICA")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ImePrezime)
                    .HasColumnName("IME_PREZIME")
                    .HasMaxLength(50);

                entity.Property(e => e.Isplata)
                    .HasColumnName("ISPLATA")
                    .HasColumnType("decimal(19, 4)");

                entity.Property(e => e.NakZaPrir)
                    .HasColumnName("NAK_ZA_PRIR")
                    .HasColumnType("decimal(19, 4)");

                entity.Property(e => e.NazivObjekta)
                    .HasColumnName("NAZIV_OBJEKTA")
                    .HasMaxLength(50);

                entity.Property(e => e.OpBrProd)
                    .HasColumnName("OP_BR_PROD")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PlacPorez)
                    .HasColumnName("PLAC_POREZ")
                    .HasColumnType("decimal(19, 4)");

                entity.Property(e => e.Razlika)
                    .HasColumnName("RAZLIKA")
                    .HasColumnType("decimal(19, 4)");

                entity.Property(e => e.Uplata)
                    .HasColumnName("UPLATA")
                    .HasColumnType("decimal(19, 4)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
