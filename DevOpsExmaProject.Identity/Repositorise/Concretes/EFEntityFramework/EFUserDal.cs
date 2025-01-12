using DevOpsExmaProject.Identity.Core.Concretes.EntityFramework;
using DevOpsExmaProject.Identity.DataAccess;
using DevOpsExmaProject.Identity.Entitys;
using DevOpsExmaProject.Identity.Repositorise.Abstracts;

namespace DevOpsExmaProject.Identity.Repositorise.Concretes.EFEntityFramework
{
    public class EFUserDal : EFEntityRepositoryBase<User, Mp3DbContext>, IUserDal
    {
        public EFUserDal(Mp3DbContext context) : base(context)
        {
        }
    }
}
