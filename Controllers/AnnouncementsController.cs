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
    public class AnnouncementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnnouncementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Announcements
        public async Task<IActionResult> Index()
        {
            return View(await _context.Announcement.ToListAsync());
        }

        // GET: Announcements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcement
                .FirstOrDefaultAsync(m => m.Id == id);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // GET: Announcements/Create
        public IActionResult Create()
        {
            UserInfo userInfo = getAuthor(); // Replace this with actual fetching logic

            AnnouncementViewModel model = new AnnouncementViewModel
            {
                Author = userInfo.Employee_Name
            };

            return View(model);
        }

        // POST: Announcements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnnouncementViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload
                string fileUrl = null;
                if (model.FileUrl != null && model.FileUrl.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    fileUrl = Path.Combine(uploadsFolder, model.FileUrl.FileName);

                    using (var stream = new FileStream(fileUrl, FileMode.Create))
                    {
                        await model.FileUrl.CopyToAsync(stream);
                    }
                }

                var announcement = new Announcement
                {

                    Content = model.Content,
                    Author = model.Author,
                    PostedDate = DateTime.Now,
                    IsPinned = false,
                    LikesCount = 0, // Initialize LikesCount
                    FileUrl = model.FileUrl?.FileName // Store only the file name in the database
                };

                _context.Announcement.Add(announcement);
                await _context.SaveChangesAsync();

                // Redirect or return a view based on your logic
                return RedirectToAction("Index");
            }

            return View(model);
        }
        // GET: Announcements/Edit/5

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcement.FindAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }
            return View(announcement);
        }

        public UserInfo getAuthor()
        {
            return new UserInfo
            {
                Employee_Name = "John Doe"
            };
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostComment(int announcementId, string content, string author)
        {
            if (string.IsNullOrEmpty(content) || announcementId <= 0)
            {
                return BadRequest("Invalid input");
            }

            var newComment = new Comment
            {
                AnnouncementId = announcementId,
                Content = content,
                Author = author,
                PostedDate = DateTime.Now,
                status="Pending..."

            };

            _context.Comment.Add(newComment);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // Method to get comments for a specific announcement
        [HttpGet]
        public async Task<IActionResult> GetComments(int announcementId)
        {
            var comments = await _context.Comment
                .Where(c => c.AnnouncementId == announcementId)
                .OrderByDescending(c => c.PostedDate)
                .ToListAsync();

            return PartialView("_Comments", comments);
        }        // GET: Announcements/Edit/5
      
        // POST: Announcements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,Author,PostedDate,IsPinned,LikesCount,FileUrl")] Announcement announcement)
        {
            if (id != announcement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(announcement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnouncementExists(announcement.Id))
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
            return View(announcement);
        }

        // GET: Announcements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcement
                .FirstOrDefaultAsync(m => m.Id == id);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // POST: Announcements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var announcement = await _context.Announcement.FindAsync(id);
            if (announcement != null)
            {
                _context.Announcement.Remove(announcement);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnnouncementExists(int id)
        {
            return _context.Announcement.Any(e => e.Id == id);
        }
    }
}
