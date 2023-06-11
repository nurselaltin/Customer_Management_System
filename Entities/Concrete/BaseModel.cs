namespace Entities.Concrete
{
	public class BaseModel
	{
        public int ID { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int UpdatedUserId { get; set; }

    }
}

