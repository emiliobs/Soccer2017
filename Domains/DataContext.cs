using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Domains
{
    public class DataContext : DbContext
    {

        public DataContext() : base("DefaultConnection")
        {

        }

        //evitar borrdo en csacada:
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new UsersMap());
            modelBuilder.Configurations.Add(new MachesMap());
            modelBuilder.Configurations.Add(new GroupsMap());
        }

        public DbSet<League> Leagues { get; set; }    
        public DbSet<Team> Teams { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }    
        public DbSet<TournamentGroup> TournamentGroups { get; set; }

        public System.Data.Entity.DbSet<Domains.User> Users { get; set; }

        public System.Data.Entity.DbSet<Domains.UserType> UserTypes { get; set; }
    }
}
