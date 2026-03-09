namespace DevSpot.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllSync();

        Task<T> GetbyIdAsync(int id);

        Task AddASync(T entity);

        Task UpdateASync(T entity);

        Task DeleteASync(int id);
    }
}
