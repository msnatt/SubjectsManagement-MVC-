using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using SubjectApplication.Models;
using static System.Net.WebRequestMethods;

namespace SubjectApplication.Controllers
{
    public class SubjectsController : Controller
    {
        private SubjectApplicationEntities db = new SubjectApplicationEntities();

        public ActionResult Index()
        {
            var subject = db.Subject.Where(m => m.Isdeleted == false);
            return View(subject);
        }

        // GET: Subjects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subject.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // GET: Subjects/Create
        public ActionResult Create()
        {
            Subject subject = new Subject();
            subject.CreateDate = DateTime.Now;
            subject.UpdateDate = DateTime.Now;
            subject.Isdeleted = false;
            subject.FileSubject = new List<FileSubject>();
            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Subject subject)
        {
            var x = subject.Topic;
            subject.Topic = null;
            var y = subject.FileSubject;
            subject.FileSubject = null;

            db.Subject.Add(subject);
            SaveTopic(x, subject.ID);
            SaveFile(y, subject.ID);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        private void SaveFile(ICollection<FileSubject> fileSubject, int ID)
        {

            var path = "~/UserImages/";
            FileInfo[] fileinfo = new DirectoryInfo(Server.MapPath("~/Temp")).GetFiles("*.*");
            var Path = Server.MapPath(path);
            var fileSubjectList = fileSubject.ToList();
            var fileinfoList = fileinfo.ToList();
            foreach (var filein in fileinfoList)
            {
                var pathFull = Path + DateTime.Now.Ticks + filein.Name;
                foreach (var fileSub in fileSubjectList)
                {
                    if (filein.Name == fileSub.NameFile)
                    {
                        filein.MoveTo(pathFull);
                        fileSub.PathFile = path;
                        fileSub.NameFile = DateTime.Now.Ticks + fileSub.NameFile;
                        db.FileSubject.Add(fileSub);
                    }
                }
            }
        }

        private void SaveTopic(ICollection<Topic> topics, int subjectID)
        {
            foreach (var item in topics)
            {
                if (item.IsDeleted == false)
                {
                    item.SubjectID = subjectID;
                    db.Topic.Add(item);
                    if (item.Topic1.Count > 0)
                    {
                        SaveTopic(item.Topic1, subjectID);
                    }
                }

            }
        }

        // GET: Subjects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subject.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            subject.Topic = subject.Topic.Where(m => m.LevelTopic == 0).ToList();
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Subject subject)
        {
            if (ModelState.IsValid)
            {
                subject.UpdateDate = DateTime.Now;

                UpdateTopic(subject.Topic);
                UpdateFileSubject(subject.FileSubject);

                db.Entry(subject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subject);
        }

        private void UpdateFileSubject(ICollection<FileSubject> fileSubject)
        {
            var path = "~/UserImages/";
            FileInfo[] fileinfo = new DirectoryInfo(Server.MapPath("~/Temp")).GetFiles("*.*");
            var Path = Server.MapPath(path);
            var fileSubjectList = fileSubject.ToList();
            var fileinfoList = fileinfo.ToList();
            foreach (var item in fileSubjectList)
            {
                if (item.ID == 0)
                {
                    var pathFull = Path + DateTime.Now.Ticks + item.NameFile;
                    foreach (var fileInfo in fileinfoList)
                    {
                        if (fileInfo.Name == item.NameFile)
                        {
                            fileInfo.MoveTo(pathFull);
                            item.PathFile = path;
                            item.NameFile = DateTime.Now.Ticks + item.NameFile;
                            db.FileSubject.Add(item);
                        }
                    }
                }
                else
                {
                    if (item.IsDeleted)
                    {
                        db.Entry(item).State = EntityState.Modified;
                    }
                }
            }
        }

        private void UpdateTopic(ICollection<Topic> topics)
        {
            foreach (var item in topics)
            {
                if (item.ID == 0)
                {
                    db.Topic.Add(item);
                }
                else
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                db.SaveChanges();
                if (item.Topic1.Count > 0)
                {
                    UpdateTopic(item.Topic1);
                }
            }
        }

        // GET: Subjects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subject.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Subject subject = db.Subject.Find(id);
            subject.Isdeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //================= AddTopic =================
        public ActionResult AddTopic(Subject subject)
        {
            Topic topic = new Topic();
            topic.IsDeleted = false;
            topic.CreateDate = DateTime.Now;
            topic.UpdateDate = DateTime.Now;
            topic.SubjectID = subject.ID;
            topic.LevelTopic = 0;
            subject.Topic.Add(topic);

            return View("Create", subject);
        }
        public ActionResult AddTopicEdit(Subject subject)
        {
            Topic topic = new Topic();
            topic.IsDeleted = false;
            topic.CreateDate = DateTime.Now;
            topic.UpdateDate = DateTime.Now;
            topic.SubjectID = subject.ID;
            topic.LevelTopic = 0;
            subject.Topic.Add(topic);

            return View("Edit", subject);
        }
        //================= End AddTopic =================


        //================= AddSubTopic =================
        //Create
        public ActionResult AddSubTopic(Subject subject)
        {
            FindTopicForAdd(subject.Topic);
            return View("Create", subject);
        }
        //Edit
        public ActionResult AddSubTopicEdit(Subject subject)
        {
            FindTopicForAdd(subject.Topic);
            return View("Edit", subject);
        }
        private void FindTopicForAdd(ICollection<Topic> topics)
        {
            foreach (var item in topics)
            {
                if (item.IsHere)
                {
                    Topic topic = new Topic();
                    topic.IsDeleted = false;
                    topic.SubjectID = item.SubjectID;
                    topic.TopicHeaderID = item.ID;
                    topic.CreateDate = DateTime.Now;
                    topic.UpdateDate = DateTime.Now;
                    topic.LevelTopic = item.LevelTopic + 1;
                    item.Topic1.Add(topic);
                }
                if (item.Topic1.Count > 0)
                {
                    FindTopicForAdd(item.Topic1);
                }
            }
        }
        //=================== End AddSubTopic ====================



        //==================== Delete Topic ======================
        //Create
        public ActionResult DelTopic(Subject subject)
        {
            FindTopicForDel(subject.Topic);
            return View("Create", subject);
        }
        //Edit
        public ActionResult DelTopicEdit(Subject subject)
        {
            FindTopicForDel(subject.Topic);
            return View("Edit", subject);
        }
        private void FindTopicForDel(ICollection<Topic> topics)
        {
            foreach (var item in topics)
            {
                if (item.IsDeleted)
                {
                    foreach (var item2 in item.Topic1)
                    {
                        item2.IsDeleted = true;
                    }
                }
                if (item.Topic1.Count > 0)
                {
                    FindTopicForAdd(item.Topic1);
                }
            }
        }
        //==================== End Delete Topic =====================

        //=================== Save File To Temp =====================
        //Edit
        public ActionResult SaveFileToTempEdit(HttpPostedFileBase[] ListUploadFile, Subject subject)
        {
            SaveFileToTemp(ListUploadFile, subject);
            return View("Edit", subject);
        }
        //Create
        public ActionResult SaveFileToTempCreate(HttpPostedFileBase[] ListUploadFile, Subject subject)
        {
            SaveFileToTemp(ListUploadFile, subject);
            return View("Create", subject);
        }
        private void SaveFileToTemp(HttpPostedFileBase[] ListUploadFile, Subject subject)
        {
            foreach (var f in ListUploadFile)
            {
                FileSubject fileSubject = new FileSubject();

                fileSubject.NameFile = Path.GetFileName(f.FileName);
                fileSubject.PathFile = "~/Temp";
                fileSubject.IsDeleted = false;
                //fileSubject.IsHidden = false;
                fileSubject.SubjectID = subject.ID;

                subject.FileSubject.Add(fileSubject);

                string FileName = Path.GetFileName(f.FileName);
                string UploadPath = Path.Combine(Server.MapPath("~/Temp"), FileName);
                f.SaveAs(UploadPath);
            }
            ModelState.Clear();
        }
        //========================== End ============================




        //================== Delete File In Temp ====================
        //Create
        public ActionResult DeleteFileInTempCreate(Subject subject)
        {
            DeleteFileInTemp(subject);
            return View("Create", subject);
        }
        //Edit
        public ActionResult DeleteFileInTempEdit(Subject subject)
        {
            DeleteFileInTemp(subject);
            return View("Edit", subject);
        }

        private void DeleteFileInTemp(Subject subject)
        {
            foreach (var f in subject.FileSubject)
            {
                if (f.ID == 0)
                {
                    if (f.IsDeleted)
                    {
                        FileInfo[] fileinfo = new DirectoryInfo(Server.MapPath(f.PathFile)).GetFiles("*.*");
                        foreach (var filein in fileinfo)
                        {
                            if (filein.Name == f.NameFile)
                            {
                                filein.Delete();
                            }
                        }
                        subject.FileSubject.Remove(f);
                        break;
                    }
                }
                else
                {
                    if (f.IsDeleted)
                    {
                        //f.IsHidden = true;
                    }
                }

            }
        }
        //========================== End ============================
    }
}
