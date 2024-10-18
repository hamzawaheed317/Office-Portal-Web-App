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
    public class Reply_Comment_ModelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Reply_Comment_ModelController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reply_Comment_Model
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reply_Comment_Model.ToListAsync());
        }

        // GET: Reply_Comment_Model/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reply_Comment_Model = await _context.Reply_Comment_Model
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reply_Comment_Model == null)
            {
                return NotFound();
            }

            return View(reply_Comment_Model);
        }

        public async Task<IActionResult> GetReplies(int commentId)
        {
            var replies = await _context.Reply_Comment_Model
           .Where(r => r.Comment_Id == commentId)
           .OrderByDescending(r => r.PostedDate)
           .ToListAsync();

            // Return the partial view with the list of replies
            return PartialView("_ReplyPartial", replies);
        }


        // GET: Reply_Comment_Model/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reply_Comment_Model/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reply_Comment_Model reply_Comment_Model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reply_Comment_Model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reply_Comment_Model);
        }

        // GET: Reply_Comment_Model/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reply_Comment_Model = await _context.Reply_Comment_Model.FindAsync(id);
            if (reply_Comment_Model == null)
            {
                return NotFound();
            }
            return View(reply_Comment_Model);
        }

        // POST: Reply_Comment_Model/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Author,Content,PostedDate,status,AnnouncementId,Comment_Id")] Reply_Comment_Model reply_Comment_Model)
        {
            if (id != reply_Comment_Model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reply_Comment_Model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Reply_Comment_ModelExists(reply_Comment_Model.Id))
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
            return View(reply_Comment_Model);
        }

        // GET: Reply_Comment_Model/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reply_Comment_Model = await _context.Reply_Comment_Model
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reply_Comment_Model == null)
            {
                return NotFound();
            }

            return View(reply_Comment_Model);
        }

        // POST: Reply_Comment_Model/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reply_Comment_Model = await _context.Reply_Comment_Model.FindAsync(id);
            if (reply_Comment_Model != null)
            {
                _context.Reply_Comment_Model.Remove(reply_Comment_Model);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Reply_Comment_ModelExists(int id)
        {
            return _context.Reply_Comment_Model.Any(e => e.Id == id);
        }
    }
}
