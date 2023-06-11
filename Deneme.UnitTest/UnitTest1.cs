using Business.Concrete;
using Core.CommonModel.Result;
using DataAccess.Concrete;
using Entities.ViewModel;

namespace Deneme.UnitTest
{
  public class Tests
  {

    // UnitOfWork_Condition_ExpectedResult
    // Condition

    [Test]
    public void UserManager_Password_ValidationError()
    {
      //Arrange
      var dal = new UserDal();
      var ticket = new UserTicketDal();
      var userManager = new UserManager(dal, ticket);
      var vm = new LoginRequestViewModel()
      {
         Password = null,
         UserName = "nurselaltin.na@gmail.com"
      };

      //Action

      var res = userManager.GetSingle(vm);
      //Assert

      Assert.AreEqual(res.StatusCode, ResultCode.ValidationError);
    }

    [Test]
    public void UserManager_UserName_ValidationError()
    {
      //Arrange
      var dal = new UserDal();
      var ticket = new UserTicketDal();
      var userManager = new UserManager(dal, ticket);
      var vm = new LoginRequestViewModel()
      {
        Password = "1234567",
        UserName = null
      };

      //Action

      var res = userManager.GetSingle(vm);
      //Assert

      Assert.AreEqual(res.StatusCode, ResultCode.ValidationError);
    }

    [Test]
    public void UserManager_IsExistUser_GeneralServiceError()
    {
      //Arrange
      var dal = new UserDal();
      var ticket = new UserTicketDal();
      var userManager = new UserManager(dal, ticket);
      var vm = new LoginRequestViewModel()
      {
        Password = "1234567",
        UserName = "nursel@gmail.com.com"
      };

      //Action

      var res = userManager.GetSingle(vm);
      //Assert

      Assert.AreEqual(res.StatusCode, ResultCode.GeneralServiceError);
    }

    [Test]
    public void UserManager_IsExistUser_Success()
    {
      //Arrange
      var dal = new UserDal();
      var ticket = new UserTicketDal();
      var userManager = new UserManager(dal, ticket);
      var vm = new LoginRequestViewModel()
      {
        Password = "1234567",
        UserName = "nursel@gmail.com.com"
      };

      //Action

      var res = userManager.GetSingle(vm);
      //Assert

      Assert.AreEqual(res.StatusCode, ResultCode.Success);
    }
  }
}