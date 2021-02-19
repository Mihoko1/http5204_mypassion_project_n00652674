using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace http5204_mypassion_project_n00652674.Models.ViewModels
{
    public class UpdateSalon
    {
        public SalonDto salon { get; set; }
      
        public IEnumerable<MemberDto> allmembers { get; set; }
      
    }
}