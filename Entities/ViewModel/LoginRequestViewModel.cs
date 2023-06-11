using FluentValidation;
using System;
namespace Entities.ViewModel
{
	public class LoginRequestViewModel
	{
		public string UserName { get; set; }
		public string Password { get; set; }

		public class LoginRequestViewModelValidator : AbstractValidator<LoginRequestViewModel>
		{
			public LoginRequestViewModelValidator()
			{
				RuleFor(x => x.UserName).NotEmpty().NotNull().Length(3, 250);
				RuleFor(x => x.Password).NotNull().NotNull().Length(0, 150);
			}
		}
	}
}

