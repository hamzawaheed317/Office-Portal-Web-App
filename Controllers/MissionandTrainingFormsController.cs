using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficePortal.Data;
using OfficePortal.Models;

namespace OfficePortal.Controllers
{
    public class MissionandTrainingFormsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        public MissionandTrainingFormsController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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
            UserInfo userInfo = GetUserInfo(); // Replace this with actual fetching logic

            MissionandTrainingForm model = new MissionandTrainingForm
            {
                EmployeeName = userInfo.Employee_Name,
                JobTitle = userInfo.Job_Title,
                Grade = userInfo.Grade,
                Number = userInfo.Employee_Number.ToString(),
                Department = userInfo.Department,
                LineManager = userInfo.role
            };

            // Assuming this method populates all the necessary employee details into the model

            return View(model);
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
                                     $"Name: {missionandTrainingForm.Departure_date}\n" +
                                     $"Date: {missionandTrainingForm.Departure_time}\n" +
                                     $"Description: {missionandTrainingForm.Purpose}\n";

                // Send email
                await _emailService.SendEmailAsync("hasanmughal538@gmail.com", subject, messageText);

                // Redirect to the index page after saving and sending the email
                return RedirectToAction(nameof(Index));
            }
            return View(missionandTrainingForm);
        }
        private UserInfo GetUserInfo()
        {
            return new UserInfo
            {
                Employee_Name = "John Doe",
                Job_Title = "Software Engineer",
                Grade = "A1",
                Employee_Number = "12345",
                Department = "IT",
                role = "Line Manager"
            };
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
