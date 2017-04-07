using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Domains
{
    public class TournamentGroup
    {
        [Key]
        public int TournamentGroupId { get; set; }

        [Required]
        [MaxLength(50)]
        [Index("TournamentGroup_Name_TournamentId_Index", IsUnique = true, Order = 1)]
        [Display(Name = "Group")]
        public string Name { get; set; }
      
       
        [Index("TournamentGroup_Name_TournamentId_Index", IsUnique = true, Order = 2)]
        [Display(Name = "Tournament")]  
        public int TournamentId { get; set; }

        //Realcion:
        [JsonIgnore]
        public virtual Tournament Tournament { get; set; }

        [JsonIgnore]
        public virtual ICollection<TournamentTeam> TournamentTeams { get; set; }

        [JsonIgnore]
        public virtual ICollection<Match> Matches { get; set; }

    }
}
