using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Domains
{
    public class League
    {

        [Key]
        public int LeagueId { get; set; }

        [Required]
        [MaxLength(50)]
        [Index("League_Name_index", IsUnique = true)]
        [Display(Name = "League")]
        public string Name { get; set; }

        [DataType(DataType.ImageUrl)]
        public  string Logo{ get; set; }

        [JsonIgnore]
        public virtual ICollection<Team> Teams{ get; set; }

    }
}
