using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace GLMS_ST10104382.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Client Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Contact Details")]
        public string ContactDetails { get; set; }

        [Required]
        public string Region { get; set; }

        public ICollection<Contract>? Contracts { get; set; }
    }
}