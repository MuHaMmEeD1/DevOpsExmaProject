namespace DevOpsExmaProject.Mp3Api.Dtos.Mp3Dtos
{
    public class GetMp3Dto
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public int LikeCount { get; set; }
        public string? ImageUrl { get; set; }
        public string? SoundUrl { get; set; }
        public bool Favorite { get; set; }
        public string? OwnerName { get; set; }

    }
}
