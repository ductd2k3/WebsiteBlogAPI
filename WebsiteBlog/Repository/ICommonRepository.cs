namespace WebsiteBlog.Repository
{
    public interface ICommonRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> Paging(int start, int end);
        T GetById(int id);
        T GetByName(string name);
        T GetByTitle(string title);
        void Update(T entity);
        void Delete(T entity);
        void Add(T entity);

    }
}
