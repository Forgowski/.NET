using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BD.Data;
using BD.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BD.Controllers
{
    public class BuyCoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BuyCoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BuyCourses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BuyCourses.Include(b => b.Course).Include(b => b.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BuyCourses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buyCourses = await _context.BuyCourses
                .Include(b => b.Course)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BuyId == id);
            if (buyCourses == null)
            {
                return NotFound();
            }

            return View(buyCourses);
        }

        // GET: BuyCourses/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: BuyCourses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BuyId,UserId,CourseId")] BuyCourses buyCourses)
        {
            if (ModelState.IsValid)
            {
                _context.Add(buyCourses);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId", buyCourses.CourseId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", buyCourses.UserId);
            return View(buyCourses);
        }

        // GET: BuyCourses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buyCourses = await _context.BuyCourses.FindAsync(id);
            if (buyCourses == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId", buyCourses.CourseId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", buyCourses.UserId);
            return View(buyCourses);
        }

        // POST: BuyCourses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BuyId,UserId,CourseId")] BuyCourses buyCourses)
        {
            if (id != buyCourses.BuyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(buyCourses);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuyCoursesExists(buyCourses.BuyId))
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
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId", buyCourses.CourseId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", buyCourses.UserId);
            return View(buyCourses);
        }

        // GET: BuyCourses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buyCourses = await _context.BuyCourses
                .Include(b => b.Course)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BuyId == id);
            if (buyCourses == null)
            {
                return NotFound();
            }

            return View(buyCourses);
        }

        // POST: BuyCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var buyCourses = await _context.BuyCourses.FindAsync(id);
            if (buyCourses != null)
            {
                _context.BuyCourses.Remove(buyCourses);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Buy(string userId, int courseId)
        {
            System.Diagnostics.Debug.WriteLine("Buy action called!");
            System.Diagnostics.Debug.WriteLine($"UserId: {userId}, CourseId: {courseId}");

            try
            {
                var buyCourse = new BuyCourses
                {
                    UserId = userId,
                    CourseId = courseId
                };

                _context.BuyCourses.Add(buyCourse);
                await _context.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine("Record added successfully!");
            }
            catch (DbUpdateException ex)
            {
                System.Diagnostics.Debug.WriteLine($"DbUpdateException: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"DbUpdateException InnerException: {ex.InnerException?.Message}");
                throw;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding record: {ex.Message}");
                throw;
            }

            return RedirectToAction("Details", "Courses", new { id = courseId });
        }


        [Authorize]
        public IActionResult MyCourses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var myCourseIds = _context.BuyCourses
                .Where(bc => bc.UserId == userId)
                .Select(bc => bc.CourseId)
                .ToList();

            if (myCourseIds.Count == 0)
            {
                // Przekierowanie do innego widoku lub akcji, np. Index
                return RedirectToAction("Index");
            }

            var myCourses = _context.Course
                .Where(c => myCourseIds.Contains(c.CourseId))
                .ToList();

            return View(myCourses);
        }

        private bool BuyCoursesExists(int id)
        {
            return _context.BuyCourses.Any(e => e.BuyId == id);
        }
    }
}
