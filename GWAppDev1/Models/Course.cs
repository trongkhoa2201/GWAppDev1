using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GWAppDev1.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string CourseName { get; set; }
        [Required]
        [StringLength(100)]
        public string Description { get; set; }
        [ForeignKey("CourseCategory")]
        public int CourseCategoryId { get; set; }
        public CourseCategory CourseCategory { get; set; }


    }
}