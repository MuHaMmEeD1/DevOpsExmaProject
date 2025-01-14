using DevOpsExmaProject.Mp3Api.Core.Abstracts;
using Microsoft.AspNetCore.Identity;

namespace DevOpsExmaProject.Mp3Api.Entitys
{
    public class User : IdentityUser, IEntity
    {


        public ICollection<Mp3>? mp3s { get; set; }
    }
}
