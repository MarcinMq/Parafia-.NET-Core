using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParafiaApk.Models
{
    public class Intencja
    {
        [Key]
        public int IdIntencja { get; set; }

        public string? Opis { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "oczekujaca";

        public DateTime DataZgloszenia { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey("Msza")]
        public int IdMsza { get; set; }

        [Required]
        [ForeignKey("Parafianin")]
        public int IdParafianin { get; set; }
    }
}
