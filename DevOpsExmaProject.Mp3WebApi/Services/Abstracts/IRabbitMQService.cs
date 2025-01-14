using DevOpsExmaProject.Mp3WebApi.Dtos.Mp3Dtos;

namespace DevOpsExmaProject.Mp3WebApi.Services.Abstracts
{
    public interface IRabbitMQService
    {

        Task AddMp3Comment(string ownerUserName, int mp3Id, string comment);
        Task<List<Mp3CommentDto>> GetMp3Comments(int mp3Id);

    }
}
