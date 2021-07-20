using PhonebookApp.Models;
using System.Net.Http;
using System.Web.Mvc;

namespace PhonebookApp.Controllers
{
    public class PhonebookController : Controller
    {

        public ActionResult Add()
        {
            return View();
        }
        // POST: Phonebook/Create
        [HttpPost]
        public ActionResult Add(Phonebook phonebook)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Phonebook", phonebook).Result;
            if(response.StatusCode == System.Net.HttpStatusCode.Created)
                TempData["SuccesMessage"] = "Phonebook saved Successfully.";
            else
                TempData["SuccesMessage"] = "Phonebook name already exist. Not saved!";

            return RedirectToAction("Index", "Entry");
        }
    }
}

