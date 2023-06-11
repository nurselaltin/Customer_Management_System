using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
  public partial class CompanySetting : BaseModel, IEntity
  { 
    public string SettingKey { get; set; }
    public string SettingVal { get; set; }
  }
}
