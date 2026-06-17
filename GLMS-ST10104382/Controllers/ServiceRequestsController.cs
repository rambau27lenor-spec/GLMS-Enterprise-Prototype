using GLMS_ST10104382.Data;
using GLMS_ST10104382.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GLMS_ST10104382.Services;

namespace GLMS_ST10104382.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CurrencyService _currencyService;

        public ServiceRequestsController(ApplicationDbContext context, CurrencyService currencyService)
        {
            _context = context;
            _currencyService = currencyService;
        }
        public async Task<IActionResult> Index()
        {
            var requests = _context.ServiceRequests
                .Include(s => s.Contract)
                .ThenInclude(c => c.Client);

            return View(await requests.ToListAsync());
        }

        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest serviceRequest)
        {
            var contract = await _context.Contracts.FindAsync(serviceRequest.ContractId);

            if (contract == null)
            {
                ModelState.AddModelError("ContractId", "Selected contract does not exist.");
            }
            else if (contract.Status == "Expired" || contract.Status == "On Hold")
            {
                ModelState.AddModelError("ContractId", "Service requests can only be created for active or draft contracts.");
            }

            var rate = await _currencyService.GetUsdToZarRateAsync();
            serviceRequest.ConvertedCostZar = _currencyService.ConvertUsdToZar(serviceRequest.Cost, rate); 

            if (ModelState.IsValid)
            {
                _context.Add(serviceRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns(serviceRequest.ContractId);
            return View(serviceRequest);
        }

        private void LoadDropdowns(int? selectedContractId = null)
        {
            ViewData["ContractId"] = new SelectList(
     _context.Contracts
         .Include(c => c.Client)
         .Select(c => new
         {
             c.Id,
             DisplayText = c.Id + " - " + c.Client.Name + " - " + c.Status
         }),
     "Id",
     "DisplayText",
     selectedContractId
 
            );
        }
    }
}