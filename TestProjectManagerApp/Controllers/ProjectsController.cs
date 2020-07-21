using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TestProjectManagerApp.Data;
using TestProjectManagerApp.Models;
using TestProjectManagerApp.Infrastructure;
using System.Security.AccessControl;
using PagedList;
using System.Web.UI.WebControls;

namespace TestProjectManagerApp.Controllers
{
    public class ProjectsController : Controller
    {
        private TestProjectManagerAppContext db = new TestProjectManagerAppContext();

        // GET: 
        [CustomAuthorize("Admin", "User")]
        public ActionResult Index(string searchStringTitle, string searchStringOrganization, string searchStringType, int? page)
        {
            var projects = from p in db.Projects select p;

            projects = projects.OrderBy(p => p.Id);

            if (!string.IsNullOrEmpty(searchStringTitle))
            {
                projects = projects.Where(p => p.Title.Contains(searchStringTitle));
            }
            if (!string.IsNullOrEmpty(searchStringOrganization))
            {
                projects = projects.Where(p => p.Organization.Contains(searchStringOrganization));
            }
            if (!string.IsNullOrEmpty(searchStringType))
            {
                projects = projects.Where(p => p.Type.ToString().Equals(searchStringType));
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(projects.ToPagedList(pageNumber, pageSize));
            //return View(await projects.ToListAsync());
        }

        // GET: Projects/Details/5
        [CustomAuthorize("Admin", "User")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.Include(p => p.Attachments).SingleOrDefaultAsync(p => p.Id == id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        [CustomAuthorize("Admin", "User")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Admin", "User")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Description,Organization,Start,End,Role,Link,Skills,Attachments,Type,Created,Updated")] Project project, List<HttpPostedFileBase> uploads)
        {
            if (ModelState.IsValid)
            {
                var files = GetFiles(uploads);
                if (files.Count > 0)
                    project.Attachments = files;

                db.Projects.Add(project);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        //Download file from Db
        [CustomAuthorize("Admin", "User")]
        public FileResult Download(string FileId)
        {
            var id = Int32.Parse(FileId);
            var fileToRetrieve = db.Files.Find(id);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }
        // GET: Projects/Edit/5
        [CustomAuthorize("Admin", "User")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Admin", "User")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,Organization,Start,End,Role,Link,Skills,Attachments,Type,Created,Updated")] Project project, List<HttpPostedFileBase> uploads)
        {
            if (ModelState.IsValid)
            {
                var files = GetFiles(uploads);
                if (files.Count > 0)
                {
                    project.Attachments = files;
                }
                var proj = db.Projects.Find(project.Id);
                proj.Created = project.Created;
                proj.Description = project.Description;
                proj.End = project.End;
                proj.Start = project.Start;
                proj.Link = project.Link;
                proj.Organization = project.Organization;
                proj.Role = project.Role;
                proj.Skills = project.Skills;
                proj.Title = project.Title;
                proj.Type = project.Type;
                proj.Updated = project.Updated;
                proj.Attachments = project.Attachments;
                //db.Entry(project).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            return View(project);
        }

        // GET: Projects/Delete/5
        [CustomAuthorize("Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Project project = await db.Projects.FindAsync(id);
            db.Projects.Remove(project);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        private List<File> GetFiles(List<HttpPostedFileBase> uploads)
        {
            List<File> files = new List<File>();
            foreach (var upload in uploads)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var file = new File
                    {
                        FileName = System.IO.Path.GetFileName(upload.FileName),
                        ContentType = upload.ContentType

                    };
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {

                        file.Content = reader.ReadBytes(upload.ContentLength);
                    }
                    files.Add(file);
                }
            }
            return files;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
