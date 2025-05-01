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
        public string Password { get; set; }
        [Required(ErrorMessage = "Le rôle est requis.")]
        [Display(Name = "Rôle")]
        public Role Role { get; set; }

    }
}
