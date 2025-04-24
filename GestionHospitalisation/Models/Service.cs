using System.ComponentModel.DataAnnotations;

namespace GestionHospitalisation.Models
{
    public class Service
    {
        [Key]
        public int NumServ { get; set; }
        [Required(ErrorMessage = "Le nom du service est requis.")]
        [StringLength(100, ErrorMessage = "Le nom du service doit contenir au moins {2} caractères.", MinimumLength = 3)]
        [Display(Name = "Nom du service")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Le nom du service ne doit contenir que des lettres.")]
        public string LibServ { get; set; }
    }
}
