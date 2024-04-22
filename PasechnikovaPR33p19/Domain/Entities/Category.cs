using System.ComponentModel.DataAnnotations;

namespace PasechnikovaPR33p19.Domain.Entities
{
    public class Category : Entity
    {
        [StringLength(150)]
        public string Name { get; set; } = null!;
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
