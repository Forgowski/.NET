using BD.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BD.Controllers
{
    public class CourseControler : Controller
    {
            public IActionResult Course()
            {
                Course course = new Course
                {
                    Id = 1,
                    Name = "Sample Course",
                    Price = 29.99m,
                    Duration = 10,
                    Author = "John Doe",
                    Description = "A sample course description.",
                    Category = "Programming",
                    Rate = 4.5,
                    VotesNum = 100
                };

                return View();
            }
        
    
}
}
