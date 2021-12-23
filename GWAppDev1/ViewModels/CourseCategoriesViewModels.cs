using GWAppDev1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GWAppDev1.ViewModels
{
    public class CourseCategoriesViewModel
    {
        public Course Course { get; set; }
        public IEnumerable<CourseCategory> CourseCategory { get; set; }

    }
}