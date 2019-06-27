using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Arabjobs.Models
{
    public class Category
    {
        public Category()
        {
            Jobs=new HashSet<Job>();
        }
        public int Id { get; set; }
        [Required]
        [DisplayName("نوع الوظيفة")]
        public string CategoryName { get; set; }

        [Required]
        [DisplayName("وصف النوع")]
        public string CategoryDescription { get; set; }

        //menyRelation
        public virtual ICollection<Job> Jobs { get; set; }

    }
}