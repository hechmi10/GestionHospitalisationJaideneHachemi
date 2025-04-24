using System.ComponentModel.DataAnnotations;

namespace GestionHospitalisation.Models
{
    public class Service
    {
        [Key]
        public int NumServ { get; set; }
        public string LibServ { get; set; }
    }
}
