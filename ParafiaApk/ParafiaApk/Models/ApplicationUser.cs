using Microsoft.AspNetCore.Identity;


namespace ParafiaApk.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Imie { get; set; }
        public string Nazwisko { get; set; }

        public string? Parafia { get; set; }
        public string? Stanowisko{ get; set; }


    }
}
