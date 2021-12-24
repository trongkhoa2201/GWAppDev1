using GWAppDev1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GWAppDev1.ViewModels
{
    public class CoursesUsersViewModel
    {
        public string UserId { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
        public int CourseId { get; set; }
        public IEnumerable<Course> Courses { get; set; }
    }
}