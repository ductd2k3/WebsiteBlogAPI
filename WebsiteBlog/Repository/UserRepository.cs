using WebsiteBlog.Data;
using WebsiteBlog.Models;

namespace WebsiteBlog.Repository
{
    public class UserRepository : IUserReposiroty
    {
        private readonly ApplicationDbContext _DbContext;
        public UserRepository(ApplicationDbContext context)
        {
            _DbContext = context;
        }
        public User GetUserByEmailPassword(string email, string password)
        {
            return _DbContext.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public void AddUser(string username, string email, string password)
        {
            User user = new User
            {
                UserName = username,
                Email = email,
                Password = password,
                role = 2
            };
            _DbContext.Users.Add(user);
            _DbContext.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _DbContext.Users.Update(user);
            _DbContext.SaveChanges();
        }

        public User GetUserByEmail(string email)
        {
            return _DbContext.Users.FirstOrDefault(user => user.Email == email);
        }
    }
}
