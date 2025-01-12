using DevOpsExmaProject.Mp3Api.Core.Concretes.EntityFramework;
using DevOpsExmaProject.Mp3Api.DataAccess;
using DevOpsExmaProject.Mp3Api.Repositorise.Abstracts;
using DevOpsExmaProject.Mp3Api.Entitys;

namespace DevOpsExmaProject.Mp3Api.Repositorise.Concretes.EFEntityFramework
{
    public class EFMp3Dal : EFEntityRepositoryBase<Mp3, Mp3DbContext>, IMp3Dal
    {
        public EFMp3Dal(Mp3DbContext context) : base(context)
        {
        }
    }
}
