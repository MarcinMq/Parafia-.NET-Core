using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParafiaApk.Models
{
    public class Parafianin
    {
        [Key]
        public int IdParafianin { get; set; }

        [Required]
        [StringLength(50)]
        public string Imie { get; set; }

        [Required]
        [StringLength(50)]
        public string Nazwisko { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(15)]
        public string? Telefon { get; set; }

        [StringLength(255)]
        public string? Adres { get; set; }

        public string? Parafia { get; set; } // Nowe pole
        public string? Stanowisko { get; set; } // Nowe pole


        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

    }
}
