namespace DevOpsExmaProject.Mp3WebApi.Services.Abstracts
{
    public interface IRedisService
    {

        bool CheckFavorite(string userId, int mp3Id);
        void ChangeFavorite(string userId, int mp3Id);

    }
}
