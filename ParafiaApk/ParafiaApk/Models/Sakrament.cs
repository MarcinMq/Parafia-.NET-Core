using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParafiaApk.Models
{
    public class Sakrament
    {
        [Key]
        public int IdSakrament { get; set; }

        [Required]
        [StringLength(50)]
        public string Rodzaj { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Required]
        [ForeignKey("Parafianin")]
        public int IdParafianin { get; set; }
        public Parafianin? Parafianin { get; set; }

        [Required]
        [ForeignKey("Ksiadz")]
        public int IdKsiadz { get; set; }
        public Ksiadz? Ksiadz { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Niezatwierdzone";
    }
}
