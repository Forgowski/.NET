using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BD.Data;
using BD.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BD.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public QuestionsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Questions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Question.Include(q => q.Course);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> ShowQuiz(int courseId)
        {
            var questions = await _context.Question
                .Where(q => q.CourseId == courseId)
                .OrderBy(q => q.QuestionId)
                .ToListAsync();

            return View(questions);
        }

        // GET: Questions/Details/5
        [Authorize]
        public async Task<IActionResult> TakeQuiz(int courseId)
        {
            var questions = await _context.Question
                .Where(q => q.CourseId == courseId)
                .OrderBy(q => q.QuestionId)
                .ToListAsync();

            return View(questions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult TakeQuiz(IFormCollection formCollection)
        {
            int userScore = 0;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int questId;
            if (formCollection.TryGetValue("courseId", out var courseIdValue) && int.TryParse(courseIdValue, out int courseId))
            {
                foreach (var key in formCollection.Keys)
                {
                    if (key == "courseId")
                    {
                        continue;
                    }
                    if (int.TryParse(key, out questId))
                    {
                        var question = _context.Question.FirstOrDefault(q => q.QuestionId == questId);
                        if (question != null)
                        {
                            if (formCollection[key] == question.AnswerCorrect)
                            {
                                userScore += question.Point;
                            }
                        }
                    }
                }
                var existingQuizResult = _context.QuizResult.FirstOrDefault(q =>
                                            q.CourseId == courseId &&
                                            q.UserId == userId);
                if (existingQuizResult != null)
                {
                    if(userScore > existingQuizResult.ResultPoint)
                    {
                        existingQuizResult.ResultPoint = userScore;
                        _context.SaveChanges();
                    }
                }
                else { 
                    var quizResult = new QuizResult
                        {
                            CourseId = courseId,
                            UserId = userId,
                            ResultPoint = userScore
                       };
                    _context.QuizResult.Add(quizResult);
                    _context.SaveChanges();
                    }
                return RedirectToAction("Details", "Courses", new { id = courseId });
            }
            return View();
        }

            // GET: Questions/Create
            [Authorize(Roles = "Admin")]
        public IActionResult Create(int courseId)
        {
            ViewBag.CourseId = courseId;
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,QuestionText,AnswerA,AnswerB,AnswerC,AnswerD,AnswerCorrect,Point")] Question question)
        {

                _context.Add(question);
                await _context.SaveChangesAsync();
            return RedirectToAction("ShowQuiz", new { courseId = question.CourseId });
        }




        // GET: Questions/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            ViewBag.courseId = question.CourseId;
            ViewBag.questionId = question.QuestionId;
            
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestionId,CourseId,QuestionText,AnswerA,AnswerB,AnswerC,AnswerD,AnswerCorrect,Point")] Question question)
        {
            if (id != question.QuestionId)
            {
                return NotFound();
            }


                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.QuestionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
            return RedirectToAction("ShowQuiz", new { courseId = question.CourseId });
        }

        // GET: Questions/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .Include(q => q.Course)
                .FirstOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Question.FindAsync(id);
            var crId = question.CourseId;
            if (question != null)
            {
                
                _context.Question.Remove(question);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("ShowQuiz", new { courseId = crId });
        }

        private bool QuestionExists(int id)
        {
            return _context.Question.Any(e => e.QuestionId == id);
        }
    }
}
