using Business.Abstract;
using Business.Concrete;
using Core.CommonModel.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.ViewModel;
using Moq;
using System.Linq.Expressions;

namespace MTS.UnitTests
{
  public class UserManagerTests
  {
    /// <summary>
    /// Loose ; Daha esnek.Strict mockladığımız tüm metotları setuplamak gerekecek.
    /// </summary>
    private Mock<IUserDal> _mockDal { get; set; }
    private Mock<IUserTicketDal> _mockTicketDal { get; set; }
    private Mock<ILogService> _logService { get; set; }
    public UserManagerTests()
    {
      _mockDal = new Mock<IUserDal>();
      _mockTicketDal = new Mock<IUserTicketDal>();
      _logService = new Mock<ILogService>();
    }

    #region Add
    [Test]
    public void Add_GivenNullFullName_ValidationErrorCode()
    {
      //Arrange
      var userManager = new UserManager(_mockDal.Object, _mockTicketDal.Object, _logService.Object);

      var vm = new RegisterViewModel()
      {
        FullName = null,
        UserName = "nursel"
      };

      //Action
      var res = userManager.Add(vm, 2);

      //Assert
      Assert.AreEqual(res.StatusCode, ResultCode.ValidationError);
    }

    [Test]
    public void Add_GivenNullUserName_ValidationErrorCode()
    {
      //Arrange
      var userManager = new UserManager(_mockDal.Object, _mockTicketDal.Object, _logService.Object);

      var vm = new RegisterViewModel()
      {
        FullName = "Nursel Altın",
        UserName = null
      };

      //Action
      var res = userManager.Add(vm, 2);

      //Assert
      Assert.AreEqual(res.StatusCode, ResultCode.ValidationError);
    }

    //[Test]
    //public void Add_GivenNullPassword_ValidationErrorCode()
    //{
    //  //Arrange
    //  var userManager = new UserManager(_mockDal.Object, _mockTicketDal.Object, _logService.Object);

    //  var vm = new RegisterViewModel()
    //  {
    //    FullName = "Nursel Altın",
    //    //Password = null,
    //    UserName = "nursel@gmail.com"
    //  };

    //  //Action
    //  var res = userManager.Add(vm, 2);

    //  //Assert
    //  Assert.AreEqual(res.StatusCode, ResultCode.ValidationError);
    //}

    [Test]
    public void Add_UserNameInValidEmailFormat_EmailWithErrorMessage()
    {
      //Arrange
      var vmValidator = new Mock<RegisterViewModelValidator>();
      var userManager = new UserManager(_mockDal.Object, _mockTicketDal.Object, _logService.Object);

      //Kimi gönderirse göndersin null değeri dönsün.Çünkü ben burayı test etmiyorum şu an için.Bu adımda takılmak istemiyorum.
      _mockDal.Setup(i => i.GetSingle(It.IsAny<Expression<Func<User, bool>>>())).Returns(new ServiceResult<User>());

      var vm = new RegisterViewModel()
      {
        FullName = "Nursel Altın",
        UserName = "nursel"
      };


      //Action
      var res = userManager.Add(vm, 2);
      //Assert

      Assert.AreEqual(res.Message, string.Format("You shouldn't use turkish character in the username like 'ç,ö,ş,ü' and it shouldn't contain empty expression.You should define lowercase."));
    }

    [Test]
    public void Add_GivenExistUser_UserExistWitGeneralServiceError()
    {
      //Arrange
      var userManager = new UserManager(_mockDal.Object, _mockTicketDal.Object, _logService.Object);
      _mockDal.Setup(i => i.GetSingle(It.IsAny<Expression<Func<User, bool>>>())).Returns(new ServiceResult<User>() { Data = new User()});

      var vm = new RegisterViewModel()
      {
        FullName = "Nursel ALTIN",
        UserName = "nursel"
      };

      //Action
      var res = userManager.Add(vm, 2);

      //Assert
      Assert.AreEqual(res.StatusCode, ResultCode.GeneralServiceError);
    }

    #endregion

    #region GetSingle
    [Test]
    public void GetSingle_GivenNullPassword_ValidationErrorCode()
    {
      //Arrange
      var userManager = new UserManager(_mockDal.Object, _mockTicketDal.Object, _logService.Object);


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
    public void GetSingle_GivenNullUserName_ValidationErrorCode()
    {
      //Arrange
      var userManager = new UserManager(_mockDal.Object, _mockTicketDal.Object, _logService.Object);
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

    #endregion

    #region UpdatePassword
    [Test]
    public void UpdatePassword_GivenNullPassword_ValidationErrorCode()
    {
      //Arrange
      var userManager = new UserManager(_mockDal.Object, _mockTicketDal.Object, _logService.Object);

      string pass = "";
      int userId = 1;
      int updatedUserId = 2;

      //Action
      var res = userManager.UpdatePassword(pass, userId, updatedUserId);

      //Assert
      Assert.AreEqual(res.StatusCode, ResultCode.ValidationError);
    }

    [Test]
    public void UpdatePassword_GivenNullPassword_PasswordWithErrorMessage()
    {
      //Arrange
      var userManager = new UserManager(_mockDal.Object, _mockTicketDal.Object, _logService.Object);

      string pass = "";
      int userId = 1;
      int updatedUserId = 2;

      //Action
      var res = userManager.UpdatePassword(pass, userId, updatedUserId);

      //Assert
      Assert.AreEqual(res.Message, string.Format("Password is not null!"));
    }

    [Test]
    public void UpdatePassword_GivenNotExistUser_ErrorMessage()
    {
      //Arrange
      var userManager = new UserManager(_mockDal.Object, _mockTicketDal.Object, _logService.Object);
      _mockDal.Setup(i => i.GetSingle(It.IsAny<Expression<Func<User, bool>>>())).Returns(new ServiceResult<User>() {  });

      string pass = "123456";
      int userId = 1;
      int updatedUserId = 2;

      //Action
      var res = userManager.UpdatePassword(pass, userId, updatedUserId);

      //Assert
      Assert.AreEqual(res.Message, String.Format("User is not exist!"));
    }

    [Test]
    public void UpdatePassword_NotUpdateAdmin_ErrorMessage()
    {
      //Arrange
      var userManager = new UserManager(_mockDal.Object, _mockTicketDal.Object, _logService.Object);
      _mockDal.Setup(i => i.GetSingle(It.IsAny<Expression<Func<User, bool>>>())).Returns(new ServiceResult<User>() { Data = new User() { UserType = 1} });

      string pass = "123456";
      int userId = 1;
      int updatedUserId = 2;

      //Action
      var res = userManager.UpdatePassword(pass, userId, updatedUserId);

      //Assert
      Assert.AreEqual(res.Message, String.Format("You can not update admin user!"));
    }

    #endregion
  }
}