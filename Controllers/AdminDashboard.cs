using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficePortal.Data;
using OfficePortal.Models;
using OfficePortal.Services;

namespace OfficePortal.Controllers
{
    public class AdminDashboard : Controller
    {
        private readonly ApplicationDbContext _context;
        private LanguageService _localization;

        public AdminDashboard(ApplicationDbContext context, LanguageService localization) 
        {
            _context = context;

            _localization = localization;

        }
        public async Task<IActionResult> HomeAdmin()
        {
            return View(await _context.Announcement.ToListAsync());
        }
        public IActionResult Home()
        {
            return View("HomeAdmin");
        }
        
        public async Task<IActionResult> FormRequestAdminPage()
        {
            var missionAndTrainingForms = await _context.MissionandTrainingForm
                .Select(mt => new FormRequestViewModel
                {
                    id = mt.id, // Assuming Id is the primary key in MissionandTrainingForm
                    EmployeeName = mt.EmployeeName,
                    Department = mt.Department,
                    LineManager = mt.LineManager,
                    Status = mt.status,
                    FormType = mt.Form_type
                })
                .ToListAsync();

            var trainingRequests = await _context.TrainingRequestViewModel
                .Select(tr => new FormRequestViewModel
                {
                    id = tr.id, // Assuming Id is the primary key in TrainingRequestViewModel
                    EmployeeName = tr.EmployeeName,
                    Department = tr.Department,
                    LineManager = tr.LineManager,
                    Status = tr.status,
                    FormType = tr.form_type
                })
                .ToListAsync();

            var result = missionAndTrainingForms.Concat(trainingRequests).ToList();

            return View(result);
        }
        public async Task<IActionResult> IndexPageTrainingRequestViewModels(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingRequestViewModel = await _context.TrainingRequestViewModel
                .FirstOrDefaultAsync(m => m.id == id);

            if (trainingRequestViewModel == null)
            {
                return NotFound();
            }

            // Return a partial view instead of a full view
            return PartialView("_TrainingRequestDetails", trainingRequestViewModel);
        }
        public async Task<IActionResult> MissionandTrainingForm(int id)
        {
            if (id <= 0) // Changed the check to ensure id is valid
            {
                return NotFound();
            }

            var missionandTrainingForm = await _context.MissionandTrainingForm
                .FirstOrDefaultAsync(m => m.id == id);

            if (missionandTrainingForm == null)
            {
                return NotFound();
            }

            return PartialView("MissionandTrainingForm", missionandTrainingForm);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateFormStatus(int id, string formType, string status)
        {
            // Validate ID and Status
            if (id <= 0 || string.IsNullOrWhiteSpace(formType) || string.IsNullOrWhiteSpace(status))
            {
                return BadRequest("Invalid ID, form type, or status.");
            }

            // Validate Status value (optional)
            var validStatuses = new List<string> { "Approved", "Rejected" };
            if (!validStatuses.Contains(status))
            {
                return BadRequest("Invalid status value.");
            }

            object form = null;

            // Retrieve the form based on formType
            switch (formType)
            {
                case "Mission and Training Form":
                    form = await _context.MissionandTrainingForm.FindAsync(id);
                    if (form is MissionandTrainingForm missionForm)
                    {
                        missionForm.status = status;
                    }
                    break;

                case "Training Request Form":
                    form = await _context.TrainingRequestViewModel.FindAsync(id);
                    if (form is TrainingRequestViewModel trainingForm)
                    {
                        trainingForm.status = status;
                    }
                    break;

                default:
                    return BadRequest("Invalid form type.");
            }

            // If form is null, return NotFound
            if (form == null)
            {
                return NotFound("Form not found.");
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { Message = $"Form {status} successfully." });
            }
            catch (Exception ex)
            {
                // Log exception (if logging is set up)
                Console.WriteLine($"Error updating form status: {ex.Message}");

                // Return an error message if saving fails
                return StatusCode(500, "An error occurred while updating the form status.");
            }
        }

    }
}