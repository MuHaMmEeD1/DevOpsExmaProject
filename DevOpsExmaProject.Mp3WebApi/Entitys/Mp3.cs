using DevOpsExmaProject.Mp3WebApi.Core.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace DevOpsExmaProject.Mp3WebApi.Entitys
{
    public class Mp3 : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)] 
        public string? Name { get; set; }

        public int LikeCount { get; set; }

        [MaxLength(250)] 
        public string? ImageUrl { get; set; }

        [MaxLength(200)]
        public string? SoundUrl { get; set; }

        public string? UserId { get; set; }

       
        public User? User { get; set; }
    }
}
