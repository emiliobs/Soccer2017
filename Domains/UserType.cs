using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains
{
    public class UserType
    {
        [Key]
        public int UserTypeId { get; set; }

        [Required]
        [MaxLength(50), MinLength(1)]
        [Display(Name = "User Type")]
        [Index("UserType_Name_Index", IsUnique = true)]
        public string Name { get; set; }

        public virtual  ICollection<User> Users { get; set; }
    }
}
