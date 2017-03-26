using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public DbSet<League> Leagues { get; set; }    
        public DbSet<Team> Teams { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }    
        public DbSet<TournamentGroup> TournamentGroups { get; set; }
    }
}
