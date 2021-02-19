using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace http5204_mypassion_project_n00652674.Models
{
    public class Member
    {
        [Key]
        public int MemberID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Title { get; set; }

        public string Email { get; set; }

        public string Picture { get; set; }

        // A member works for one salon
        [ForeignKey("Salon")]
        public int SalonID { get; set; }

        public string SalonName { get; set; }

        public virtual Salon Salon { get; set; }

        public ICollection<Hairstyle> Hairstyles { get; set; }
    }

    public class MemberDto
    {
        public int MemberID { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public string Title { get; set; }

        public string Email { get; set; }

        public string Picture { get; set; }

        public int SalonID { get; set; }

        public string SalonName { get; set; }


    }
}