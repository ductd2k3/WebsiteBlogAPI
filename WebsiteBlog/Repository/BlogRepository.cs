using Microsoft.EntityFrameworkCore;
using WebsiteBlog.Data;
using WebsiteBlog.Models;

namespace WebsiteBlog.Repository
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ApplicationDbContext _context;
        public BlogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Blog> Paging(int start, int end) 
        {
            return _context.Blogs
                .OrderByDescending(blog => blog.BlogId)
                .Skip(start).Take(end);
        }

        public void Add(Blog entity)
        {
            _context.Blogs.Add(entity);
            _context.SaveChanges();
        }

        public Blog GetById(int id)
        {
            return _context.Blogs.FirstOrDefault(b => b.BlogId == id);
        }

        public IEnumerable<Blog> Paging(string title, int categoryId, int start, int end)
        {
            return _context.Blogs
                .Where(b => b.Title.Contains(title) && b.CategoryId == categoryId )
                .OrderByDescending (b => b.BlogId).ToList();
        }

        public IEnumerable<Blog> Paging(string title, int categoryId, int userId, int start, int end)
        {
            return _context.Blogs
                .Where(b => b.Title.Contains(title) && b.CategoryId == categoryId && b.UserId == userId )
                .OrderByDescending(b => b.BlogId).ToList();
        }
    }
}
