using System.Text.RegularExpressions;
using Business.Abstract;
using Core.CommonModel.Result;
using Core.Helper;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.ViewModel;
using MailSendler.Abstract;
using static Entities.ViewModel.LoginRequestViewModel;
using Core.CommonModel;
namespace Business.Concrete
{
  public class UserManager : IUserService
  {
    private IUserDal _userDal;
    private ICompanySettingDal _companySettingDal;
    private ILogService _logService;
    private IMailSendlerService _mailSendlerService;
    private IUserTicketDal _userTicketDal;
    public UserManager(IUserDal userDal, IUserTicketDal userTicketDal, ILogService logService, ICompanySettingDal companySettingDal, IMailSendlerService mailSendlerService)
    {      
      _userDal = userDal;
      _userTicketDal = userTicketDal;
      _logService = logService;
      _companySettingDal = companySettingDal;
      _mailSendlerService = mailSendlerService;
    }
    public ServiceResult Add(RegisterViewModel vm, int createdUserId)
    {

      ServiceResult res = new ServiceResult();

      #region Validation 

      var valid = new RegisterViewModelValidator().Validate(vm);
      if (!valid.IsValid)
      {
        var fieldError = valid.Errors.First();
        res.Status = false;
        res.StatusCode = ResultCode.BadRequest;
        res.Message = String.Format("'{0}' {1}", fieldError.PropertyName, fieldError.ErrorMessage);
        return res;
      }

      string userNameForCheck = vm.UserName.ToLower();
      var rule = "^.+@.+\\.[a-zA-Z]{2,3}$";
      if (!Regex.IsMatch(userNameForCheck, rule))
      {
        res.Status = false;
        res.StatusCode = ResultCode.GeneralServiceError;
        res.Message = string.Format("Kullanıcı adında 'ç,ö,ş,ü' gibi  türkçe karakter kullanılmamalı ve boş ifadeler içermemelidir. Küçük harf tanımlamalısınız.");
        return res;
      }

      #endregion

      //Is user exist?
      var user = _userDal.GetSingle(x => x.UserName == vm.UserName).Data;
      if (user != null)
      {
        res.Status = false;
        res.StatusCode = ResultCode.GeneralServiceError;
        res.Message = String.Format("{0} zaten mevcut!", vm.UserName);
        return res;
      }

      //Do random password
      var pass = Helper.CreateGenerateKey(8);

      //Do hash password
      var hashPassword = Helper.ComputeHash(pass);
      if (String.IsNullOrEmpty(hashPassword))
      {
        res.Status = false;
        res.StatusCode = ResultCode.GeneralServiceError;
        res.Message = string.Format("Şifre kontrolünde hata oluştu!");
        return res;
      }

      #region Mapping
      //Mapping
      User userData = new User()
      {
        FullName = vm.FullName,
        UserName = vm.UserName,
        Title = vm.Title,
        Password = hashPassword,
        IsActive = true,
        PhotoUrl = vm.PhotoUrl,
        Phone = vm.Phone,
        UserBeginDate = vm.UserBeginDate,
        UserEndDate = vm.UserEndDate,
        UserTC = vm.UserTC,
        UserAddress = vm.UserAddress,
        UserType = vm.UserType,
        CreatedDate = DateTime.Now,
        CreatedUserId = createdUserId,
      };

      #endregion

      res = _userDal.Add(userData);
      if (res.Status == false)
      {
        res.Message = "Ekleme sırasında hata oluştu!";

        //Log
        _logService.Add("ERROR", "UserManager.Add", res.Message, createdUserId);

      }
      else
      {
        res.Success("Başarıyla kayıt edildi.");
        res.StatusCode = ResultCode.Created;

        //Log
        _logService.Add("INFO", "UserManager.Add", "Başarıyla kayıt edildi!", createdUserId);
      }

      //Send Mail
      var setting = _companySettingDal.GetSettingVal("Mail.Settings");
      var mail = _mailSendlerService.SendMail("Şifre Güncellenmesi Hk.", "na.nurselaltin@gmail.com", "Mail content", "", null, null, setting.Data);
      if (!mail.Status)
        _logService.Add("ERROR", "MailSendlerService.SendMail", "Mail gönderirken hata oluştu! : " + mail.Message, createdUserId);

      return res;
    }
    public ServiceResult Delete(int id)
    {
      var res = new ServiceResult();

      //Is user exist?
      var user = _userDal.GetSingle(x => x.ID == id).Data;
      if (user == null)
      {
        res.Status = false;
        res.Message = String.Format("Kullanıcı bulunamadı.");
        res.StatusCode = ResultCode.NotFound;

        //Log
        _logService.Add("ERROR", "UserManager.Delete", res.Message, 0);

        return res;
      }

      //Is user Admin?
      if (user.UserType == 1) // 1: Admin
      {
        res.Status = false;
        res.StatusCode = ResultCode.Forbidden;
        res.Message = String.Format("Admin kullanıcısını silemezsiniz!");


        //Log
        _logService.Add("ERROR", "UserManager.Delete", res.Message, 0);

        return res;
      }

      var result  = _userDal.Delete(user);
      if (!result.Status)
      {
        res.Message = "Silme sırasında hata oluştu!";
        res.Status = false;
        res.StatusCode = ResultCode.GeneralServiceError;
        //Log
        _logService.Add("ERROR", "UserManager.Delete", res.Message + result.Message, 0);

      }
      else
      {
        res.Message = "Başarıyla silindi!";
        res.StatusCode = ResultCode.SuccesWithNoContent;

        //Log
        _logService.Add("INFO", "UserManager.Delete", res.Message, 0);
      }

      return res;
    }
    public ServiceResult<UserViewModel> Update(UserViewModel vm, int updatedUserId)
    {
      ServiceResult<UserViewModel> res = new ServiceResult<UserViewModel>();

      #region Validation & Mapping

      var valid = new UserViewModelValidator().Validate(vm);
      if (!valid.IsValid)
      {
        var fieldError = valid.Errors.First();
        res.Status = false;
        res.StatusCode = ResultCode.BadRequest;
        res.Message = String.Format("'{0}' {1}", fieldError.PropertyName, fieldError.ErrorMessage);
        return res;
      }

      //Is user exist?
      var user = _userDal.GetSingle(x => x.ID == vm.ID).Data;
      if (user == null)
      {
        res.Status = false;
        res.Message = String.Format("Kullanıcı bulunamadı!");
        res.StatusCode = ResultCode.NotFound;
        return res;
      }

      //Is user Admin?
      if (user.UserType == 1) // 1: Admin
      {
        res.Status = false;
        res.StatusCode = ResultCode.Forbidden;
        res.Message = String.Format("Admin kullanıcısını güncelleyemezsiniz!");

        //Log
        _logService.Add("ERROR", "UserManager.Update", res.Message, 0);

        return res;
      }

      //Check Username(Email)
      //TO DO: e mail regex eklenecek.
      string userNameForCheck = vm.UserName.ToLower();
      var rule = "^.+@.+\\.[a-zA-Z]{2,3}$";
      if (!Regex.IsMatch(userNameForCheck, rule))
      {
        res.Status = false;
        res.Message = string.Format("Kullanıcı adında 'ç,ö,ş,ü' gibi  türkçe karakter kullanılmamalı ve boş ifadeler içermemelidir. Küçük harf tanımlamalısınız.");
        return res;
      }

      //Do hash password
      var hashPassword = Helper.ComputeHash(vm.Password);

      //Mapping
      User userData = new User()
      {
        FullName = vm.FullName,
        UserName = vm.UserName,
        Title = vm.Title,
        Password = hashPassword,
        IsActive = true,
        PhotoUrl = vm.PhotoUrl,
        Phone = vm.Phone,
        UserBeginDate = vm.UserBeginDate,
        UserEndDate = vm.UserEndDate,
        UserTC = vm.UserTC,
        UserAddress = vm.UserAddress,
        UserType = vm.UserType,
        UpdatedDate = DateTime.Now,
        UpdatedUserId = updatedUserId,
      };

      #endregion

      var result = _userDal.Update(userData);

      if (result.Status == false)
      {
        result.Message = "Güncellenme sırasında hata oluştu!";
        res.StatusCode = ResultCode.GeneralServiceError;
        //Log
        _logService.Add("ERROR", "CompanySettingManager.Update", "Güncellenme sırasında hata oluştu" + result.Message, updatedUserId);

      }
      else
      {
        res.Data = Map(result.Data);
        res.Status = result.Status;
        res.Message = "Güncelleme başarılı.";
        res.StatusCode = ResultCode.Success;

        //Log
        _logService.Add("INFO", "CompanySettingManager.Update", "Başarıyla güncellendi!", updatedUserId);
      }

      return res;
    }
    public ServiceResult UpdatePassword(string password, int userId, int updatedUserId)
    {
      ServiceResult res = new ServiceResult();

      #region Validation & Mapping

      if (String.IsNullOrEmpty(password))
      {
        res.Status = false;
        res.StatusCode = ResultCode.BadRequest;
        res.Message = String.Format("Şifre boş bırakılamaz!");
        return res;
      }

      //Is user exist?
      var user = _userDal.GetSingle(x => x.ID == userId).Data;
      if (user == null)
      {
        res.Status = false;
        res.StatusCode = ResultCode.NotFound;
        res.Message = String.Format("Kullanıcı bulunamadı!");
        return res;
      }

      //Is user Admin?
      if (user.UserType == 1) // 1: Admin
      {
        res.Status = false;
        res.StatusCode = ResultCode.Forbidden;
        res.Message = String.Format("Admin kullanıcısını güncelleyemezsiniz!");

        //Log
        _logService.Add("ERROR", "UserManager.UpdatePassword", res.Message, 0);

        return res;
      }

      //Do hash password
      var hashPassword = Helper.ComputeHash(password);

      //Mapping
      user.Password = hashPassword;
      user.UpdatedDate = DateTime.Now;
      user.UpdatedUserId = updatedUserId;

      #endregion

      var result = _userDal.Update(user);
      if (result == null)
      {
        res.Status = false;
        res.StatusCode = ResultCode.GeneralServiceError;
        res.Message = "Güncellenme sırasında hata oluştu!";

        //Log
        _logService.Add("ERROR", "UserManager.UpdatePassword", "Güncellenme sırasında hata oluştu!" + result.Message, updatedUserId);
      }
      else
      {
        res.Message = "Şifre başarılı bir şekilde güncellendi.";
        res.StatusCode = ResultCode.Success;
        //Log
        _logService.Add("INFO", "CompanySettingManager.Update", "Başarıyla güncellendi!", updatedUserId);
      }

      return res;
    }
    public ServiceResult<UserViewModel> GetSingle(LoginRequestViewModel vm)
    {
      ServiceResult<UserViewModel> res = new ServiceResult<UserViewModel>();

      #region Validation 
      var valid = new LoginRequestViewModelValidator().Validate(vm);
      if (!valid.IsValid)
      {
        var fieldError = valid.Errors.First();
        res.Status = false;
        res.StatusCode = ResultCode.BadRequest;
        res.Message = String.Format("'{0}' {1}", fieldError.PropertyName, fieldError.ErrorMessage);
        return res;
      }
      #endregion

      var hashedPass = Helper.ComputeHash(vm.Password);
      var user = _userDal.GetSingle(x => x.UserName == vm.UserName && x.Password == hashedPass && x.IsActive == true);
      if (user.Data == null)
      {
        res.Status = false;
        res.StatusCode = ResultCode.NotFound;
        res.Message = String.Format("Kullanıcı bulunmadı!");
        return res;
      }

      res.Data = Map(user.Data);
      return res;
    }
    public ServiceResult<int> GetUserID(string userName, string password)
    {
      return _userDal.GetUserID(userName, password);
    }
    public ServiceResult TicketAdd(UserTicket userTicket)
    {
      return _userTicketDal.Add(userTicket);
    }
    public UserViewModel Map(User user)
    {
      UserViewModel vm = new UserViewModel();

            vm.ID = user.ID;
            vm.FullName = user.FullName;
            vm.UserName = user.UserName;
            vm.Password = user.Password;
            vm.Role = ((UserTypeEnum)(user.Role)).ToString();
            return vm;
        }
    }
}