using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PasechnikovaPR33p19.ViewModels
{
    public class BookViewModel
    {
        [MinLength(2)]
        [MaxLength(150)]
        [Display(Name = "Название книги")]
        public string Title { get; set; } = string.Empty;


        [MinLength(2)]
        [MaxLength(150)]
        [Display(Name = "Автор")]
        public string Author { get; set; } = string.Empty;


        [Range(0, 10000)]
        [Display(Name = "Количество страниц")]
        public int PageCount { get; set; }


        [Display(Name = "Изображение")]
        [Required]
        public IFormFile Photo { get; set; }


        [Display(Name = "Файл книги (pdf)")]
        [Required]
        public IFormFile File { get; set; }


        [Required]
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }

        public List<SelectListItem> Categories { get; set; } = new();
    }
}
