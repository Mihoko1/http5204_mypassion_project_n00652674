using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace http5204_mypassion_project_n00652674.Models
{
    public class Hairstyle
    {
        [Key]
        public int HairstyleID { get; set; }

        public DateTime DateUpload { get; set; }

        public string HairstylePhoto { get; set; }

        public string Type { get; set; }

        public string Detail { get; set; }

        // A Hairstyle has one Member
        [ForeignKey("Member")]
        public int MemberID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual Member Member { get; set; }
    }

    public class HairstyleDto
    {
        [Key]
        public int HairstyleID { get; set; }

        [DisplayName("Date Upload")]
        public DateTime DateUpload { get; set; }

        [DisplayName("Hairstyle Photo")]
        public string HairstylePhoto { get; set; }

        public string Type { get; set; }

        public string Detail { get; set; }

        public int MemberID { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

    }
}