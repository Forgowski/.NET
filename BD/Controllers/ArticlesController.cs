using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BD.Data;
using BD.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel;
using System.Security.Claims;

namespace BD.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ArticlesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Articles

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Article.Include(a => a.Course);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article
                .Include(a => a.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        public IActionResult GetArticlesByCourseId(int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isCourseBought = _context.BuyCourses.Any(bc => bc.CourseId == courseId && bc.UserId == userId);

            var articles = _context.Article.Where(a => a.CourseId == courseId).ToList();
            var sortedArticles = articles.OrderBy(a => a.CourseId).ToList();

            ViewBag.IsCourseBought = isCourseBought;
            return PartialView("_ArticlesPartialView", sortedArticles);
        }

        // GET: Articles/Create
        [DisplayName("Add new article")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(int id)
        {
            System.Diagnostics.Debug.WriteLine($"CourseId received: {id}");
            ViewBag.CourseId = id;
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TextContent,Title,CourseId")] Article article)
        {
            if (ModelState.IsValid)
            {
                _context.Add(article);
                await _context.SaveChangesAsync();
                int courseId = article.CourseId;

                return RedirectToAction("Details", "Courses", new { id = courseId });
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId", article.CourseId);
            return View(article);
        }

        // GET: Articles/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId", article.CourseId);
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TextContent,Title,CourseId")] Article article)
        {
            if (id != article.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                int courseId = article.CourseId;

                return RedirectToAction("Details", "Courses", new { id = courseId });
            }

            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId", article.CourseId);
            return View(article);
        }

        // GET: Articles/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article
                .Include(a => a.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int courseId = 0;
            var article = await _context.Article.FindAsync(id);
            if (article != null)
            {
                courseId = article.CourseId;
                _context.Article.Remove(article);
            }

            await _context.SaveChangesAsync();
            if (courseId == 0)
            {
                return RedirectToAction("Index", "Courses");
            }
            else
            {
                return RedirectToAction("Details", "Courses", new { id = courseId });
            }
            }

        private bool ArticleExists(int id)
        {
            return _context.Article.Any(e => e.Id == id);
        }
    }
}
