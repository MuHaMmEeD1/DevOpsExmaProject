using DevOpsExmaProject.Mp3Api.Dtos;

namespace DevOpsExmaProject.Mp3Api.Services.Abstracts
{
    public interface IColudinaryService
    {
        Task<string> UploadImageAsync(ClodinaryAddFile file);
        Task<string> UploadMp3Async(ClodinaryAddFile file);


    }
}
