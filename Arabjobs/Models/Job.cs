using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arabjobs.Models
{
    public class Job
    {
        public int Id  { get; set; }

        [Required]
        [Display(Name = "اسم الوظيفة")]
        public string  JobName { get; set; }

        [Required]
        [Display(Name = "وصف الوظيفة")]
        public string JobDescription { get; set; }
       
        [DisplayName("صورة الوظيفة")]
        public string JobImage { get; set; }


        [Display (Name = "نووع الوظيفة")]
        public int  CategoryId { get; set; }

        public string UserId { get; set; }
        //one relation
        public  virtual Category categ { get; set; }

        
        public virtual  ApplicationUser User { get; set; }
    }
}