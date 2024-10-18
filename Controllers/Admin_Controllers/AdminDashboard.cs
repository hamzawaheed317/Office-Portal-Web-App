using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficePortal.Data;
using OfficePortal.Models;
using OfficePortal.Models.Account_Models;
using OfficePortal.Models.Admin_Models;
using OfficePortal.Models.Admin_Models.Announcement_Models;
using OfficePortal.Models.Employee_Models;
using OfficePortal.Services;
using WebApplication1.Services;

namespace OfficePortal.Controllers.Admin_Controllers
{
    public class AdminDashboard : Controller
    {
        private readonly ApplicationDbContext _context;
        private LanguageService _localization;
        private readonly IActiveDirectoryService _activeDirectoryService;
        private UserInfo _userInfo;

        public AdminDashboard(ApplicationDbContext context, LanguageService localization, IActiveDirectoryService activeDirectoryService)
        {
            _context = context;

            _localization = localization;

            _activeDirectoryService = activeDirectoryService;


            _userInfo = _activeDirectoryService.GetUserInfo();

        }

        [Authorize(Policy = "AdminPolicy")]

        public async Task<IActionResult> HomeAdmin()
        {
            return View(await _context.Announcement.ToListAsync());
        }
        public async Task<IActionResult> Home()
        {
            var viewModel = await GetAdminDataAsync();

            return View("HomeAdmin", viewModel);
        }


        [Authorize(Policy = "AdminPolicy")]


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

        [Authorize(Policy = "AdminPolicy")]

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

        [Authorize(Policy = "AdminPolicy")]

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

        [Authorize(Policy = "AdminPolicy")]

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

        [Authorize(Policy = "AdminPolicy")]

        private async Task<AdminViewModel> GetAdminDataAsync()
        {
            var viewModel = new AdminViewModel();

            // Get all announcements
            viewModel.announcements = await _context.Announcement.ToListAsync();

            // Count pending form requests from the two form tables
            viewModel.form_requests_count = await _context.TrainingRequestViewModel
                .Where(f => f.status == "Pending...") // Adjust the status name as needed
                .CountAsync() +
                await _context.MissionandTrainingForm // Replace with your actual DbSet name
                .Where(f => f.status == "Pending...")
                .CountAsync();

            // Count pending comments
            viewModel.comment_requests_count = await _context.Comment
                .Where(c => c.status == "Pending...") // Adjust the status name as needed
                .CountAsync();

            // Assuming you have a separate count for initiatives if needed
            viewModel.initative_requests_count = 20;

            return viewModel;
        }

        [Authorize(Policy = "AdminPolicy")]

        public IActionResult GetPendingComments()
        {
            // Fetch all pending comments
            var pendingComments = _context.Comment
                .Where(c => c.status == "Pending...")
                .ToList();

            // Fetch all pending replies
            var pendingReplies = _context.Reply_Comment_Model
                .Where(r => r.status == "Pending...")
                .OrderByDescending(r => r.PostedDate)
                .ToList();

            // Combine the comments with their corresponding replies based on status
            var pendingCommentsWithReplies = pendingComments.Select(c => new CommentViewModel
            {
                Comment = c,
                Replies = pendingReplies // Include all pending replies
            }).ToList();

            return View("GetPendingComments", pendingCommentsWithReplies);
        }



        [Authorize(Policy = "AdminPolicy")]


        [HttpPost]
        public async Task<IActionResult> UpdateCommentStatus(int id, string newStatus)
        {
            var comment = await _context.Comment.FindAsync(id);
            if (comment != null)
            {
                comment.status = newStatus; // Update the status
                await _context.SaveChangesAsync(); // Save changes to the database

                return Json(new { success = true }); // Return a success response
            }

            return Json(new { success = false, message = "Comment not found." }); // Return a failure response
        }

        [Authorize(Policy = "AdminPolicy")]


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.announcement.Content))
            {
                return Json(new { success = false, message = "Content should not be null." });
            }

            string fileUrl = null;
            if (model.announcement.FileUrl != null && model.announcement.FileUrl.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".mp4" };
                var extension = Path.GetExtension(model.announcement.FileUrl.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    return Json(new { success = false, message = "Only image files (.jpg, .png) or video files (.mp4) are allowed." });
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                fileUrl = Path.Combine(uploadsFolder, model.announcement.FileUrl.FileName);

                using (var stream = new FileStream(fileUrl, FileMode.Create))
                {
                    await model.announcement.FileUrl.CopyToAsync(stream);
                }
            }

            var announcement = new Announcement
            {
                Content = model.announcement.Content,
                Author = _userInfo.Employee_Name,
                PostedDate = DateTime.Now,
                IsPinned = false,
                LikesCount = 0,
                FileUrl = model.announcement.FileUrl?.FileName
            };

            _context.Announcement.Add(announcement);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Announcement created successfully!" });
        }
    }


}