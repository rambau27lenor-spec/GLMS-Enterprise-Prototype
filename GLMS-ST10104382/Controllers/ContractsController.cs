using GLMS_ST10104382.Data;
using GLMS_ST10104382.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GLMS_ST10104382.Controllers
{
    public class ContractsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ContractsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index(string? status, DateTime? startDate, DateTime? endDate)
        {
            var contracts = _context.Contracts
                .Include(c => c.Client)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
                contracts = contracts.Where(c => c.Status == status);

            if (startDate.HasValue)
                contracts = contracts.Where(c => c.StartDate >= startDate.Value);

            if (endDate.HasValue)
                contracts = contracts.Where(c => c.EndDate <= endDate.Value);

            ViewBag.Status = status;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            return View(await contracts.ToListAsync());
        }

        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contract contract, IFormFile? signedAgreement)
        {
            if (contract.EndDate < contract.StartDate)
            {
                ModelState.AddModelError("EndDate", "End date cannot be before start date.");
            }

            if (signedAgreement != null)
            {
                if (Path.GetExtension(signedAgreement.FileName).ToLower() != ".pdf")
                {
                    ModelState.AddModelError("SignedAgreementPath", "Only PDF files are allowed.");
                }
            }

            if (ModelState.IsValid)
            {
                if (signedAgreement != null)
                {
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "agreements");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid() + "_" + signedAgreement.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await signedAgreement.CopyToAsync(stream);
                    }

                    contract.SignedAgreementPath = "/agreements/" + uniqueFileName;
                }

                _context.Add(contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns(contract.ClientId);
            return View(contract);
        }

        public async Task<IActionResult> DownloadAgreement(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);

            if (contract == null || string.IsNullOrEmpty(contract.SignedAgreementPath))
                return NotFound();

            string filePath = Path.Combine(_environment.WebRootPath, contract.SignedAgreementPath.TrimStart('/'));

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            return PhysicalFile(filePath, "application/pdf", Path.GetFileName(filePath));
        }

        private void LoadDropdowns(int? selectedClientId = null)
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", selectedClientId);

            ViewBag.StatusOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Draft", Value = "Draft" },
                new SelectListItem { Text = "Active", Value = "Active" },
                new SelectListItem { Text = "Expired", Value = "Expired" },
                new SelectListItem { Text = "On Hold", Value = "On Hold" }
            };

            ViewBag.ServiceLevels = new List<SelectListItem>
            {
                new SelectListItem { Text = "Standard", Value = "Standard" },
                new SelectListItem { Text = "Premium", Value = "Premium" },
                new SelectListItem { Text = "Enterprise", Value = "Enterprise" }
            };
        }
    }
}