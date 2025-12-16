using DogShelterMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace DogShelterMvc.Data
{
    public class DogShelterDbContext : DbContext
    {
        public DogShelterDbContext(DbContextOptions<DogShelterDbContext> options)
            : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Dog> Dogs { get; set; }
        public DbSet<DogHistory> DogHistories { get; set; }
        public DbSet<DogImage> DogImages { get; set; }
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<Hracka> Hracky { get; set; }
        public DbSet<Quarantine> Quarantines { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<MedicalEquipment> MedicalEquipments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pavilion> Pavilions { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Shelter> Shelters { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<KeyValueUS> KeyValueUS { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Dog entity
            modelBuilder.Entity<Dog>(entity =>
            {
                entity.ToTable("Dogs");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.BodyColor).HasMaxLength(100);
                entity.Property(e => e.DuvodPrijeti).HasMaxLength(500);
                entity.Property(e => e.StavPes).HasMaxLength(100);
                
                entity.HasOne(d => d.Utulek)
                    .WithMany()
                    .HasForeignKey(d => d.UtulekId)
                    .OnDelete(DeleteBehavior.SetNull);
                
                entity.HasOne(d => d.Karantena)
                    .WithMany()
                    .HasForeignKey(d => d.KarantenaId)
                    .OnDelete(DeleteBehavior.SetNull);
                
                entity.HasOne(d => d.Majitel)
                    .WithMany()
                    .HasForeignKey(d => d.MajitelId)
                    .OnDelete(DeleteBehavior.SetNull);
                
                entity.HasOne(d => d.Otec)
                    .WithMany()
                    .HasForeignKey(d => d.OtecId)
                    .OnDelete(DeleteBehavior.SetNull);
                
                entity.HasOne(d => d.Matka)
                    .WithMany()
                    .HasForeignKey(d => d.MatkaId)
                    .OnDelete(DeleteBehavior.SetNull);
                
                entity.HasOne(d => d.DogImage)
                    .WithMany()
                    .HasForeignKey(d => d.ObrazekId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure Owner entity
            modelBuilder.Entity<Owner>(entity =>
            {
                entity.ToTable("Owners");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Surname).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Phone).HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(200);
                
                entity.HasOne(o => o.Adresa)
                    .WithMany()
                    .HasForeignKey(o => o.AddressID)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure Shelter entity
            modelBuilder.Entity<Shelter>(entity =>
            {
                entity.ToTable("Shelters");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Telephone).HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(200);
                
                entity.HasOne(s => s.Adresa)
                    .WithMany()
                    .HasForeignKey(s => s.AddressID)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure Address entity
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Addresses");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Street).IsRequired().HasMaxLength(200);
                entity.Property(e => e.City).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Psc).HasMaxLength(10);
            });

            // Configure Quarantine entity
            modelBuilder.Entity<Quarantine>(entity =>
            {
                entity.ToTable("Quarantines");
                entity.HasKey(e => e.Id);
            });

            // Configure Pavilion entity
            modelBuilder.Entity<Pavilion>(entity =>
            {
                entity.ToTable("Pavilions");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PavName).IsRequired().HasMaxLength(200);
            });

            // Configure DogImage entity
            modelBuilder.Entity<DogImage>(entity =>
            {
                entity.ToTable("DogImages");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FileName).HasMaxLength(500);
                entity.Property(e => e.ImageData).HasColumnType("BLOB");
            });

            // Configure DogHistory entity
            modelBuilder.Entity<DogHistory>(entity =>
            {
                entity.ToTable("DogHistories");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.EventDescription).HasMaxLength(1000);
                
                entity.HasOne(dh => dh.Pes)
                    .WithMany()
                    .HasForeignKey(dh => dh.DogId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure MedicalRecord entity
            modelBuilder.Entity<MedicalRecord>(entity =>
            {
                entity.ToTable("MedicalRecords");
                entity.HasKey(e => e.Id);
                
                entity.HasOne(mr => mr.Dog)
                    .WithMany()
                    .HasForeignKey(mr => mr.DogId)
                    .OnDelete(DeleteBehavior.SetNull);
                
                entity.HasOne(mr => mr.Type)
                    .WithMany()
                    .HasForeignKey(mr => mr.TypeProcId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure Procedure entity
            modelBuilder.Entity<Procedure>(entity =>
            {
                entity.ToTable("Procedures");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ProcName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.DescrName).HasMaxLength(1000);
                
                entity.HasOne(p => p.Record)
                    .WithMany()
                    .HasForeignKey(p => p.ZdrZaznamid)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure Reservation entity
            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.ToTable("Reservations");
                entity.HasKey(e => e.Id);
                
                entity.HasOne(r => r.Pes)
                    .WithMany()
                    .HasForeignKey(r => r.DogId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure Storage entity
            modelBuilder.Entity<Storage>(entity =>
            {
                entity.ToTable("Storages");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Type).HasMaxLength(100);
            });

            // Configure Feed entity
            modelBuilder.Entity<Feed>(entity =>
            {
                entity.ToTable("Feeds");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nazev).IsRequired().HasMaxLength(200);
                
                entity.HasOne(f => f.Sklad)
                    .WithMany()
                    .HasForeignKey(f => f.SkladID)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure Hracka entity
            modelBuilder.Entity<Hracka>(entity =>
            {
                entity.ToTable("Hracky");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nazev).IsRequired().HasMaxLength(200);
                
                entity.HasOne(h => h.Sklad)
                    .WithMany()
                    .HasForeignKey(h => h.SkladID)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure MedicalEquipment entity
            modelBuilder.Entity<MedicalEquipment>(entity =>
            {
                entity.ToTable("MedicalEquipments");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MedicalName).IsRequired().HasMaxLength(200);
                
                entity.HasOne(me => me.Sklad)
                    .WithMany()
                    .HasForeignKey(me => me.SkladID)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Uname).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Hash).IsRequired().HasMaxLength(500);
            });

            // Configure Log entity
            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("Logs");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CUser).HasMaxLength(100);
                entity.Property(e => e.TableName).HasMaxLength(100);
                entity.Property(e => e.Operation).HasMaxLength(50);
                entity.Property(e => e.OldValue).HasColumnType("TEXT");
                entity.Property(e => e.NewValue).HasColumnType("TEXT");
            });

            // Configure KeyValueUS entity
            modelBuilder.Entity<KeyValueUS>(entity =>
            {
                entity.ToTable("KeyValueUS");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nazev).IsRequired().HasMaxLength(200);
            });
        }
    }
}

