
using Core.CommonModel.Result;
using Core.ORM.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;

namespace DataAccess.Concrete
{
    public class UserDal : EfRepositoryBase<User, MTSContext>, IUserDal
    {
        public ServiceResult<int> GetUserID(string userName, string password)
        {
            ServiceResult<int> res = new ServiceResult<int>();
            using (var context = new MTSContext())
            {
                try
                {
                    res.Data = context.Set<User>().FirstOrDefault(x => x.UserName == userName && x.Password == password).ID;
                }
                catch (Exception ex)
                {
                    res.Status = false;
                    res.Message = ex.Message;
                }
            }

            return res;
        }
    }
}
