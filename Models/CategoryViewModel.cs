using MediaSoft.Models.Entities;

namespace MediaSoft.Models
{
    public class CategoryViewModel
    {
        public AddCategoryViewModel NewCategory { get; set; } = new AddCategoryViewModel();
        public List<CategoryDB> Categories { get; set; } = new List<CategoryDB>();

        public List<NoteDB> Notes { get; set; } = new List<NoteDB>();

        public Guid? EditingNoteId { get; set; }
    }
}
