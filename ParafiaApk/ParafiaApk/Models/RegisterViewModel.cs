using System.ComponentModel.DataAnnotations;

namespace ParafiaApk.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email jest wymagany")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [StringLength(100, ErrorMessage = "Hasło musi mieć co najmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hasła nie są zgodne")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane")]
        [StringLength(50)]
        public string Imie { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [StringLength(50)]
        public string Nazwisko { get; set; }

        [Required(ErrorMessage = "Telefon jest wymagany")]
        [Phone(ErrorMessage = "Niepoprawny format numeru telefonu")]
        public string PhoneNumber { get; set; }

        // Pola opcjonalne
        [Display(Name = "Parafia")]
        public string? Parafia { get; set; }

        [Display(Name = "Stanowisko")]
        public string? Stanowisko { get; set; }
    }
}
