
using Core.CommonModel.Result;
using Entities.Concrete;
using Entities.ViewModel;

namespace Business.Abstract
{
    public interface IUserService
    {
        ServiceResult Add(RegisterViewModel vm,int createdUserId);
        ServiceResult<UserViewModel> Update(UserViewModel vm, int updatedUserId);
        ServiceResult UpdatePassword(string password, int userId, int updatedUserId);
        ServiceResult<int> GetUserID(string userName, string password);
        ServiceResult<UserViewModel> GetSingle(LoginRequestViewModel vm);
        ServiceResult Delete(int id);
        ServiceResult TicketAdd(UserTicket userTicket);
    }
}
