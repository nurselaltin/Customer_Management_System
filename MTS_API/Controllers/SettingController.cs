using Business.Abstract;
using Core.CommonModel.Result;
using Entities.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace MTS_API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SettingController : ControllerBase
  {
    private ICompanySettingService _companySettingService;

    public SettingController(ICompanySettingService companySettingService)
    {
      _companySettingService = companySettingService;
    }

    [HttpPost]
    [Route("AddSetting")]
    public ServiceResult AddSetting(CompanySettingViewModel vm)
    {
      ServiceResult res = new ServiceResult();
  
      res = _companySettingService.Add(vm, 1);
      return res;
    }

    [HttpPost]
    [Route("UpdateSetting")]
    public ServiceResult<CompanySettingViewModel> UpdateSetting(CompanySettingViewModel vm)
    {
      ServiceResult<CompanySettingViewModel> res = new ServiceResult<CompanySettingViewModel>();

      res = _companySettingService.Update(vm, 1);
      return res;
    }

    [HttpDelete]
    [Route("DeleteSetting")]
    public ServiceResult DeleteSetting(string settingKey, string accessToken)
    {
      ServiceResult res = new ServiceResult();

      //var tokenRes = TokenHelper.TokenCoz(accessToken);

      //if (tokenRes.StatusCode == ResultCode.Unauthorized)
      //{
      //  res.StatusCode = tokenRes.StatusCode;
      //  res.Message = tokenRes.Message;
      //  return res;
      //}
      res = _companySettingService.Delete(settingKey);
      return res;
    }
  }
}
