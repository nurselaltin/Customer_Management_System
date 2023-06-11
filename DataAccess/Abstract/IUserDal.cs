using Core.CommonModel.Result;
using Core.ORM;
using Entities.Concrete;

namespace DataAccess.Abstract
{
  public interface IUserDal : IRepositoryBase<User>
  {
        ServiceResult<int> GetUserID(string userName, string password);
  }
}
