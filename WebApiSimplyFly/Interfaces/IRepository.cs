namespace WebApiSimplyFly.Interfaces
{
    public interface IRepository<T,K>
    {
        public Task<T> GetAsync(K key);
        public Task<List<T>> GetAsync();
        public Task<T> Add(T items);
        public Task<T> Update(T items);
        public Task<T> Delete(K key);
    }
}
