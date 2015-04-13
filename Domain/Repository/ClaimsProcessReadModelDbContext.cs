using System.Data.Entity;
using Microsoft.Its.Domain.Sql;

namespace Domain.Repository
{
    public class ClaimsProcessReadModelDbContext : ReadModelDbContext
    {
        public static string ConnectionString;

        public ClaimsProcessReadModelDbContext()
            : base(ConnectionString ?? NameOrConnectionString)
        {

        }
        public ClaimsProcessReadModelDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString ?? NameOrConnectionString)
        {
        }

        //public DbSet<>
            
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
