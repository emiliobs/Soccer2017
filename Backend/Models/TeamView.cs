using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Domains;

namespace Backend.Models
{
    [NotMapped]
    public class TeamView  : Team
    {
        public HttpPostedFileBase LogoFile { get; set; }

    }
}