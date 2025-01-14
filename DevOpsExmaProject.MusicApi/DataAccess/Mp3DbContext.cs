using DevOpsExmaProject.Mp3Api.Entitys;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevOpsExmaProject.Mp3Api.DataAccess
{
    public class Mp3DbContext : IdentityDbContext<User>
    {
        public Mp3DbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Mp3> Mp3s { get; set; }



    }
}
