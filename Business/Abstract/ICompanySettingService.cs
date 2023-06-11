using Core.CommonModel.Result;
using Entities.Concrete;
using Entities.ViewModel;

namespace Business.Abstract
{
  public interface ICompanySettingService
  {
    public ServiceResult<CompanySetting> GetSettingVal(string SettinKey);
    ServiceResult Add(CompanySettingViewModel vm, int createdUserId);
    public ServiceResult Delete(string settingKey);
    ServiceResult<CompanySettingViewModel> Update(CompanySettingViewModel vm, int updatedUserId);
  }
}
