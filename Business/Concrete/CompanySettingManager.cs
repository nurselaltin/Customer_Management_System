using Business.Abstract;
using Core.CommonModel;
using Core.CommonModel.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.ViewModel;

namespace Business.Concrete
{
  public class CompanySettingManager : ICompanySettingService
  {
    private ICompanySettingDal _companySettingDal;
    private ILogService _logService;
    public CompanySettingManager(ICompanySettingDal companySettingDal, ILogService logService)
    {
      _companySettingDal = companySettingDal;
      _logService = logService;
    }

    public ServiceResult Add(CompanySettingViewModel vm, int createdUserId)
    {
      var companySetting = new CompanySetting();
      companySetting.SettingKey = vm.SettingKey;
      companySetting.SettingVal = vm.SettingVal;
      companySetting.CreatedDate = DateTime.Now;
      companySetting.CreatedUserId = createdUserId;

      var res = _companySettingDal.Add(companySetting);

      if (res.Status == false)
      {
        res.Message = "Ekleme sırasında hata oluştu!";

        //Log
        _logService.Add("ERROR", "CompanySettingManager.Add", res.Message, createdUserId);

      }
      else
      {
        res.Message = "Başarıyla kayıt edildi.";
        res.StatusCode = ResultCode.Created;

        //Log
        _logService.Add("INFO", "CompanySettingManager.Add", res.Message, createdUserId);
      }
      return res;
    }
    public ServiceResult<CompanySettingViewModel> Update(CompanySettingViewModel vm, int updatedUserId)
    {
      ServiceResult<CompanySettingViewModel> res = new ServiceResult<CompanySettingViewModel>();

      #region Validation

      var valid = new CompanySettingViewModelValidator().Validate(vm);
      if (!valid.IsValid)
      {
        var fieldError = valid.Errors.First();
        res.Status = false;
        res.StatusCode = ResultCode.BadRequest;
        res.Message = String.Format("'{0}' {1}", fieldError.PropertyName, fieldError.ErrorMessage);
        return res;
      }

      //Is setting key exist?
      var sett = _companySettingDal.GetSettingVal(vm.SettingKey).Data;
      if (sett == null)
      {
        //Add
        CompanySetting model = new CompanySetting();
        model.SettingVal = vm.SettingVal;
        model.SettingKey = vm.SettingKey;
        model.CreatedDate = DateTime.Now;
        model.CreatedUserId = updatedUserId;

        var ress = _companySettingDal.Add(model);
        if (ress.Status == false)
        {
          ress.Message = "Güncellenme sırasında hata oluştu!";
          ress.StatusCode = ResultCode.GeneralServiceError;
          //Log
          _logService.Add("ERROR", "CompanySettingManager.Update", res.Message, updatedUserId);
        }
        else
        {
          res.Message = "Başarılı bir şekilde güncellendi.";
          res.StatusCode = ResultCode.SuccesWithNoContent;

          //Log
          _logService.Add("INFO", "CompanySettingManager.Update", "Başarıyla güncellendi!", updatedUserId);
        }

        return res;
      }

      #endregion

      //Update
      sett.UpdatedDate = DateTime.Now;
      sett.UpdatedUserId = updatedUserId;
      sett.SettingVal = vm.SettingVal;
      var result = _companySettingDal.Update(sett);
      if (result.Status == false)
      {
        result.Message = "Güncellenme sırasında hata oluştu!";
        result.StatusCode = ResultCode.GeneralServiceError;

        //Log
        _logService.Add("ERROR", "CompanySettingManager.Update", "Güncellenme sırasında hata oluştu" + res.Message, updatedUserId);

      }
      else
      {
        res.Data = new CompanySettingViewModel();
        res.Data.SettingKey = result.Data.SettingKey;
        res.Data.SettingVal = result.Data.SettingVal;
        res.Status = result.Status;
        res.Message = "Güncelleme başarılı.";
        res.StatusCode = ResultCode.Success;

        //Log
        _logService.Add("INFO", "CompanySettingManager.Update", "Başarıyla güncellendi!", updatedUserId);
      }

      return res;
    }
    public ServiceResult Delete(string  settingKey)
    {
      var res = new ServiceResult();

      //Is setting exist?
      var sett = _companySettingDal.Find(x => x.SettingKey == settingKey);
      if (sett == null)
      {
        res.Status = false;
        res.StatusCode = ResultCode.NotFound;
        res.Message = String.Format("İlgili kayıt bulunamadı!");

        //Log
        _logService.Add("ERROR", "CompanySettingManager.Delete", "İlgili kayıt bulunamadı!" + res.Message, 0);
        return res;
      }

      var result = _companySettingDal.Delete(sett);
      if (!result.Status)
      {
        res.Message = "Silme sırasında hata oluştu!";
        res.StatusCode = ResultCode.GeneralServiceError;

        //Log
        _logService.Add("ERROR", "CompanySettingManager.Delete", res.Message + result.Message, 0);

      }
      else
      {
        res.Message = "Başarıyla silindi.";
        res.StatusCode = ResultCode.SuccesWithNoContent;

        //Log
        _logService.Add("INFO", "CompanySettingManager.Delete", res.Message, 0);
      }
      return res;
    }
    public ServiceResult<CompanySetting> GetSettingVal(string SettinKey)
    {
      return _companySettingDal.GetSettingVal(SettinKey);
    }
  }
}
