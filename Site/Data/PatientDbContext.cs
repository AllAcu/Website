using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllAcu.Models.Patients;
using Microsoft.Its.Domain.Sql;

namespace AllAcu
{
    public class PatientDbContext : ReadModelDbContext
    {
        public static string ConnectionString;

        public DbSet<PatientPersonalInformation> Patients { get; set; }

        public PatientDbContext()
            : base(ConnectionString ?? NameOrConnectionString)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientPersonalInformation>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<PatientPersonalInformation>()
                .Ignore(i => i.Address);
        }
    }
}
