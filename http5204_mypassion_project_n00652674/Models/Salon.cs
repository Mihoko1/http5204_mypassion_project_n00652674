using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace http5204_mypassion_project_n00652674.Models
{
    public class Salon
    {
        [Key]
        public int SalonID { get; set; }

        public string SalonName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Area { get; set; }

        public string Website { get; set; }

        public string Phone { get; set; }

        public string SalonPicture { get; set; }

        // A salon can have many members
        public ICollection<Member> Members { get; set; }

    }

    public class SalonDto
    {
        [DisplayName("Salon ID")]
        public int SalonID { get; set; }

        [DisplayName("Salon Name")]
        public string SalonName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Area { get; set; }

        public string Website { get; set; }

        public string Phone { get; set; }

        public string SalonPicture { get; set; }
    }
}