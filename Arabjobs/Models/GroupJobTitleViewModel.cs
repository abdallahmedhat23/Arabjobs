using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Arabjobs.Models
{
    public class GroupJobTitleViewModel
    {
        //هذا من اجل نقل البيانات الي الفيوو
        public string JobtTitle { get; set; }
        public IEnumerable<ApplyForJob> IcolItems { get; set; }
    }
}