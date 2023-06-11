using FluentValidation;

namespace Entities.ViewModel
{
	public class CompanySettingViewModel
  {
		public string SettingKey { get; set; }
		public string SettingVal { get; set; }
    public string AccessToken { get; set; }
  }
  public class CompanySettingViewModelValidator : AbstractValidator<CompanySettingViewModel>
  {
    public CompanySettingViewModelValidator()
    {
      RuleFor(x => x.SettingKey).NotEmpty().NotNull();
      RuleFor(x => x.SettingVal).NotNull().NotNull();
    }
  }
}

