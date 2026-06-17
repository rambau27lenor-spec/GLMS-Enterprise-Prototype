using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLMS_ST10104382.Models
{
    public class Contract
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Client")]
        public int ClientId { get; set; }

        public Client? Client { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required]
        public string Status { get; set; } = "Draft";

        [Required]
        [Display(Name = "Service Level")]
        public string ServiceLevel { get; set; }

        [Display(Name = "Signed Agreement")]
        public string? SignedAgreementPath { get; set; }

        public ICollection<ServiceRequest>? ServiceRequests { get; set; }
        
    }
}
