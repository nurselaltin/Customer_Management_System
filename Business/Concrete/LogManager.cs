using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
  public class LogManager : ILogService
  {
    private ILogDal _logDal;
    public LogManager(ILogDal logDal)
    {
      _logDal = logDal;
    }
    public void Add(string LogType, string Part, string Message, int UserId)
    {
      Log log = new Log();
      log.LogType = LogType;
      log.Part = Part;
      log.User = UserId.ToString();
      log.IpAddress = "";
      log.Message = Message;
      log.SystemDate = DateTime.Now;

      _logDal.Add(log);
    }
  }
}
