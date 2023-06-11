using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;

namespace DataAccess.Concrete
{
  public class LogDal : ILogDal
  {
    public void Add(Log log)
    {
      using (var context = new MTSContext())
      {
        try
        {
          context.Add(log);
          context.SaveChanges();
        }
        catch
        {

        }
      }
    }
  }
}
