using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GWAppDev1.Models;
using GWAppDev1.Utils;
using Microsoft.AspNet.Identity;

namespace AppDev.Controllers
{
    public class IndexforTrainerandTraineeController : Controller
    {
        // GET: IndexForTrainerAndTrainee
        private ApplicationDbContext _context;

        public IndexforTrainerandTraineeController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: IndexforTrainerandTrainee
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Authorize(Roles = Role.Trainer)]
        public ActionResult HomeOfTrainer()
        {
            var userId = User.Identity.GetUserId();
            var trainerInDb = _context.Trainers.SingleOrDefault(t => t.UserId == userId);
            return View(trainerInDb);
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
            return RedirectToAction("HomeOfTrainer", "");
        }
        [HttpGet]
        [Authorize(Roles = Role.Trainer)]
        public ActionResult ViewCourse()
        {
            //get courseCategories
            var courseCategoryInDB = _context.courseCategories.ToList();

            var userId = User.Identity.GetUserId();
            var courseInDB = _context.CourseTrainer
                                .Where(t => t.UserId == userId)
                                .Select(c => c.Course).ToList();
            return View(courseInDB);

        }
        [HttpGet]
        [Authorize(Roles = Role.Trainer)]
        public ActionResult ViewTraineeInCourse()
        {
            var userId = User.Identity.GetUserId();
            var TrainerInDb = _context.CourseTrainee.Where(t => t.UserId == userId)
                                                             .Select(c => c.CourseId).ToList();

            return View();
        }
        [HttpGet]
        [Authorize(Roles = Role.Trainee)]
        public ActionResult HomeOfTrainee()
        {
            var userId = User.Identity.GetUserId();
            var traineeInDb = _context.Trainers.SingleOrDefault(t => t.UserId == userId);
            return View(traineeInDb);
        }

        [HttpGet]
        [Authorize(Roles = Role.Trainee)]
        public ActionResult ViewCourseTraineeAssigned()
        {
            //get courseCategories
            var courseCategoryInDB = _context.courseCategories.ToList();

            var userId = User.Identity.GetUserId();
            var courseInDB = _context.CourseTrainee
                                .Where(t => t. UserId == userId)
                                .Select(c => c.Course).ToList();
            return View(courseInDB);

        }

        [HttpGet]
        [Authorize(Roles = Role.Trainee)]
        public ActionResult ViewAllStudentInCourse(int id)
        {
            var userId = User.Identity.GetUserId();
            var TrainerInDb = _context.CourseTrainee.Where(t => t.CourseId == id).Select(c => c.UserId).ToList();
            return View(TrainerInDb);
        }
    }
}