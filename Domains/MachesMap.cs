using System.Data.Entity.ModelConfiguration;

namespace Domains
{
    internal class MachesMap : EntityTypeConfiguration<Match>
    {
        public MachesMap()
        {
            HasRequired(o => o.Local).WithMany(m => m.Locals).HasForeignKey(m => m.LocalId);

            HasRequired(o => o.Visitor).WithMany(m => m.Visitors).HasForeignKey(m => m.VisitorId);
        }
    }
}