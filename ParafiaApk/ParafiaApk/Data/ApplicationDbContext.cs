using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ParafiaApk.Models;

namespace ParafiaApk.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSety dla tabel w aplikacji
        public DbSet<Ksiadz> Ksiadz { get; set; }
        public DbSet<Parafianin> Parafianin { get; set; }
        public DbSet<Sakrament> Sakrament { get; set; }
        public DbSet<Msza> Msza { get; set; }
        public DbSet<KsiadzMsza> KsiadzMsza { get; set; }
        public DbSet<Intencja> Intencja { get; set; }
        public DbSet<Ogloszenie> Ogloszenie { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfiguracja relacji wiele-do-wielu dla tabeli KsiadzMsza
            modelBuilder.Entity<KsiadzMsza>()
                .HasKey(km => new { km.IdKsiadz, km.IdMsza });

            modelBuilder.Entity<KsiadzMsza>()
                .HasOne<Ksiadz>()
                .WithMany()
                .HasForeignKey(km => km.IdKsiadz);

            modelBuilder.Entity<KsiadzMsza>()
                .HasOne<Msza>()
                .WithMany()
                .HasForeignKey(km => km.IdMsza);

            // Konfiguracja relacji Parafianin -> ApplicationUser
            //modelBuilder.Entity<Parafianin>()
            //    .HasOne(p => p.ApplicationUser)
            //    .WithMany() // Jeśli ApplicationUser nie ma kolekcji Parafianin
            //    .HasForeignKey(p => p.ApplicationUserId)
            //    .OnDelete(DeleteBehavior.Cascade); // Usunięcie użytkownika usuwa powiązany rekord Parafianin
        }
    }
}
