using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParafiaApk.Models
{
    public class Ogloszenie
    {
        [Key]
        public int IdOgloszenie { get; set; }

        [Required]
        [StringLength(100)]
        public string Tytul { get; set; }

        [Required]
        public string Tresc { get; set; }

        public DateTime DataUtworzenia { get; set; } = DateTime.UtcNow;

        [Required]
        [ForeignKey("Ksiadz")]
        public int IdKsiadz { get; set; }
    }
}
