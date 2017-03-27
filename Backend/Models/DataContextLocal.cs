using Domains;

namespace Backend.Models
{

    public class DataContextLocal : DataContext
    {
        public System.Data.Entity.DbSet<Domains.Date> Dates { get; set; }
    }
}