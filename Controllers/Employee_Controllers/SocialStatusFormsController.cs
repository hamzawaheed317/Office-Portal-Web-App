using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficePortal.Data;
using OfficePortal.Models.Account_Models;
using OfficePortal.Models.Employee_Models;
using WebApplication1.Services;

namespace OfficePortal.Controllers.Employee_Controllers
{
        public class SocialStatusFormsController : Controller
        {
            private readonly ApplicationDbContext _context;
            private readonly IEmailService _emailService;
            private readonly IActiveDirectoryService _activeDirectoryService;

            private UserInfo _userInfo;
            private readonly string _uploadFolder;

            public SocialStatusFormsController(ApplicationDbContext context, IEmailService emailService, IActiveDirectoryService activeDirectoryService)
            {
                _context = context;
                _emailService = emailService;
                _activeDirectoryService = activeDirectoryService;
                _userInfo = _activeDirectoryService.GetUserInfo();
                _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadsfiles");

                // Check if the upload folder exists; if not, create it
                if (!Directory.Exists(_uploadFolder))
                {
                    Directory.CreateDirectory(_uploadFolder);
                }
            }

        // GET: SocialStatusForms
        public async Task<IActionResult> Index()
        {
            // Include the related Files for each SocialStatusForm
            var forms = await _context.SocialStatusForm
                                       .Include(f => f.Files) // Load related files for each form
                                       .ToListAsync();

            return View(forms);
        }

        // GET: SocialStatusForms/Details/5
        public async Task<IActionResult> Details(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var socialStatusForm = await _context.SocialStatusForm
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (socialStatusForm == null)
                {
                    return NotFound();
                }

                return View(socialStatusForm);
            }

        // GET: SocialStatusForms/Create
        public IActionResult Create()
        {
            // Initialize the view model with user information
            SocialStatusFormViewModel socialStatusModel = new SocialStatusFormViewModel
            {
                EmployeeName = _userInfo.Employee_Name,
                JobTitle = _userInfo.Job_Title,
                BayanatiNumber = _userInfo.Employee_Number.ToString(),
                Department = _userInfo.Department,
                Nationality = "Europe"
            };

            if (socialStatusModel.UploadedFiles != null && socialStatusModel.UploadedFiles.Count > 0)
            {
                socialStatusModel.UploadedFiles = null; 
            }

            return View(socialStatusModel);
        }


