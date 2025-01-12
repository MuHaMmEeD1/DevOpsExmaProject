using DevOpsExmaProject.Identity.Core.Abstracts;
using Microsoft.AspNetCore.Identity;

namespace DevOpsExmaProject.Identity.Entitys
{
    public class User : IdentityUser, IEntity
    {
    }
}
