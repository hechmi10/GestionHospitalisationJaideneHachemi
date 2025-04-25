using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionHospitalisation.Models
{
    public class Hospitalisation
    {
        public int NumServ { get; set; }
        public int CodePat { get; set; }
        [Key]
        public DateTime DateEntree { get; set; }
        public DateTime DateSortie { get; set; }
        public double Frais { get; set; }
        public virtual Service Service { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
