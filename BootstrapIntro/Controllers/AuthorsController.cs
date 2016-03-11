using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BootstrapIntro.DAL;
using BootstrapIntro.Models;
using BootstrapIntro.ViewModels;
using System.Web.ModelBinding;
using BootstrapIntro.Filters;

namespace BootstrapIntro.Controllers
{
    public class AuthorsController : Controller
    {
        private BookContext db = new BookContext();

        // GET: Authors
        [GenerateResultListFilterAttribute(typeof(Author), typeof(AuthorViewModel))]
        public ActionResult Index([Form] QueryOptions queryOptions)
        {
            var start = (queryOptions.CurrentPage - 1) * queryOptions.PageSize; 

            var authors = db.Authors
                .OrderBy(queryOptions.Sort)
                .Skip(start)
                .Take(queryOptions.PageSize);

            queryOptions.TotalPages =
                (int)Math.Ceiling((double)db.Authors.Count() / queryOptions.PageSize);

            ViewData["QueryOptions"] = queryOptions;
            return View(authors.ToList());
        }

        // GET: Authors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                throw new System.Data.Entity.Core.ObjectNotFoundException
                    (string.Format("Unable to find author with id {0}", id));
            }
            return View(author);
        }

        // GET: Authors/Create
        public ActionResult Create()
        {
            return View("Form", new AuthorViewModel());
        }

        // POST: Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Biography")]
                                    AuthorViewModel author)
        {
            if (ModelState.IsValid)
            {
                AutoMapper.Mapper.CreateMap<AuthorViewModel, Author>();
                db.Authors.Add(AutoMapper.Mapper.Map<AuthorViewModel, Author>(author));
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(author);
        }

        // GET: Authors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Author author = db.Authors.Find(id);

            if (author == null)
            {
                return HttpNotFound();
            }

            AutoMapper.Mapper.CreateMap<Author, AuthorViewModel>();
            return View("Form", AutoMapper.Mapper.Map<Author, AuthorViewModel>(author));
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Biography")]
        AuthorViewModel author)
        {
            if (ModelState.IsValid)
            {
                AutoMapper.Mapper.CreateMap<AuthorViewModel, Author>();
                db.Entry(AutoMapper.Mapper.Map<AuthorViewModel, Author>(author)).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(author);
        }

        // GET: Authors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Author author = db.Authors.Find(id);

            if (author == null)
            {
                return HttpNotFound();
            }

            AutoMapper.Mapper.CreateMap<Author, AuthorViewModel>();
            return View(AutoMapper.Mapper.Map<Author, AuthorViewModel>(author));
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Author author = db.Authors.Find(id);
            db.Authors.Remove(author);
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
    }
}
