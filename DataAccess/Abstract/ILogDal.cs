using Core.CommonModel.Result;
using Entities.Concrete;

namespace DataAccess.Abstract
{
  public interface ILogDal 
  {
    void Add(Log log);
  }
}
