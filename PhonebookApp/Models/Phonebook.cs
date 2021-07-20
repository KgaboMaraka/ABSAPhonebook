using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PhonebookApp.Models
{
    public class Phonebook
    {
        public Phonebook()
        {
            this.Entries = new HashSet<Entry>();
        }

        [Display(Name = "PhonebookID")]
        public int ID { get; set; }

        [DisplayName("Phonebook Name")]
        public string Name { get; set; }

        public virtual ICollection<Entry> Entries { get; set; }
        public IEnumerable<SelectListItem> Phonebooks { get; set; }
    }
}