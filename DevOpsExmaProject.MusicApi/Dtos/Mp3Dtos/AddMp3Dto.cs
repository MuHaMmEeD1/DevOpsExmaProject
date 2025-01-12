namespace DevOpsExmaProject.Mp3Api.Dtos.Mp3Dtos
{
    public class AddMp3Dto
    {
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public IFormFile? ImageFile { get; set; }
        public IFormFile? Mp3File { get; set; }



    }
}
