using Core.CommonModel;
using Core.CommonModel.Result;
using DataAccess.Concrete;
using Entities.Concrete;
using MailKit.Net.Smtp;
using MailKit.Security;
using MailSendler.Abstract;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json;

namespace MailSendler.Concrete
{
  public class MailSendlerService : IMailSendlerService
  {

    //SEND MAİL
    public ServiceResult<bool> SendMail(string subject, string receiver, string content = null, string sender = null, List<string> attachments = null, Dictionary<string, byte[]> files = null, CompanySetting setting = null)
    {
      ServiceResult<bool> res = new ServiceResult<bool>();
      var sett  = JsonConvert.DeserializeObject<MailSettings>(setting.SettingVal);
      try
      {

        // create email message
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("nurselaltin.na@gmail.com"));
        email.To.Add(MailboxAddress.Parse(receiver));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Plain) { Text = content };
        
        // send email
        using var smtp = new SmtpClient();
        smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        smtp.Authenticate(sett.UserName, sett.AppPassword);
        smtp.Send(email);
        smtp.Disconnect(true);


        //// CREATE REQUEST
        //RestClient client = new RestClient();
        ////client.BaseUrl = new Uri(settings.root_url);
        //client.Authenticator = new HttpBasicAuthenticator("api", settings.api_key);

        //RestRequest request = new RestRequest();
        //request.AddParameter("domain", settings.domain, ParameterType.UrlSegment);
        //request.Resource = "{domain}/messages";

        //// GENERAL PARAMETERS
        //request.AddParameter("subject", subject);
        //request.AddParameter("o:tracking", false);
        //request.AddParameter("to", receiver);
        //request.AddParameter("html", content);

        //// ADD SENDER INFO
        //if (string.IsNullOrEmpty(sender))
        //  request.AddParameter("from", settings.default_sender);
        //else
        //  request.AddParameter("from", sender);

        //// ATTACHMENT & FILES
        //if (attachments != null)
        //  foreach (var file in attachments)
        //    request.AddFile("attachment", file);

        //if (files != null)
        //  foreach (var file in files)
        //    request.AddFile("attachment", file.Value, file.Key);

        //// SEND REQUEST
        //if (settings.send_mail)
        //{
        //  var result = client.Post(request);

        //  if (result.StatusCode != HttpStatusCode.OK)
        //    throw new Exception($"Mail Server Error! \nStatus: {result.StatusCode} \nRequest Message: {result.Content}");
        //}
      }
      catch (Exception exp)
      {
        res.Status = false;
        res.StatusCode = ResultCode.GeneralServiceError;
        res.Message = exp.Message;

        return res;
      }

      res.Status = true;
      res.StatusCode = ResultCode.Success;
      return res;
    }
  }
  public class MailSettings
  {
    public string Sender { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string AppPassword { get; set; }
  }
}
