using Core.CommonModel.Result;
using DataAccess.Concrete;
using Entities.Concrete;

namespace MailSendler.Abstract
{
  public interface IMailSendlerService
  {
    ServiceResult<bool> SendMail(string subject, string receiver, string content = null, string sender = null, List<string> attachments = null, Dictionary<string, byte[]> files = null, CompanySetting setting = null);
  }
}
