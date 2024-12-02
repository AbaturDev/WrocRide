namespace WrocRide.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
