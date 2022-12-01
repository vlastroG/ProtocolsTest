using LibraryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryService.Services.Impls
{
    public class LibraryRepository : ILibraryRepositoryService
    {
        private readonly ILibraryDatabaseContextService _context;


        public LibraryRepository(ILibraryDatabaseContextService context)
        {
            _context = context;
        }


        public IList<Book> GetByAuthor(string author)
        {
            return _context.Books
                .Where(
                     b => !(b.Authors.FirstOrDefault(
                         a => a.Name.IndexOf(author, StringComparison.OrdinalIgnoreCase) >= 0) is null))
                .ToList();
        }

        public IList<Book> GetByCategory(string category)
        {
            return _context.Books
                .Where(
                    book => book.Category
                    .IndexOf(category, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }

        public IList<Book> GetByTitle(string title)
        {
            return _context.Books
                .Where(
                    book => book.Title
                    .IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }
    }
}