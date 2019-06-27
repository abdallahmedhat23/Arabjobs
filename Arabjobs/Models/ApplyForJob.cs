using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Arabjobs.Models
{
    public class ApplyForJob
    {
        public int  Id { get; set; }
        [DisplayName("الرسالة")]
        public string Message { get; set; }
        [DisplayName("تاريخ التقدم")]
        public DateTime ApplyDate { get; set; }

        public int JobId { get; set; }
        public string UserId { get; set; }

        //many to many relationShip with table Job & AspnetUsers 
        public virtual Job Job { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}