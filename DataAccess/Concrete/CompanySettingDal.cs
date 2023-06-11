using Core.CommonModel.Result;
using Core.ORM.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;
using Newtonsoft.Json;

namespace DataAccess.Concrete
{
	public class CompanySettingDal : EfRepositoryBase<CompanySetting, MTSContext>, ICompanySettingDal
  {
   
    public ServiceResult<CompanySetting> GetSettingVal(string SettinKey)
    {
      ServiceResult<CompanySetting> res = new ServiceResult<CompanySetting>();
      using (var context = new MTSContext())
      {
        try
        {
          var data = context.Set<CompanySetting>().FirstOrDefault(x => x.SettingKey == SettinKey);
          res.Data = data;
          res.Status = true;
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

