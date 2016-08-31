namespace DataEf.Context
{
    public interface IUnitOfWork
    {
        ForethoughtRepository<T> GetRepository<T>() where T : class;
        void Save();
    }
}