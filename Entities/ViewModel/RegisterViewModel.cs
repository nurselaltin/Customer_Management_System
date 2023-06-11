using FluentValidation;

namespace Entities.ViewModel
{
  public class RegisterViewModel
  {
    public string FullName { get; set; }
    public string UserName { get; set; }
    public string Title { get; set; } //Doktor, sekreter, asistan .Boş gönderebilir
    public string PhotoUrl { get; set; }
    public string Phone { get; set; }
    public string AccessToken { get; set; }
    public bool IsActive { get; set; }
    public string UserTC { get; set; }
    public string UserBeginDate { get; set; }
    public string UserEndDate { get; set; }
    public string UserAddress { get; set; }
    public int UserType { get; set; } //Admin, Customer, Worker .//Role denk geliyor
  }

  public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
  {
    public RegisterViewModelValidator()
    {
      RuleFor(x => x.UserName).NotEmpty().NotNull().Length(3, 50);
      RuleFor(x => x.FullName).NotNull().Length(0, 150);
      RuleFor(x => x.Phone).NotEmpty().NotNull();
      RuleFor(x => x.UserTC).NotEmpty().NotNull();
      RuleFor(x => x.UserEndDate).NotEmpty().NotNull();
      RuleFor(x => x.UserEndDate).NotEmpty().NotNull();
      RuleFor(x => x.IsActive).NotEmpty().When(x => ((bool?)x.IsActive) != false);
    }
  }
}
