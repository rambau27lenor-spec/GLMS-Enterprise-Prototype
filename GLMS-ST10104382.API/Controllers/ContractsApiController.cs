using GLMS_ST10104382.Data;
using GLMS_ST10104382.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GLMS_ST10104382.API.Controllers
{
    [Route("api/contracts")]
    [ApiController]
    public class ContractsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContractsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contract>>> GetContracts()
        {
            return await _context.Contracts
               
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contract>> GetContract(int id)
        {
            var contract = await _context.Contracts
                
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null)
            {
                return NotFound();
            }

            return contract;
        }

        [HttpPost]
        public async Task<ActionResult<Contract>> CreateContract(Contract contract)
        {
            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetContract),
                new { id = contract.Id }, contract);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateContractStatus(
            int id,
            [FromBody] string status)
        {
            var contract = await _context.Contracts.FindAsync(id);

            if (contract == null)
            {
                return NotFound();
            }

            contract.Status = status;
            await _context.SaveChangesAsync();

            return Ok(contract);
        }
    }
}