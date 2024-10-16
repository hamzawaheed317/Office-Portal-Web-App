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
    public class MissionandTrainingFormsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IActiveDirectoryService _activeDirectoryService;

        private UserInfo _userInfo;
        public MissionandTrainingFormsController(ApplicationDbContext context, IEmailService emailService, IActiveDirectoryService activeDirectoryService)
        {
            _context = context;
            _emailService = emailService;
            _activeDirectoryService = activeDirectoryService;
            _userInfo = _activeDirectoryService.GetUserInfo();
        }

        // GET: MissionandTrainingForms
        public async Task<IActionResult> Index()
        {
            return View(await _context.MissionandTrainingForm.ToListAsync());
        }

        // GET: MissionandTrainingForms/Details/5
        public IActionResult Details(int id)
        {
            var form = _context.MissionandTrainingForm.Find(id);
            if (form == null)
            {
                return NotFound();
            }
            return PartialView("_MissionandTrainingFormDetails", form); // Create a partial view for the details
        }


        // GET: MissionandTrainingForms/Create
        public IActionResult Create()
        {

            MissionandTrainingForm model = new MissionandTrainingForm
            {
                EmployeeName = _userInfo.Employee_Name,
                JobTitle = _userInfo.Job_Title,
                Grade = _userInfo.Grade,
                Number = _userInfo.Employee_Number.ToString(),
                Department = _userInfo.Department,
                LineManager = _userInfo.role
            };

            return PartialView("_MissionTrainingFormPartial", model); // Return the partial view with the model
        }

        // POST: MissionandTrainingForms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MissionandTrainingForm missionandTrainingForm)
        {
            if (ModelState.IsValid)
            {
                // Save the form data to the database
                _context.Add(missionandTrainingForm);
                await _context.SaveChangesAsync();

                // Prepare email content
                string subject = "New Mission and Training Form Submitted";
                string messageText = $"A new mission and training form has been submitted.\n\n" +
                                     $"Details:\n" +
                                     $"Name: {missionandTrainingForm.EmployeeName}\n" +
                                     $"Date: {missionandTrainingForm.date_from}\n" +
                                     $"Description: {missionandTrainingForm.Purpose}\n";

                await _emailService.SendEmailAsync(_userInfo.Admin_email, subject, messageText);

                return RedirectToAction(nameof(Index));
            }
            return View(missionandTrainingForm);
        }
       

        // GET: MissionandTrainingForms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var missionandTrainingForm = await _context.MissionandTrainingForm.FindAsync(id);
            if (missionandTrainingForm == null)
            {
                return NotFound();
            }
            return View(missionandTrainingForm);
        }

        // POST: MissionandTrainingForms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,EmployeeName,JobTitle,Grade,Number,Department,LineManager,Type,Purpose,Departure_date,Arrival_date,date_from,Departure_time,Arrival_time,status,Form_type")] MissionandTrainingForm missionandTrainingForm)
        {
            if (id != missionandTrainingForm.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(missionandTrainingForm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MissionandTrainingFormExists(missionandTrainingForm.id))
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
            return View(missionandTrainingForm);
        }

        // GET: MissionandTrainingForms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var missionandTrainingForm = await _context.MissionandTrainingForm
                .FirstOrDefaultAsync(m => m.id == id);
            if (missionandTrainingForm == null)
            {
                return NotFound();
            }

            return View(missionandTrainingForm);
        }

        // POST: MissionandTrainingForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var missionandTrainingForm = await _context.MissionandTrainingForm.FindAsync(id);
            if (missionandTrainingForm != null)
            {
                _context.MissionandTrainingForm.Remove(missionandTrainingForm);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MissionandTrainingFormExists(int id)
        {
            return _context.MissionandTrainingForm.Any(e => e.id == id);
        }
    }
}
