using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Domains;

namespace Backend.Models
{
    [NotMapped]
    public class MatchView : Match
    {
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public string DateString { get; set; }

        [Display(Name = "Local League")]
        [DataType(DataType.Time)]
        public string TimeString { get; set; }

        [Display(Name = "Local League")]
        public int LocalLeagueId { get; set; }

         [Display(Name = "Visitor League")]
        public int VisitorLeagueId { get; set; }

    }
}