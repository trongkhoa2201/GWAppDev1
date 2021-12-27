using GWAppDev1.Models;
using GWAppDev1.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GWAppDev1.Controllers
{
    [Authorize(Roles = Role.Trainee)]
    public class TraineesController : Controller
    {
        private ApplicationDbContext _context;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public TraineesController()
        {
            _context = new ApplicationDbContext();
        }

        public TraineesController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Trainee
        public ActionResult Index()
        {
            var trainee = _context.Trainees.ToList();
            return View(trainee);
        }
        [HttpGet]
        public ActionResult Detail(int id)
        {
            var trainee = _context.Trainees.SingleOrDefault(t => t.Id == id);
            if (trainee == null)
            {
                return HttpNotFound();
            }
            return View(trainee);
        }
        [HttpGet]
        public ActionResult ShowAllCourses()
        {
            var userId = User.Identity.GetUserId();
            var courses = _context.CourseTrainee
                .Where(u => u.UserId.Equals(userId))
                .Select(u => u.Course)
                .ToList();
            if (courses == null)
            {
                return HttpNotFound();
            }
            return View(courses);
        }
        [HttpGet]
        public ActionResult ShowOtherTrainee(int id)
        {
            //get current user Id
            var userId = User.Identity.GetUserId();
            var users = _context.CourseTrainee
                .Where(u => u.CourseId == id)
                .Select(u => u.User)
                .ToList();
            var role = _context.Roles
              .SingleOrDefault(r => r.Name.Equals(Role.Trainee));
            var newusers = users
             .Where(m => m.Roles.Any(r => r.RoleId.Equals(role.Id)) && m.Id != userId)
             .ToList();
            return View(newusers);
        }
    }
}