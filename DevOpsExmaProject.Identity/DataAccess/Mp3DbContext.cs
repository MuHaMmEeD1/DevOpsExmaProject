using DevOpsExmaProject.Identity.Entitys;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevOpsExmaProject.Identity.DataAccess
{
    public class Mp3DbContext : IdentityDbContext<User>
    {

       
        public Mp3DbContext(DbContextOptions options) : base(options)
        {
        }

    }
}
