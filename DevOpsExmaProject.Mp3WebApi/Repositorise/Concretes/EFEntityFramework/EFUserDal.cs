using DevOpsExmaProject.Mp3WebApi.Core.Concretes.EntityFramework;
using DevOpsExmaProject.Mp3WebApi.DataAccess;
using DevOpsExmaProject.Mp3WebApi.Entitys;
using DevOpsExmaProject.Mp3WebApi.Repositorise.Abstracts;

namespace DevOpsExmaProject.Mp3WebApi.Repositorise.Concretes.EFEntityFramework
{
    public class EFUserDal : EFEntityRepositoryBase<User, Mp3DbContext>, IUserDal
    {
        public EFUserDal(Mp3DbContext context) : base(context)
        {
        }
    }
}
