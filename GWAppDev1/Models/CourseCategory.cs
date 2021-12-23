using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GWAppDev1.Models
{
    public class CourseCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string NameCourseCategory { get; set; }
        [Required]
        [StringLength(50)]
        public string Descriptions { get; set; }
    }
}