        [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(SocialStatusFormViewModel socialStatusForm)
            {
                if (!ModelState.IsValid)
                {

                TempData["AlertMessage"] = "Error: Please correct the errors in the form.";
                TempData["AlertType"] = "danger";
                return View(socialStatusForm);

                }

                // Step 1: Create the SocialStatusForm model from the view model
                var socialStatus = new SocialStatusForm
                {
                    EmployeeName = socialStatusForm.EmployeeName,
                    JobTitle = socialStatusForm.JobTitle,
                    Nationality = socialStatusForm.Nationality,
                    Department = socialStatusForm.Department,
                    BayanatiNumber = socialStatusForm.BayanatiNumber,
                    JoiningDate = socialStatusForm.JoiningDate,
                    SpouseFullName = socialStatusForm.SpouseFullName,
                    SpouseWorkplace = socialStatusForm.SpouseWorkplace,
                    SpouseDateOfMarriage = socialStatusForm.SpouseDateOfMarriage,
                    SpouseDateOfEmployment = socialStatusForm.SpouseDateOfEmployment,
                    NumberOfChildren = socialStatusForm.NumberOfChildren,
                    SonName = socialStatusForm.SonName,
                    ReasonForChange = socialStatusForm.ReasonForChange,
                    DateOfChange = socialStatusForm.DateOfChange,
                    Form_Type = socialStatusForm.Form_Type,
                    Status = socialStatusForm.Status
                };

                // Step 2: Save the SocialStatusForm to its context
                _context.SocialStatusForm.Add(socialStatus);
                await _context.SaveChangesAsync(); // Save to DB to get the Id

            // Step 3: Process uploaded files
            if (socialStatusForm.UploadedFiles != null && socialStatusForm.UploadedFiles.Count > 0)
            {
                foreach (var file in socialStatusForm.UploadedFiles)
                {
                    System.Diagnostics.Debug.WriteLine($"File name: {file.FileName}, Length: {file.Length}");

                    if (file.Length > 0 && IsFileTypeValid(file.FileName))
                    {
                        var originalFileName = Path.GetFileNameWithoutExtension(file.FileName); // Get original filename without extension
                        var fileExtension = Path.GetExtension(file.FileName); // Get file extension

                        // Generate a base filename using the social status ID
                        var baseFileName = $"{socialStatus.Id}_{originalFileName}";

                        // Create a full file path
                        var filePath = Path.Combine(_uploadFolder, baseFileName + fileExtension);

                        // Check if the file already exists and modify the filename if necessary
                        int counter = 1;
                        while (System.IO.File.Exists(filePath))
                        {
                            // Generate a new filename with a counter
                            filePath = Path.Combine(_uploadFolder, $"{baseFileName}_{counter++}{fileExtension}");
                        }

                        try
                        {
                            // Save the file to the uploads folder
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            // Create a File record
                            var fileRecord = new File
                            {
                                File_Name = Path.GetFileName(filePath), // Save the unique filename to the database
                                SocialStatusRecord_Id = socialStatus.Id // Associate with the saved social status
                            };

                            // Add the file record to the context
                            _context.File.Add(fileRecord);
                        }
                        catch (Exception ex)
                            {
                                // Log the exception for debugging
                                System.Diagnostics.Debug.WriteLine($"Error saving file: {ex.Message}");

                                TempData["AlertMessage"] = "Error: Failed to create Social Status Form.";
                                TempData["AlertType"] = "danger";

                        }
                    }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"Invalid file type: {file.FileName}");
                            TempData["AlertMessage"] = "Error: Failed to create Social Status Form.";
                            TempData["AlertType"] = "danger";
                    }
                }

                    // Step 7: Save the file records to their respective DB
                    await _context.SaveChangesAsync();

                    TempData["AlertMessage"] = "Success: Social Status Form created successfully.";
                    TempData["AlertType"] = "success";

            }

            return RedirectToAction(nameof(Index));
            }

            // Function to validate file types
            private bool IsFileTypeValid(string fileName)
            {
                var allowedExtensions = new[] { ".jpeg", ".jpg", ".pdf" };
                var extension = Path.GetExtension(fileName).ToLowerInvariant();
                return allowedExtensions.Contains(extension);
            }

       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var socialStatusForm = await _context.SocialStatusForm.FindAsync(id);
            if (socialStatusForm == null)
            {
                return NotFound();
            }
            return View(socialStatusForm);
        }

        // POST: SocialStatusForms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeName,JobTitle,Nationality,Department,BayanatiNumber,JoiningDate,SpouseFullName,SpouseWorkplace,SpouseDateOfMarriage,SpouseDateOfEmployment,NumberOfChildren,SonName,ReasonForChange,DateOfChange,Form_Type,Status")] SocialStatusForm socialStatusForm)
        {
            if (id != socialStatusForm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(socialStatusForm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SocialStatusFormExists(socialStatusForm.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(socialStatusForm);
        }

        // GET: SocialStatusForms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var socialStatusForm = await _context.SocialStatusForm
                .FirstOrDefaultAsync(m => m.Id == id);
            if (socialStatusForm == null)
            {
                return NotFound();
            }

            return View(socialStatusForm);
        }

        // POST: SocialStatusForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var socialStatusForm = await _context.SocialStatusForm.FindAsync(id);
            if (socialStatusForm != null)
            {
                _context.SocialStatusForm.Remove(socialStatusForm);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SocialStatusFormExists(int id)
        {
            return _context.SocialStatusForm.Any(e => e.Id == id);
        }
    }
}
