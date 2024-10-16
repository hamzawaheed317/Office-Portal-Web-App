using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficePortal.Data;
using OfficePortal.Models;
using OfficePortal.Models.Account_Models;
using OfficePortal.Models.Admin_Models.Announcement_Models;
using OfficePortal.Services;
using WebApplication1.Services;


namespace OfficePortal.Controllers.Admin_Controllers
{
    public class AnnouncementsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private LanguageService _localization;
        private UserInfo _userInfo;
        private readonly IActiveDirectoryService _activeDirectoryService;


        public AnnouncementsController(ApplicationDbContext context, LanguageService localization, IActiveDirectoryService activeDirectoryService)
        {
            _context = context;
            _localization = localization;
            _activeDirectoryService = activeDirectoryService;

            _userInfo = _activeDirectoryService.GetUserInfo();

        }

        // GET: Announcements
        [Authorize(Policy = "AdminEmployeePolicy")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Announcement.ToListAsync());
        }

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

        [HttpPost]
        public JsonResult PostReply(Reply_Comment_Model reply)
        {
            if (ModelState.IsValid)
            {
                _context.Reply_Comment_Model.Add(reply);
                _context.SaveChanges();
                return Json(new { success = true, message = "Reply posted successfully." });
            }
            return Json(new { success = false, message = "Error posting reply." });
        }



        [Authorize(Policy = "AdminEmployeePolicy")]
        [HttpPost]
        public IActionResult Post_Comment([FromBody] Comment comment)
        {
            var newComment = new Comment
            {
                AnnouncementId = comment.AnnouncementId,
                Content = comment.Content,
                Author = comment.Author,
                PostedDate = DateTime.Now,
                status = comment.status
            };

            _context.Add(newComment);
            _context.SaveChanges();

            return Json(new { success = true, message = _localization.Getkey("CommentPostedMessage").Value });

        }

        [Authorize(Policy = "AdminEmployeePolicy")]
        [HttpGet]
        public async Task<IActionResult> GetComments(int announcementId)
        {
            var comments = await _context.Comment
      .Where(c => c.AnnouncementId == announcementId && c.status == "Approved")
      .OrderByDescending(c => c.PostedDate)
      .ToListAsync();

            return PartialView("_Comments", comments);
        }       

       
       
    }
}
