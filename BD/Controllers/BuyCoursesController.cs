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
