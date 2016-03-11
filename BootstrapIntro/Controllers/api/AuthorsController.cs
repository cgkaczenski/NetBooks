using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BootstrapIntro.DAL;
using BootstrapIntro.Models;
using BootstrapIntro.ViewModels;

namespace BootstrapIntro.Controllers.api
{
    public class AuthorsController : ApiController
    {
        private BookContext db = new BookContext();
        // GET: api/Authors
        public ResultList<AuthorViewModel> Get([FromUri]QueryOptions queryOptions)
        {
            var start = (queryOptions.CurrentPage - 1) * queryOptions.PageSize;

            var authors = db.Authors.
            OrderBy(queryOptions.Sort).
            Skip(start).
            Take(queryOptions.PageSize);

            queryOptions.TotalPages =
            (int)Math.Ceiling((double)db.Authors.Count() / queryOptions.PageSize);

            AutoMapper.Mapper.CreateMap<Author, AuthorViewModel>();

            return new ResultList<AuthorViewModel>(
                AutoMapper.Mapper.Map<List<Author>, List<AuthorViewModel>>
                    (authors.ToList()), queryOptions);
        }

        // GET: api/Authors/5
        [ResponseType(typeof(AuthorViewModel))]
        public IHttpActionResult Get(int id)
        {
            Author author = db.Authors.Find(id);

            if (author == null)
            {
                throw new System.Data.Entity.Core.ObjectNotFoundException
                (string.Format("Unable to find author with id {0}", id));
            }

            AutoMapper.Mapper.CreateMap<Author, AuthorViewModel>();

            return Ok(AutoMapper.Mapper.Map<Author, AuthorViewModel>(author));
        }

        // PUT: api/Authors/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Put(AuthorViewModel author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            AutoMapper.Mapper.CreateMap<AuthorViewModel, Author>();
            db.Entry(AutoMapper.Mapper.Map<AuthorViewModel, Author>(author)).State
            = EntityState.Modified;
            db.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Authors
        [ResponseType(typeof(AuthorViewModel))]
        public IHttpActionResult Post(AuthorViewModel author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            AutoMapper.Mapper.CreateMap<AuthorViewModel, Author>();
            db.Authors.Add(AutoMapper.Mapper.Map<AuthorViewModel, Author>(author));
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = author.Id }, author);
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
