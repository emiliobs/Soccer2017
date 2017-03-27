using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains
{
    public class Date
    {
        [Key]
        public int DateId { get; set; }    
        
        [Required]
        [MaxLength(50)]
        [Index("Date_Name_TournamentId_Index", IsUnique = true, Order=1)]
        [Display(Name = "Date")]
        public string Name { get; set; }

        [Index("Date_Name_TournamentId_Index", IsUnique = true, Order = 2)]
        public int TournamentId { get; set; }           
        public virtual Tournament Tournament { get; set; }
    }
}
