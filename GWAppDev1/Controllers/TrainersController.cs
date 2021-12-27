using GWAppDev1.Models;
using GWAppDev1.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GWAppDev1.Controllers
{
    [Authorize(Roles = Role.Trainer)]
    public class TrainersController : Controller
    {
        private ApplicationDbContext _context;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public TrainersController()
        {
            _context = new ApplicationDbContext();
        }

        public TrainersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
        // GET: Trainer
        public ActionResult Index()
        {
            var trainer = _context.Trainers.ToList();
            return View(trainer);
        }
        [HttpGet]
        public ActionResult Detail(int id)
        {
            var trainer = _context.Trainers.SingleOrDefault(t => t.Id == id);
            if (trainer == null)
            {
                return HttpNotFound();
            }
            return View(trainer);
        }
        [HttpGet]
        [Authorize(Roles = Role.Trainer)]
        public ActionResult UpdateProfileOfTrainer(int id)
        {
            var traineeInDb = _context.Trainers.SingleOrDefault(t => t.Id == id);
            if (traineeInDb == null)
            {
                return HttpNotFound();
            }
            return View(traineeInDb);
        }
        [HttpPost]
        [Authorize(Roles = Role.Trainer)]
        public ActionResult UpdateProfileOfTrainer(Trainer trainer)
        {
            if (!ModelState.IsValid)
            {
                return View(trainer);
            }
            var trainerInDb = _context.Trainers.SingleOrDefault(t => t.Id == trainer.Id);
            if (trainerInDb == null)
            {
                return HttpNotFound();
            }
            trainerInDb.Fullname = trainer.Fullname;
            trainerInDb.Age = trainer.Age;
            trainerInDb.Address = trainer.Address;
            trainerInDb.Specialty = trainer.Specialty;

            _context.SaveChanges();
            return RedirectToAction("Detail","Trainers");
        }

        [Authorize(Roles = Role.Trainer)]
        [HttpGet]
        public ActionResult ViewCourse()
        {
            //var courseCategoryInDB = _context.courseCategories.ToList();
            var userId = User.Identity.GetUserId();
            var courseInDB = _context
                .CourseTrainer
                .Where(t => t.UserId == userId)
                .Select(t => t.Course)
                .ToList();
            return View(courseInDB);
        }

        [HttpGet]
        public ActionResult ViewAllTrainees(int id)
        {
            //Get current user ID
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