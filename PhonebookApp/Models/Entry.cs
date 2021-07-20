using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PhonebookApp.Models
{
    public class Entry
    {
        public int ID { get; set; }

        public Nullable<int> PhonebookID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [DisplayName("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number(Phone number should be 10 digits long)")]
        public string PhoneNumber { get; set; }

        public virtual Phonebook Phonebook { get; set; }
    }
}