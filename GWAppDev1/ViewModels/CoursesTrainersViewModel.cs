using GWAppDev1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GWAppDev1.ViewModels
{
    public class CoursesTrainersViewModel
    {
        public string TrainerId { get; set; }
        public IEnumerable<ApplicationUserManager> Trainers { get; set; }
        public int CourseId { get; set; }
        public IEnumerable<Course> Courses { get; set; }
    }
}