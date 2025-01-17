using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DevOpsExmaProject.Mp3WebApi.Entitys;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DevOpsExmaProject.Mp3WebApi.DataAccess
{
    public class Mp3DbContext : IdentityDbContext<User>
    {
        public Mp3DbContext(DbContextOptions<Mp3DbContext> options) : base(options)
        {
        }

        public DbSet<Mp3> Mp3s { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(string) && property.GetColumnType() == null)
                    {
                        property.SetColumnType("text");
                    }
                }
            }
        }
    }
}
