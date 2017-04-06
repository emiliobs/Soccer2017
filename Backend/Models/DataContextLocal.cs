using Domains;

namespace Backend.Models
{

    public class DataContextLocal : DataContext
    {
        public System.Data.Entity.DbSet<Domains.Date> Dates { get; set; }

        public System.Data.Entity.DbSet<Domains.TournamentTeam> TournamentTeams { get; set; }

        public System.Data.Entity.DbSet<Domains.UserType> UserTypes { get; set; }

        public System.Data.Entity.DbSet<Domains.User> Users { get; set; }

        public System.Data.Entity.DbSet<Domains.Status> Status { get; set; }

        public System.Data.Entity.DbSet<Domains.Match> Matches { get; set; }

        public System.Data.Entity.DbSet<Domains.Predictions> Predictions { get; set; }

        public System.Data.Entity.DbSet<Domains.Group> Groups { get; set; }

        public System.Data.Entity.DbSet<Domains.GroupUser> GroupUsers { get; set; }
    }
}