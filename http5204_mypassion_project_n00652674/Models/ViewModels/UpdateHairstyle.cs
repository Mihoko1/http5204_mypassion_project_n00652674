using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace http5204_mypassion_project_n00652674.Models.ViewModels
{
    public class UpdateHairstyle
    {
        public HairstyleDto hairstyle { get; set; }
        //Needed for a dropdownlist which presents the player with a choice of teams to play for
        public IEnumerable<MemberDto> allmembers { get; set; }
    }
}