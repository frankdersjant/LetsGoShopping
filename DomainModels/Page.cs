namespace DomainModels
{
    public class Page
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Body { get; set; }
        public bool hasSidebar { get; set; }
        public int Sorting { get; set; }
    }
}
