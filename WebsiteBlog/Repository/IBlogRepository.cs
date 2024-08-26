using Microsoft.EntityFrameworkCore;
using WebsiteBlog.Models;

namespace WebsiteBlog.Repository
{
    public interface IBlogRepository
    {
        IEnumerable<Blog> Paging(int start, int end);
        IEnumerable<Blog> Paging(string title, int categoryId, int start, int end);
        IEnumerable<Blog> Paging(string title, int categoryId, int userId, int start, int end);

        void Add(Blog entity);

        Blog GetById(int id);
    }
}
