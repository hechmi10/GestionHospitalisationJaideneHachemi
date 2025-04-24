using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionHospitalisation.Models
{
    public class Hospitalisation
    {
        
        [ForeignKey("Service")]
        public int NumServ { get; set; }
        [ForeignKey("Patient")]
        public int CodePat { get; set; }
        [Key]
        public DateTime DateEntree { get; set; }
        public DateTime DateSortie { get; set; }
        public double Frais { get; set; }
    }
}
