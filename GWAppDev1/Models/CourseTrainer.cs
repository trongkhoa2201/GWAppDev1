using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GWAppDev1.Models
{
    public class CourseTrainer
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }
        [Key]
        [Column(Order = 2)]
        [ForeignKey("Trainer")]
        public string TrainerId { get; set; }
        public ApplicationUser Trainer { get; set; }
    }
}