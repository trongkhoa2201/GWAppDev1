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
            var users = _context.CourseTrainer
                .Where(t => t.CourseId == id)
                .Select(t => t.User)               
                .ToList();
            ViewBag.courseId = id;
            return View(users);
        }
        [HttpGet]
        public ActionResult AssignTrainer()
        {
            var role = _context.Roles
                .SingleOrDefault(r => r.Name.Equals(Role.Trainer));
            var users = _context.Users
                .Where(m => m.Roles.Any(r => r.RoleId.Equals(role.Id)))
                .ToList();
            var viewModel = new CoursesUsersViewModel
            {
                Courses = _context.Courses.ToList(),
                Users = users
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AssignTrainer(CoursesUsersViewModel viewModel)
        {
            var model = new CourseTrainer
            {
                CourseId = viewModel.CourseId,
                UserId = viewModel.UserId
            };
            try
            {
                _context.CourseTrainer.Add(model);
                _context.SaveChanges();
            }
            catch (System.Exception)
            {
                ModelState.AddModelError("duplicate", "User already existed in team");
                var role = _context.Roles
                    .SingleOrDefault(r => r.Name.Equals(Role.Trainer));
                var users = _context.Users
                    .Where(m => m.Roles.Any(r => r.RoleId.Equals(role.Id)))
                    .ToList();
                var NewviewModel = new CoursesUsersViewModel
                {
                    Courses = _context.Courses.ToList(),
                    Users = users
                };
                return View(NewviewModel);
            }
            return RedirectToAction("ShowCourse");
        }

        [HttpGet]
        public ActionResult RemoveTrainer(int id, string trainerId)
        {
            var userinCourse = _context.CourseTrainer.SingleOrDefault(
              u => u.CourseId == id && u.UserId == trainerId);

            if (userinCourse == null) return HttpNotFound();

            _context.CourseTrainer.Remove(userinCourse);
            _context.SaveChanges();

            return RedirectToAction("ShowTrainers", new { id = id });

        }
        [HttpGet]
        public ActionResult ShowTrainees(int id)
        {
            var users = _context.CourseTrainee
                .Where(t => t.CourseId == id)
                .Select(t => t.User)
                .ToList();
            ViewBag.courseId = id;
            return View(users);
        }
        [HttpGet]
        public ActionResult AssignTrainee()
        {
            var role = _context.Roles.SingleOrDefault(r => r.Name.Equals(Role.Trainee));
            var users = _context.Users
                .Where(m => m.Roles.Any(r => r.RoleId.Equals(role.Id))).ToList();
            var viewModel = new CoursesUsersViewModel
            {
                Courses = _context.Courses.ToList(),
                Users = users
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult AssignTrainee(CoursesUsersViewModel viewModel)
        {
            var model = new CourseTrainee
            {
                CourseId = viewModel.CourseId,
                UserId = viewModel.UserId
            };
            try
            {
                _context.CourseTrainee.Add(model);
                _context.SaveChanges();
            }
            catch (System.Exception)
            {
                ModelState.AddModelError("duplicate", "User already existed in team");
                var role = _context.Roles.SingleOrDefault(r => r.Name.Equals(Role.Trainee));
                var users = _context.Users
                    .Where(m => m.Roles.Any(r => r.RoleId.Equals(role.Id))).ToList();
                var NewviewModel = new CoursesUsersViewModel
                {
                    Courses = _context.Courses.ToList(),
                    Users = users
                };
                return View(NewviewModel);
            }
            return RedirectToAction("ShowCourse");
        }
        [HttpGet]
        public ActionResult RemoveTrainee(int id, string traineeId)
        {
            var userinCourse = _context.CourseTrainee.SingleOrDefault(
              u => u.CourseId == id && u.UserId == traineeId);

            if (userinCourse == null) return HttpNotFound();

            _context.CourseTrainee.Remove(userinCourse);
            _context.SaveChanges();

            return RedirectToAction("ShowTrainees", new { id = id });
        }
    }
}