using DevOpsExmaProject.Mp3WebApi.Dtos;

namespace DevOpsExmaProject.Mp3WebApi.Services.Abstracts
{
    public interface IFileService
    {
        Task<string> UploadImageAsync(LocalAddFile file);
    }
}
