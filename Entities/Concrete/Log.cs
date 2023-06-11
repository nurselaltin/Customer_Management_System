using Entities.Abstract;

namespace Entities.Concrete
{
    public class Log : IEntity
    {
    public int Id { get; set; }
    public System.DateTime SystemDate { get; set; }
    public string LogType { get; set; }
    public string Part { get; set; }
    public string Message { get; set; }
    public string IpAddress { get; set; }
    public string User { get; set; }
  }
}
