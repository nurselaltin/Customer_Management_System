using Business.Abstract;
using Core.CommonModel.Result;
using Core.Infrastructure;
using Entities.Concrete;
using Entities.ViewModel;
using MailSendler.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MailSendler.Abstract;

namespace TestologAPI.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private IUserService _userService;
        private ILogService _logService;
        private IJwtAuthManager _jwtAuthManager;
        private readonly IConfiguration _configuration;
        private IMailSendlerService _mailSendlerService;
        public LoginController(IConfiguration configuration,IUserService userService, IJwtAuthManager jwtAuthManager, ILogService logService, IMailSendlerService mailSendlerService)
        {
            _configuration = configuration;
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
            _logService = logService;
            _mailSendlerService = mailSendlerService;
        }

    [HttpPost]
    [Route("Login")]
    public ServiceResult<LoginResViewModel> Login([FromBody] LoginRequestViewModel vm)
    {
      ServiceResult<LoginResViewModel> res = new ServiceResult<LoginResViewModel>();

      //1.Adım
      ServiceResult<UserViewModel> user = _userService.GetSingle(vm);
      if (!user.Status)
      {
        res.Message = user.Message;
        res.Status = user.Status;
        res.StatusCode = user.StatusCode;

        //Log
        _logService.Add("ERROR", "LoginController.Login", "Giriş sağlanamadı : Oturum sağlanamadı! Lütfen daha sonra tekrar deneyiniz!", 0); //Giriş yapmaya çalışan kullanıcı

        return res;
      }

            //2.Adım
            var claims = new[]
            {
              new Claim(ClaimTypes.Name,vm.UserName),
              new Claim(ClaimTypes.Sid,user.Data.ID.ToString()),
              new Claim(ClaimTypes.Uri,vm.Password),
              new Claim(ClaimTypes.Role,user.Data.Role)
            };
            DateTime currentTime = DateTime.Now;
            DateTime expireTime = currentTime.AddDays(1);
            //var jwtResult = _jwtAuthManager.GenerateTokens(vm.UserName, claims, DateTime.Now);  //??



            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
    _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwtResult = new JwtSecurityTokenHandler().WriteToken(token);


            UserTicket userTicketModel = new()
            {
                UserId = user.Data.ID,
                Token = jwtResult,
                Ticket = Guid.NewGuid().ToString(),
                CreatedDate = currentTime,
                ExpireDate = expireTime,
            };
            _userService.TicketAdd(userTicketModel);

            res.Data = new LoginResViewModel
            {
                AccessToken = jwtResult,
                //RefreshToken = jwtResult.RefreshToken.TokenString,
                //RefreshTokenExpireDate = jwtResult.RefreshToken.ExpireAt.ToString(),
                TokenExpireDate = expireTime.ToString()
            };

      return res;
    }
  }
}
