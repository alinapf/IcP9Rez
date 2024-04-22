using PasechnikovaPR33p19.Domain.Entities;
using PasechnikovaPR33p19.Domain.Services;

namespace PasechnikovaPR33p19.Infrastructure
{
    public class BooksReader : IBooksReader
    {
        private readonly IRepository<Book> books;
        private readonly IRepository<Category> categories;

        public BooksReader(IRepository<Book> books, IRepository<Category> categories)
        {
            this.books = books;
            this.categories = categories;
        }

        public async Task<Book?> FindBookAsync(int bookId) => await books.FindAsync(bookId);

        public async Task<List<Book>> GetAllBooksAsync() => await books.GetAllAsync();

        public async Task<List<Book>> FindBooksAsync(string searchString, int categoryId) => (searchString, categoryId) switch
        {
            ("" or null, 0) => await books.GetAllAsync(),
            (_, 0) => await books.FindWhere(book => book.Title.Contains(searchString) || book.Author.Contains(searchString)),
            (_, _) => await books.FindWhere(book => book.CategoryId == categoryId &&
                (book.Title.Contains(searchString) || book.Author.Contains(searchString))),
        };

        public async Task<List<Category>> GetCategoriesAsync() => await categories.GetAllAsync();
    }
}
