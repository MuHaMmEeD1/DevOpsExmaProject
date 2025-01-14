using DevOpsExmaProject.Mp3WebApi.Core.Concretes.EntityFramework;
using DevOpsExmaProject.Mp3WebApi.DataAccess;
using DevOpsExmaProject.Mp3WebApi.Entitys;
using DevOpsExmaProject.Mp3WebApi.Repositorise.Abstracts;

namespace DevOpsExmaProject.Mp3WebApi.Repositorise.Concretes.EFEntityFramework
{
    public class EFMp3Dal : EFEntityRepositoryBase<Mp3, Mp3DbContext>, IMp3Dal
    {
        public EFMp3Dal(Mp3DbContext context) : base(context)
        {
        }
    }
}
