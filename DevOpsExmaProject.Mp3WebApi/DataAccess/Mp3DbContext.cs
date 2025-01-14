using DevOpsExmaProject.Mp3WebApi.Entitys;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevOpsExmaProject.Mp3WebApi.DataAccess
{
    public class Mp3DbContext : IdentityDbContext<User>
    {
        public Mp3DbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Mp3> Mp3s { get; set; }



    }
}
