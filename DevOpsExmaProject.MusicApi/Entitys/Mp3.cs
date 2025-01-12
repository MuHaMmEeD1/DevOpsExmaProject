using DevOpsExmaProject.Mp3Api.Core.Abstracts;
using DevOpsExmaProject.Mp3Api.Entitys;

namespace DevOpsExmaProject.Mp3Api.Entitys
{
    public class Mp3 : IEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int LikeCount { get; set; }
        public string? ImageUrl { get; set; }
        public string? SoundUrl { get; set; }

        public string? UserId { get; set; }
        public User? User { get; set; }
         
    }
}
