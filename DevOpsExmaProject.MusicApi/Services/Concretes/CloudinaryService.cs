using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DevOpsExmaProject.Mp3Api.Dtos;
using DevOpsExmaProject.Mp3Api.Services.Abstracts;
using DevOpsExmaProject.Mp3Api.Settings;

namespace DevOpsExmaProject.Mp3Api.Services.Concretes
{
    public class CloudinaryService : IColudinaryService
    {

        private IConfiguration _configuration;
        private CloudinarySettings _cloudinarySettings;
        private Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            _configuration = configuration;
            _cloudinarySettings = _configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();
            Account account = new Account(_cloudinarySettings.CloudName, _cloudinarySettings.ApiKey, _cloudinarySettings.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(ClodinaryAddFile dto)
        {
            ImageUploadResult uploadedResult = new ImageUploadResult();

            IFormFile? file = dto.File;
            if (file?.Length > 0)
            {
                using (Stream? stream = file.OpenReadStream())
                {
                    ImageUploadParams uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.Name, stream)
                    };
                    uploadedResult = await _cloudinary.UploadAsync(uploadParams);

                    if (uploadedResult != null)
                    {
                        return uploadedResult.Url.ToString();
                    }
                }
            }
            return "";
        }

        public async Task<string> UploadMp3Async(ClodinaryAddFile dto)
        {
            RawUploadResult uploadedResult = new RawUploadResult();

            IFormFile? file = dto.File;
            if (file?.Length > 0)
            {
                using (Stream? stream = file.OpenReadStream())
                {
                    var fileDescription = new FileDescription(file.Name, stream);

                    RawUploadParams uploadParams = new RawUploadParams
                    {
                        File = fileDescription
                    };

                    uploadedResult = await _cloudinary.UploadAsync(uploadParams);

                    if (uploadedResult != null && !string.IsNullOrEmpty(uploadedResult.Url?.ToString()))
                    {
                        return uploadedResult.Url.ToString();
                    }
                }
            }
            return "";
        }



    }
}
