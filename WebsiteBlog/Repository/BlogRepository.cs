using Microsoft.EntityFrameworkCore;
using WebsiteBlog.Data;
using WebsiteBlog.Models;

namespace WebsiteBlog.Repository
{
    public class BlogRepository : ICommonRepository<Blog>
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

        public void Delete(Blog entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blog> GetAll()
        {
            throw new NotImplementedException();
        }

        public Blog GetById(int id)
        {
            return _context.Blogs.FirstOrDefault(b => b.BlogId == id);
        }

        public Blog GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Blog GetByTitle(string title)
        {
            throw new NotImplementedException();
        }

        public void Update(Blog entity)
        {
            throw new NotImplementedException();
        }
    }
}
