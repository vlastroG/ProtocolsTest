using LibraryService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace LibraryService.Services.Impls
{
    public class LibraryDatabaseContext : ILabraryDatabaseContextService
    {
        private IList<Book> _libraryDatabase;

        public IList<Book> Books => throw new NotImplementedException();

        public LibraryDatabaseContext()
        {
            Initialize();
        }

        private void Initialize()
        {
        }
    }
}