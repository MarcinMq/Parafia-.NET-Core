using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParafiaApk.Models
{
    public class KsiadzMsza
    {
        [Required]
        [ForeignKey("Ksiadz")]
        public int IdKsiadz { get; set; }

        [Required]
        [ForeignKey("Msza")]
        public int IdMsza { get; set; }
    }
}
