using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLMS_ST10104382.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Contract")]
        public int ContractId { get; set; }

        public Contract? Contract { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0.01, 1000000)]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Cost in USD")]
        public decimal Cost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Cost in ZAR")]
        public decimal ConvertedCostZar { get; set; }

        [Required]
        public string Status { get; set; } = "Pending";
    }
}
