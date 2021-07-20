using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PhonebookAPI.Models;

namespace PhonebookAPI.Controllers
{
    public class EntryController : ApiController
    {
        private PhonebookEntities db = new PhonebookEntities();
        List<Entry> entries = new List<Entry>();
        public EntryController()
        {
            this.entries = db.Entries.ToList();
        }
        public EntryController(PhonebookEntities dbContext)
        {
            db = dbContext;
        }
        public EntryController(List<Entry> entries)
        {
            this.entries = entries;
        }
        // GET: api/Entry
        public IEnumerable<Entry> GetEntries()
        {
            return entries;
        }

        // POST: api/Entry        
        public HttpResponseMessage PostEntry([FromBody] Entry entry)
        {
            if (ModelState.IsValid)
            {
                var count = db.Entries.Where(x => x.Name.Equals(entry.Name, StringComparison.OrdinalIgnoreCase) && x.PhoneNumber.Equals(entry.PhoneNumber) && x.PhonebookID == entry.PhonebookID).Count();
                if (count > 0) 
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Found, "A contact with the name: " + entry.Name + " and number: " + entry.PhoneNumber + " already exists."); 
                }

                db.Entries.Add(entry);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, entry);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = entry.ID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
    }
}
