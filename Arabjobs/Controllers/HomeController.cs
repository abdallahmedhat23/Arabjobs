using System;
using System.Linq;
using System.Web.Mvc;
using Arabjobs.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Arabjobs.Controllers
{
    public class HomeController : Controller
    {
        private  ApplicationDbContext db=new ApplicationDbContext();

        public ActionResult Index()
        {
            
           var list= db.Categories.ToList();
            return View(list);
        }

        public ActionResult Details(int jobid)
        {
                           
            var jobdetails = db.Jobs.Find(jobid);
            Session["JobId"] = jobid;//هذه السيشن تقوم بتمرير اد بتاع الوظيفة وتخزين موقت فيها وتمريرة الي الاسفل ف عملية التقدم لهذه الوظيفة
            if (jobdetails == null)
                return HttpNotFound();
            return View(jobdetails);
        }
        [Authorize]
        public ActionResult Apply()
        {
            
            return View();

        }
        [HttpPost]
      
        public ActionResult Apply(string Message)
        {
           var UserId = User.Identity.GetUserId();//دي هتجبلى اليوزر الحالي اللي عمل تسجيل دخول
           var jobId = (int) Session["JobId"];//عمليت مبصايه من القيمه اللي جوة السيشن بعد مااحولها ل انتجر 
           var chk = db.ApplyForJobs.Where(a => a.JobId == jobId && a.UserId == UserId).ToList();//to check if this in database or no 
           if (chk.Count < 1)
           {
               var applyjob = new ApplyForJob();
               applyjob.JobId = jobId;
               applyjob.UserId = UserId;
               applyjob.ApplyDate = DateTime.Now;
               applyjob.Message = Message;
               db.ApplyForJobs.Add(applyjob);
               db.SaveChanges();
               ViewBag.Result = "لقد تم الاضافه بنجاح";
           }

           else
               ViewBag.Result = "لقد قمت بالتقدم الي هذه الوظيفة من قبل";
               return View();

        }

        [Authorize]
        public ActionResult GetJobsByUser()
        {
            var userid = User.Identity.GetUserId();//get the current looged user
            var GetUserData = db.ApplyForJobs.Where(a => a.UserId == userid);//الحصول علي معلومات اليوزر الذي قام بتسجيل الدخول
            return View(GetUserData.ToList());
        }

        public ActionResult DetailsOfJob(int id)
        { 
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {           
            var thisjob = db.ApplyForJobs.Find(id);
            if (thisjob==null)
            {
                return HttpNotFound();
            }

            return View(thisjob);
        }
        [HttpPost]
        public ActionResult Edit(ApplyForJob job)
        {
            if (ModelState.IsValid)
            {
             
               job.ApplyDate=DateTime.Now;
               db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GetJobsByUser");
            }

            return View(job);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id==null)
            {
            return  new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var job = db.ApplyForJobs.Find(id);
            if (job==null)
            {
                return HttpNotFound();
            }
            return View(job);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(statusCode:HttpStatusCode.BadRequest);
            }
            var job = db.ApplyForJobs.Find(id);
            if (job==null)
            {
                return HttpNotFound();
            }
                db.ApplyForJobs.Remove(job);
                db.SaveChanges();
                return RedirectToAction("GetJobsByUser");
        }

        [Authorize] //publisher
        public ActionResult GetJobsByPublisher()
        {// بين الجداولjoinدي عملية الحصول علي المعلومات عن طريق
         
            var UserId = User.Identity.GetUserId();
            var Jobs = from app in db.ApplyForJobs
                join jb in db.Jobs
                on app.JobId equals jb.Id
                where jb.User.Id == UserId
                select app;
                
//دي عمليةتجميع المعلومات اللي حصلت عليها وفرزها عن طريق اسم الوظيفه 
                var grob = from item in Jobs
                    group item by item.Job.JobName
                    into asd
                    select new GroupJobTitleViewModel()//هذا ال viemodel من اجل تمرير الداتا الي الفيو
                    {
                        JobtTitle = asd.Key,
                        IcolItems = asd
                    };
                return View(grob.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your applipagedescriptioncation.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(string SearchValue)
        {
            var result = db.Jobs.Where(a => a.JobName.Contains(SearchValue) ||
                                            a.JobDescription.Contains(SearchValue) ||
                                            a.categ.CategoryName.Contains(SearchValue) ||
                                            a.categ.CategoryDescription.Contains(SearchValue)).ToList();
            return View(result);
        }
    }
}
//var UserId = User.Identity.GetUserId();
//var Jobs = from app in db.ApplyForJobs
//join job in db.Jobs
//on app.JobId equals job.Id
//where job.UserId == UserId
//select app;      
                    
       

//return View(Jobs.ToList());