using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains
{
    public class Tournament
    {

        [Key]
        public int TournamentId { get; set; }

        [Required]
        [MaxLength(50)]
        [Index("Tournament_Name_index", IsUnique = true)]
        [Display(Name = "Tournament")]
        public string Name { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Logo { get; set; }

        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }

        [Display(Name = "Order")]
        public int Order { get; set; }

        //Relacion:
        public virtual ICollection<TournamentGroup> TournamentGroup { get; set; }
        public virtual ICollection<Date> Dates { get; set; }


    }
}
