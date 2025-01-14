using DevOpsExmaProject.Mp3Api.Core.Concretes.EntityFramework;
using DevOpsExmaProject.Mp3Api.DataAccess;
using DevOpsExmaProject.Mp3Api.Entitys;
using DevOpsExmaProject.Mp3Api.Repositorise.Abstracts;

namespace DevOpsExmaProject.Mp3Api.Repositorise.Concretes.EFEntityFramework
{
    public class EFUserDal : EFEntityRepositoryBase<User, Mp3DbContext> , IUserDal
    {
        public EFUserDal(Mp3DbContext context) : base(context)
        {
        }
    }
}
