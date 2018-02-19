namespace DomainModels
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public string ImageName { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category category { get; set; }
       
    }
}
