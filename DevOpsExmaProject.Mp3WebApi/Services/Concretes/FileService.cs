using DevOpsExmaProject.Mp3WebApi.Dtos;
using DevOpsExmaProject.Mp3WebApi.Services.Abstracts;

public class FileService : IFileService
{
    private readonly string _uploadPath;

    public FileService(IConfiguration configuration)
    {
        _uploadPath = "/app/images";  
    }

    public async Task<string> UploadImageAsync(LocalAddFile file)
    {
        if (file.File == null || file.File.Length == 0)
        {
            throw new ArgumentException("No file provided.");
        }

        var fileName = Path.GetFileName(file.File.FileName);
        var filePath = Path.Combine(_uploadPath, fileName);

        Directory.CreateDirectory(_uploadPath);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.File.CopyToAsync(stream);
        }

        return $"/images/{fileName}";
    }
}
