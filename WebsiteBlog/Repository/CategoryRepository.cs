using WebsiteBlog.Data;
using WebsiteBlog.DTO;
using WebsiteBlog.Models;

namespace WebsiteBlog.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _DBcontext;
        public CategoryRepository(ApplicationDbContext context) 
        {
            _DBcontext = context;
        }

        public IEnumerable<Category> GetAll()
        {
            return _DBcontext.Categories.ToList();
        }

    }
}
