using DevOpsExmaProject.Mp3WebApi.Dtos;

namespace DevOpsExmaProject.Mp3WebApi.Services.Abstracts
{
    public interface IColudinaryService
    {
        Task<string> UploadImageAsync(ClodinaryAddFile file);
        Task<string> UploadMp3Async(ClodinaryAddFile file);


    }
}
