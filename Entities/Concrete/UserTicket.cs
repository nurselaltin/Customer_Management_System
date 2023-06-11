using System;
using System.Security.Claims;
using Entities.Abstract;

namespace Entities.Concrete
{
	public class UserTicket : BaseModel, IEntity
    {
        public long UserId { get; set; }
        public string Token { get; set; }
        public string Ticket { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}

