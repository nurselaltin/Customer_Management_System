using System;
using Core.CommonModel.Result;
using Core.ORM;
using DataAccess.Concrete;
using Entities.Concrete;

namespace DataAccess.Abstract
{
  public interface ICompanySettingDal : IRepositoryBase<CompanySetting>
  {
    public ServiceResult<CompanySetting> GetSettingVal(string SettinKey);
  }
}

