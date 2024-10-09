using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficePortal.Data;
using OfficePortal.Models;
using OfficePortal.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OfficePortal.Controllers
{
    public class EventModelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private LanguageService _localization;

        public EventModelsController(ApplicationDbContext context, LanguageService localization)
        {
            _context = context;
            _localization = localization;

        }

        // GET: EventModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.EventModel.ToListAsync());
        }

        // GET: EventModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventModel = await _context.EventModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventModel == null)
            {
                return NotFound();
            }

            return View(eventModel);
        }

        public ActionResult<EventModel> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var eventModel = _context.EventModel
                .FirstOrDefault(m => m.Id == id);
            if (eventModel == null)
                return NotFound();

            return eventModel;
        }

        //GET: EventModels/Create
        [HttpGet("Create")]
        public ActionResult Create()
        {
            return PartialView("_CreateEvent", new EventModel());
        }

        // POST: EventModels/Create//
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Create")]
        //[ValidateAntiForgeryToken]
        public IActionResult Create([FromBody] EventModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                _context.SaveChanges();
                //return RedirectToAction(nameof(Index));
                return Json(new { success = true, message = "Event created successfully!"/*, events = RedirectToAction("Detail", new { id = model.Id })*/ });
            }
            return Json(new { success = false, message = "Failed to create event." });
            //return View(Model);
        }
        public IActionResult Calender()
        {
            return View("Calender");
        }
        // GET: EventModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventModel = await _context.EventModel.FindAsync(id);
            if (eventModel == null)
            {
                return NotFound();
            }
            return View(eventModel);
        }

        // POST: EventModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,StartDate,EndDate,Repeat,Location,Attendees,Description,Reminder,Label")] EventModel eventModel)
        {
            if (id != eventModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventModelExists(eventModel.Id))
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
            return View(eventModel);
        }

        // GET: EventModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventModel = await _context.EventModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventModel == null)
            {
                return NotFound();
            }

            return View(eventModel);
        }
        // EventModelsController.cs

        [HttpGet]
        public JsonResult GetEvents()
        {
            // Fetch events from the database and format them for FullCalendar
            var events = _context.EventModel.Select(e => new
            {
                id = e.Id,
                title = e.Title,
                start = e.StartDate.HasValue ? e.StartDate.Value.ToString("yyyy-MM-ddTHH:mm:ss") : null, // Format for FullCalendar
                end = e.EndDate.HasValue ? e.EndDate.Value.ToString("yyyy-MM-ddTHH:mm:ss") : null,     // Format for FullCalendar
                description = e.Description,
                label = e.Label
            }).ToList();

            return Json(events); // Return the events as JSON
        }


        // POST: EventModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventModel = await _context.EventModel.FindAsync(id);
            if (eventModel != null)
            {
                _context.EventModel.Remove(eventModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventModelExists(int id)
        {
            return _context.EventModel.Any(e => e.Id == id);
        }
    }
}
