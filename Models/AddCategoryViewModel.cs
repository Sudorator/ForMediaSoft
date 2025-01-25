using System.ComponentModel.DataAnnotations;

namespace MediaSoft.Models
{
    public class AddCategoryViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public  string Name { get; set; } = string.Empty;
    }
}
