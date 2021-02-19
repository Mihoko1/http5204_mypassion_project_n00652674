using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace http5204_mypassion_project_n00652674.Models.ViewModels
{
    public class ShowSalon
    {
        public SalonDto salon { get; set; }

        //Information about all players on that team
        public IEnumerable<MemberDto> salonmembers { get; set; }
    }
}