using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Domains;

namespace Backend.Models
{
    [NotMapped]
    public class LeagueView : League
    {
        public HttpPostedFileBase LogoFile { get; set; }
    }
}