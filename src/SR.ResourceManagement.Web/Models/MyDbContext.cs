using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SR.ResourceManagement.Domain;

namespace SR.ResourceManagement.Web.Models
{
    public class MyDbContext :DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonPersonType>()
                .HasKey(pt => new {pt.PersonId, pt.PersonTypeId});
            modelBuilder.Entity<PersonPersonType>()
                .HasOne<Person>(pt => pt.Person)
                .WithMany(p => p.PersonPersonType)
                .HasForeignKey(pt => pt.PersonId);
            modelBuilder.Entity<PersonPersonType>()
                .HasOne<PersonType>(pt => pt.PersonType)
                .WithMany(t => t.PersonPersonType)
                .HasForeignKey(pt => pt.PersonTypeId);
        }
        public MyDbContext()
        {
        }


        public DbSet<Person> Persons { get; set; }

        public DbSet<PersonType> PersonTypes { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

    }

    public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=password");
            return new MyDbContext(optionsBuilder.Options);
        }
    }

}