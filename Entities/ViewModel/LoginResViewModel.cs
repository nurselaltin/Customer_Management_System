using FluentValidation;
using System;
namespace Entities.ViewModel
{
	public class LoginResViewModel
	{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string RefreshTokenExpireDate { get; set; }
    public string TokenExpireDate { get; set; }
  }
}

