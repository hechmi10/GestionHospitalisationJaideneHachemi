using System.ComponentModel.DataAnnotations;

namespace GestionHospitalisation.Models
{
    public class Patient
    {
        [Key]
        public int CodePat { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Le nom doit contenir au moins {2} caractères.", MinimumLength = 3)]
        [Display(Name = "Nom")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Le nom ne doit contenir que des lettres.")]
        public string Nom { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Le prénom doit contenir au moins {2} caractères.", MinimumLength = 3)]
        [Display(Name = "Prénom")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Le prénom ne doit contenir que des lettres.")]
        public string Prenom { get; set; }
        [Required]
        [Display(Name = "Date de Naissance")]
        [DataType(DataType.Date)]
        public DateTime DateNaiss { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "Le numéro de téléphone doit contenir au moins {2} caractères.", MinimumLength = 3)]
        public string Mutuelle { get; set; }
    }
}
