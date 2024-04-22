using System.ComponentModel.DataAnnotations;

namespace PasechnikovaPR33p19.Domain.Entities
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; set; }
    }
}
