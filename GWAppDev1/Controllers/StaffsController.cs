using GWAppDev1.Models;
using GWAppDev1.Utils;
using GWAppDev1.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace GWAppDev1.Controllers
{
    [Authorize(Roles = Role.Staff)]
    public class StaffsController : Controller
    {
        private ApplicationDbContext _context;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public StaffsController()
        {
            _context = new ApplicationDbContext();
        }

        public StaffsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CreateTrainee()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTrainee(RegisterViewModels viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = viewModel.Trainee.Email, Email = viewModel.Trainee.Email };
                var result = await UserManager.CreateAsync(user, viewModel.Password);
                var userId = user.Id;
                var newTrainee = new Trainee()
                {
                    UserId = userId,
                    Fullname = viewModel.Trainee.Fullname,
                    Age = viewModel.Trainee.Age,
                    Address = viewModel.Trainee.Address,
                    Email = viewModel.Trainee.Email,
                    DateOfBirth = viewModel.Trainee.DateOfBirth
                };
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, Role.Trainee);
                    _context.Trainees.Add(newTrainee);
                    _context.SaveChanges();
                    return RedirectToAction("ShowTraineeInfo", "Staffs");
                }

                AddErrors(result);
            }
            return View(viewModel);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        [HttpGet]
        public ActionResult ShowTraineeInfo(string searchString)
        {
            var Trainees = _context.Trainees.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                Trainees = Trainees.Where(t => t.Fullname.ToLower().Contains(searchString.ToLower()) ||
                t.Age.ToString().Contains(searchString.ToLower())).ToList();
            }
            return View(Trainees);
        }
        [HttpGet]
        public ActionResult EditTraineeInfo(int id)
        {
            var trainee = _context.Trainees.SingleOrDefault(t => t.Id == id);
            if (trainee == null)
            {
                return HttpNotFound();
            }
            return View(trainee);
        }
        [HttpPost]
        public ActionResult EditTraineeInfo(Trainee model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var traineeInDb = _context.Trainees.SingleOrDefault(t => t.Id == model.Id);

            if (traineeInDb == null)
            {
                return HttpNotFound();
            }
            traineeInDb.Fullname = model.Fullname;
            traineeInDb.Age = model.Age;
            traineeInDb.Address = model.Address;
            traineeInDb.DateOfBirth = model.DateOfBirth;
            _context.SaveChanges();
            return RedirectToAction("ShowTraineeInfo");
        }
        [HttpGet]
        public ActionResult DeleteTrainee(int id)
        {
            var userId = User.Identity.GetUserId();
            var trainees = _context.Trainees.SingleOrDefault(t => t.Id == id && t.UserId == userId);
            if (trainees == null)
            {
                return HttpNotFound();
            }
            _context.Trainees.Remove(trainees);
            _context.SaveChanges();
            return RedirectToAction("ShowTraineeInfo");
        }
        [HttpGet]
        public ActionResult ShowCourseCategory(string searchString)
        {
            var courseCategory = _context.courseCategories.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                courseCategory = courseCategory.Where(t => t.NameCourseCategory.ToLower().Contains(searchString.ToLower()) ||
                 t.Descriptions.ToLower().Contains(searchString.ToLower())).ToList();
            }
            return View(courseCategory);
        }
        [HttpGet]
        public ActionResult CreateCourseCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateCourseCategory(CourseCategory model)
        {
            var newCourseCategory = new CourseCategory()
            {
                NameCourseCategory = model.NameCourseCategory,
                Descriptions = model.Descriptions
            };
            _context.courseCategories.Add(newCourseCategory);
            _context.SaveChanges();
            return RedirectToAction("ShowCourseCategory");
        }
        [HttpGet]
        public ActionResult EditCourseCategory(int id)
        {
            var courseCategory = _context.courseCategories.SingleOrDefault(t => t.Id == id);
            if (courseCategory == null)
            {
                return HttpNotFound();
            }
            return View(courseCategory);
        }
        [HttpPost]
        public ActionResult EditCourseCategory(CourseCategory model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var courseCategoryInDb = _context.courseCategories.SingleOrDefault(t => t.Id == model.Id);

            if (courseCategoryInDb == null)
            {
                return HttpNotFound();
            }
            courseCategoryInDb.NameCourseCategory = model.NameCourseCategory;
            courseCategoryInDb.Descriptions = model.Descriptions;
            _context.SaveChanges();
            return RedirectToAction("ShowCourseCategory");
        }
        [HttpGet]
        public ActionResult DeleteCourseCategory(int id)
        {
            var courseCategory = _context.courseCategories.SingleOrDefault(t => t.Id == id);
            if (courseCategory == null)
            {
                return HttpNotFound();
            }
            _context.courseCategories.Remove(courseCategory);
            _context.SaveChanges();
            return RedirectToAction("ShowCourseCategory");
        }

        [HttpGet]
        public ActionResult ShowCourse(string searchString)
        {
            var course = _context.Courses.Include(t => t.CourseCategory).ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                course = course.Where(t => t.CourseName.ToLower().Contains(searchString.ToLower()) ||
                  t.Description.ToLower().Contains(searchString.ToLower())).ToList();
            }
            return View(course);
        }
        [HttpGet]
        public ActionResult CreateCourse()
        {
            var viewModel = new CourseCategoriesViewModel()
            {
                CourseCategory = _context.courseCategories.ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult CreateCourse(CourseCategoriesViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var model = new CourseCategoriesViewModel
                {
                    CourseCategory = viewModel.CourseCategory.ToList(),
                    Course = viewModel.Course
                };
                return View(model);
            }
            var newCourse = new Course()
            {
                CourseName = viewModel.Course.CourseName,
                Description = viewModel.Course.Description,
                CourseCategoryId = viewModel.Course.CourseCategoryId
            };
            _context.Courses.Add(newCourse);
            _context.SaveChanges();
            return RedirectToAction("ShowCourse");
        }
        [HttpGet]
        public ActionResult DeleteCourse(int id)
        {
            var course = _context.Courses.SingleOrDefault(t => t.Id == id);
            if (course == null)
            {
                return HttpNotFound();
            }
            _context.Courses.Remove(course);
            _context.SaveChanges();
            return RedirectToAction("ShowCourse");
        }
        [HttpGet]
        public ActionResult EditCourse(int id)
        {
            var course = _context.Courses.SingleOrDefault(t => t.Id == id);
            if (course == null)
            {
                return HttpNotFound();
            }
            var viewModel = new CourseCategoriesViewModel()
            {
                Course = course,
                CourseCategory = _context.courseCategories.ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult EditCourse(CourseCategoriesViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var model = new CourseCategoriesViewModel()
                {
                    CourseCategory = viewModel.CourseCategory.ToList(),
                    Course = viewModel.Course
                };
                return View(model);
            }
            var courseInDb = _context.Courses.SingleOrDefault(t => t.Id == viewModel.Course.Id);
            if (courseInDb == null)
            {
                return HttpNotFound();
            }
            courseInDb.CourseName = viewModel.Course.CourseName;
            courseInDb.Description = viewModel.Course.Description;
            courseInDb.CourseCategoryId = viewModel.Course.CourseCategoryId;
            _context.SaveChanges();
            return RedirectToAction("ShowCourse");
        }
        [HttpGet]
        public ActionResult ShowTrainers(int id)
        {
            var trainers = _context.CourseTrainers
                .Where(t => t.CourseId == id)
                .Select(t => t.Trainer)
                .ToList();
            return View(trainers);
        }
        [HttpGet]
        public ActionResult AssignTrainer()
        {
            var role = (from r in _context.Roles where r.Name.Contains("Trainer") select r).FirstOrDefault();
            var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();
            var viewModel = new CoursesTrainersViewModel
            {
                Courses = _context.Courses.ToList(),
                Trainers = users
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult AssignTrainer(CoursesTrainersViewModel viewModel)
        {
            var model = new CourseTrainer
            {
                CourseId = viewModel.CourseId,
                TrainerId = viewModel.TrainerId
            };

            try
            {
                _context.CourseTrainers.Add(model);
                _context.SaveChanges();
            }
            catch (System.Exception)
            {
                ModelState.AddModelError("duplicate", "User already existed in Team");
                var role = (from r in _context.Roles where r.Name.Contains("Trainer") select r).FirstOrDefault();
                var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();
                var newViewModel = new CoursesTrainersViewModel
                {
                    Courses = _context.Courses.ToList(),
                    Trainers = users
                };
                return View(newViewModel);
            }

            return RedirectToAction("ShowTeam");
        }
        [HttpGet]
        public ActionResult RemoveTrainer(int id, string trainerId)
        {
            var TrainerInCourse = _context.CourseTrainers.SingleOrDefault(
              u => u.CourseId == id && u.TrainerId == trainerId);

            if (TrainerInCourse == null) return HttpNotFound();

            _context.CourseTrainers.Remove(TrainerInCourse);
            _context.SaveChanges();

            return RedirectToAction("ShowTrainers", new { id = id });
        }
    }
}