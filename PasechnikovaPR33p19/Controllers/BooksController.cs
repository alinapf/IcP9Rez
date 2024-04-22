using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PasechnikovaPR33p19.Domain.Entities;
using PasechnikovaPR33p19.Domain.Services;
using PasechnikovaPR33p19.ViewModels;

namespace PasechnikovaPR33p19.Controllers
{
    public class BooksController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index(string searchString = "", int categoryId = 0)
        {
            var viewModel = new BooksCatalogViewModel
            {
                Books = await reader.FindBooksAsync(searchString, categoryId),
                Categories = await reader.GetCategoriesAsync()
            };

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddBook()
        {
            var viewModel = new BookViewModel();

            var categories = await reader.GetCategoriesAsync();

            var items = categories.Select(c =>
                new SelectListItem { Text = c.Name, Value = c.Id.ToString() });

            viewModel.Categories.AddRange(items);
            return View(categories);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            var book = await reader.FindBookAsync(bookId);
            if (book is null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateBook(int bookId)
        {
            var book = await reader.FindBookAsync(bookId);
            if (book is null)
            {
                return NotFound();
            }
            var bookVm = new UpdateBookViewModel
            {
                Id = book.Id,
                Author = book.Author,
                Title = book.Title,
                CategoryId = book.CategoryId,
                PageCount = book.PagesCount,
                FileString = book.Filename,
                PhotoString = book.ImageUrl
            };

            var categories = await reader.GetCategoriesAsync();
            var items = categories.Select(c =>
                new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            bookVm.Categories.AddRange(items);

            return View(bookVm);
        }


        private readonly IBooksReader reader;
        private readonly IWebHostEnvironment appEnvironment;

        public BooksController(IBooksReader reader,
            IWebHostEnvironment appEnvironment)
        {
            this.reader = reader;
            this.appEnvironment = appEnvironment;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddBookAsync(BookViewModel bookVm)
        {
            if (!ModelState.IsValid)
            {
                return View(bookVm);
            }

            try
            {
                var book = new Book
                {
                    Author = bookVm.Author,
                    Title = bookVm.Title,
                    CategoryId = bookVm.CategoryId,
                    PagesCount = bookVm.PageCount
                };
                string wwwroot = appEnvironment.WebRootPath; // получаем путь до wwwroot

            }
            catch (IOException)
            {
                ModelState.AddModelError("ioerror", "Не удалось сохранить файл.");
                return View(bookVm);
            }
            catch
            {
                ModelState.AddModelError("database", "Ошибка при сохранении в базу данных.");
                return View(bookVm);
            }

            return RedirectToAction("Index", "Books");
        }
    }
}
