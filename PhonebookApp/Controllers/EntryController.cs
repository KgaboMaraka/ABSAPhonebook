using PhonebookApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;

namespace PhonebookApp.Controllers
{
    public class EntryController : Controller
    {
        // GET: Entry
        public ActionResult Index(string searchString)
        {
            IEnumerable<Entry> entryList;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Entry").Result;
            entryList = response.Content.ReadAsAsync<IEnumerable<Entry>>().Result;

            if (!String.IsNullOrEmpty(searchString))
            {
                entryList = entryList.Where(e => e.Name.Trim().ToLower().Contains(searchString.Trim().ToLower())
                                       || e.PhoneNumber.Trim().Contains(searchString.Trim())
                                       || e.Phonebook.Name.Trim().ToLower().Contains(searchString.Trim().ToLower()));
            }

            return View(entryList);
        }

        public ActionResult Add()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Phonebook").Result;

            IEnumerable<Phonebook> phonebookList = response.Content.ReadAsAsync<IEnumerable<Phonebook>>().Result;
            ViewBag.PhonebookList = new SelectList(phonebookList, "ID", "Name");
            return View();
        }

        // POST: Entry/Create
        [HttpPost]
        public ActionResult Add(Entry entry)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Entry", entry).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                TempData["SuccesMessage"] = "Contact added Successfully.";
            else
                TempData["SuccesMessage"] = "Contact name and number already exist. Not saved!";
            
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            HttpResponseMessage phonebookResponse = GlobalVariables.WebApiClient.GetAsync("Phonebook").Result;
            IEnumerable<Phonebook> phonebookList = phonebookResponse.Content.ReadAsAsync<IEnumerable<Phonebook>>().Result;
            ViewBag.PhonebookList = new SelectList(phonebookList, "ID", "Name");

            HttpResponseMessage entryResponse = GlobalVariables.WebApiClient.GetAsync("Entry/" + id).Result;
            Entry entry = entryResponse.Content.ReadAsAsync<Entry>().Result;

            return View(entry);
        }

        // PUT: Entry/Put
        [HttpPost]
        public ActionResult Edit(Entry entry)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("Entry", entry).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                TempData["SuccesMessage"] = "Contact updated Successfully.";
            else
                TempData["SuccesMessage"] = "Contact not updated!";

            return RedirectToAction("Index");
        }

        // DELETE: Entry/Delete
        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Entry/" + id).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                TempData["SuccesMessage"] = "Contact deleted Successfully.";
            else
                TempData["SuccesMessage"] = "Contact not deleted!";

            return RedirectToAction("Index");
        }
    }
}
