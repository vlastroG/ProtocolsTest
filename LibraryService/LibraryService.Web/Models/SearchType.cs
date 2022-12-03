using System.ComponentModel.DataAnnotations;

namespace LibraryService.Web.Models
{
    /// <summary>
    /// Категории для поиска
    /// </summary>
    public enum SearchType
    {
        /// <summary>
        /// Название книги
        /// </summary>
        [Display(Name = "Заголовок")]
        Title,
        /// <summary>
        /// Автор книги (один из)
        /// </summary>
        [Display(Name = "Автор")]
        Author,
        /// <summary>
        /// Каткгория книги
        /// </summary>
        [Display(Name = "Категория")]
        Category
    }
}
