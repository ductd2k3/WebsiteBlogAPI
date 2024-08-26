using Microsoft.EntityFrameworkCore;
using WebsiteBlog.Models;

namespace WebsiteBlog.Repository
{
    public interface ICategoryRepository
    {
        public IEnumerable<Category> GetAll();
    }
}
