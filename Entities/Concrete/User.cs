using Core.CommonModel;
using Entities.Abstract;
namespace Entities.Concrete
{

  public class User : BaseModel, IEntity
  {
    public string FullName { get; set; }
    public string Title { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string UserTC { get; set; }
    public string PhotoUrl { get; set; } //File tipinde gönderilmesi gerekiyor.Photo adında olmalı
    public string Phone { get; set; }
    public string UserBeginDate { get; set; }
    public string UserEndDate { get; set; }
    public string UserAddress { get; set; }
    public int UserType { get; set; } // Value : UserTypeEnum (Core > Enums)
    public bool IsActive { get; set; } = true;
  }
}
