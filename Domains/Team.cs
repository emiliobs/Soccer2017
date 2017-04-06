using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        [Required]
        [MaxLength(50)]
        [Index("Team_Name_LeagueId_Index", IsUnique = true, Order = 1)]
        [Display(Name = "Team")]
        public string Name { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Logo { get; set; }

        [StringLength(3), MinLength(3)]
        [Index("TeamInitials _Name_LeagueId_Index", IsUnique = true, Order = 1)]
        public string Initials { get; set; }

        [Index("TeamInitials _Name_LeagueId_Index", IsUnique = true, Order = 2)]
        [Index("Team_Name_LeagueId_Index", IsUnique = true, Order = 2)]
        [Display(Name = "League")]
        public int LeagueId { get; set; }

        //relations                    
        public virtual League  League{ get; set; }
        public virtual ICollection<TournamentTeam> TournamentTeams { get; set; }
        public virtual ICollection<User> Fans { get; set; }
        public virtual ICollection<Match> Locals { get; set; }
        public virtual ICollection<Match> Visitors { get; set; }


    }
}
