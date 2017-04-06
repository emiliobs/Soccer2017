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
    public class UserView : User
    {
        [Display(Name = "Picture")]
        public HttpPostedFileBase PictureFile { get; set; }

        [Display(Name = "Favorite League")]
        public int  FavoriteLeagueId { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required]
        [StringLength(20), MinLength(8)]
        public string Password { get; set; }

        [Display(Name = "Password Confirm")]
        [Compare("Password", ErrorMessage = "The password an confirm does not match.")]
        [DataType(DataType.Password)]
        [Required]
        [StringLength(20), MinLength(8)]
        public string PasswordConfirm { get; set; }


    }
}