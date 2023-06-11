using Entities.Concrete;

namespace Business.Abstract
{
  public interface ILogService
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="LogType">Hata veya bildirim mi : ERORR, INFO</param>
    /// <param name="Part">Katman ismi ve metot : MailSendler.SendMail</param>
    /// <param name="Message">Mesaj</param>
    /// <param name="UserId"></param>
    void Add(string LogType, string Part, string Message,  int UserId);
  }
}
