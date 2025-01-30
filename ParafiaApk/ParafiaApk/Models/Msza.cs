using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParafiaApk.Models
{
    public class Msza
    {
        [Key]
        public int IdMsza { get; set; }

        [Required]
        public DateTime Data { get; set; } = DateTime.UtcNow;

        [Required]
        public TimeSpan Godzina { get; set; }

        [StringLength(50)]
        public string? Rodzaj { get; set; }

        // Nowe pole: Ksiądz odprawiający mszę
        [ForeignKey("Ksiadz")]
        public int? IdKsiadz { get; set; }
        public Ksiadz? Ksiadz { get; set; } // Relacja
    }
}
