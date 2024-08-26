using WebsiteBlog.Models;

namespace WebsiteBlog.Repository
{
    public interface IUserReposiroty
    {
        User GetUserByEmailPassword(string email, string password);
        User GetUserByEmail(string email);
        void AddUser(string username, string email, string password);
        void UpdateUser(User user);
    }
}
