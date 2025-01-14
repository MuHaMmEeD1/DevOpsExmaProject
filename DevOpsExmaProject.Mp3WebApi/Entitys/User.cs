using DevOpsExmaProject.Mp3WebApi.Core.Abstracts;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;

namespace DevOpsExmaProject.Mp3WebApi.Entitys
{
    public class User : IdentityUser, IEntity
    {


        public ICollection<Mp3>? mp3s { get; set; }
    }
}
