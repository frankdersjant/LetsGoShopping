using System.ComponentModel.DataAnnotations;

namespace WebUI.ViewModels
{
    public class PageVM
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 50,  MinimumLength = 3)]
        public string Title { get; set; }

        public string Slug { get; set; }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = 3)]
        public string Body { get; set; }

        public int Sorting { get; set; }
        public bool hasSidebar { get; set; }
    }
}