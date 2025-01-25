namespace MediaSoft.Models.Entities
{
    public class NoteDB
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Content { get; set; }

        // связь с категорией
        public CategoryDB Category { get; set; }
    }
}
