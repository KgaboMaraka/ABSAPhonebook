using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PhonebookAPI.Models;

namespace PhonebookAPI.Controllers
{
    public class PhonebookController : ApiController
    {
        private PhonebookEntities db = new PhonebookEntities();
        List<Phonebook> phonebooks = new List<Phonebook>();
        public PhonebookController()
        {
            this.phonebooks = db.Phonebooks.ToList();
        }
        public PhonebookController(PhonebookEntities dbContext)
        {
            db = dbContext;
        }
        public PhonebookController(List<Phonebook> phonebooks)
        {
            this.phonebooks = phonebooks;
        }
        // GET: api/Phonebook

        public IEnumerable<Phonebook> Get()
        {
            return phonebooks;
        }

        // POST: api/Phonebook
        public HttpResponseMessage Post([FromBody]Phonebook phonebook)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var p = db.Phonebooks.Where(b => b.Name.Equals(phonebook.Name, StringComparison.OrdinalIgnoreCase)).Count();
                    if (p > 0) 
                    { 
                        return Request.CreateErrorResponse(HttpStatusCode.Found, "A phonebook with the name: " + phonebook.Name + " already exists"); 
                    }

                    db.Phonebooks.Add(phonebook);
                    db.SaveChanges();

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, phonebook);
                    response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = phonebook.ID }));
                    return response;
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
            }
            catch (Exception)
            {
                throw;
            }
            
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
