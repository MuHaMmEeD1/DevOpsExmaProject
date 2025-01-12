namespace DevOpsExmaProject.Mp3Api.Services.Abstracts
{
    public interface IRedisService
    {

        bool CheckFavorite(string userId, int mp3Id);
        void ChangeFavorite(string userId, int mp3Id);

    }
}
