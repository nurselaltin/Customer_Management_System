using Core.CommonModel;
using Core.CommonModel.Result;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MTS_API.Helper
{
  public class TicketModel
  {
    public int UserId { get; set; }
    public string UserName { get; set; }

  }
  public class TokenHelper
  {
    public static ServiceResult<TicketModel> TokenCoz(string token)
    {
      var res = new ServiceResult<TicketModel>();
     
      try
      {
        var jsonToken = new JwtSecurityTokenHandler().ReadJwtToken(token); // Tokenı doğru kontrol etmiyor
        res.Data = new TicketModel();
        res.Data.UserId = Convert.ToInt32(new ClaimsIdentity(jsonToken.Claims).Claims.FirstOrDefault(o => o.Type == ClaimTypes.Sid).Value);
        res.Data.UserName = Convert.ToString(new ClaimsIdentity(jsonToken.Claims).Claims.FirstOrDefault(o => o.Type == ClaimTypes.Name).Value);
       
      }
      catch (Exception ex)
      {
        res.Message = "Yetkiniz yok!";
        res.StatusCode = ResultCode.Unauthorized;
        res.Status = false;
        
      }
   
      return res;
    }
  }
}

