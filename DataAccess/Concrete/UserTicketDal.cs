using System;
using Core.ORM.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Context;
using Entities.Concrete;

namespace DataAccess.Concrete
{
	public class UserTicketDal : EfRepositoryBase<UserTicket, MTSContext>, IUserTicketDal
	{
	}
}

