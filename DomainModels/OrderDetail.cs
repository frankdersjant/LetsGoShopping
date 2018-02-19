namespace DomainModels
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public virtual Order Orders { get; set; }

        public virtual Product Products { get; set; }
    }
}
