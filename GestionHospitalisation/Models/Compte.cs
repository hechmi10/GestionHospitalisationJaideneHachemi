using System.ComponentModel.DataAnnotations;

namespace GestionHospitalisation.Models
{
    public class Compte
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Le nom d'utilisateur est requis.")]
        [StringLength(100, ErrorMessage = "Le nom d'utilisateur doit contenir au moins {2} caractères.", MinimumLength = 3)]
        [Display(Name = "Nom d'utilisateur")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Le nom d'utilisateur ne doit contenir que des lettres et des chiffres.")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Le mot de passe doit contenir au moins {2} caractères.", MinimumLength = 6)]
        [Display(Name = "Mot de passe")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{6,}$", ErrorMessage = "Le mot de passe doit contenir au moins une lettre majuscule, une lettre minuscule et un chiffre.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Le rôle est requis.")]
        [StringLength(50, ErrorMessage = "Le rôle doit contenir au moins {2} caractères.", MinimumLength = 3)]
        [Display(Name = "Rôle")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Le rôle ne doit contenir que des lettres.")]
        public string Role { get; set; }

    }
}
