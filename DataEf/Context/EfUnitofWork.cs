using System;
using System.Data.Entity;

namespace DataEf.Context
{
  
    public class EfUnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly DbContext _context;

        public EfUnitOfWork(string connection)
        {
            _context = new DashboardContext(connection);
        }

        public EfUnitOfWork()
        {
            _context = new DashboardContext();
        }

        //using this you dont have to create properties for each type as done above.
        public ForethoughtRepository<T> GetRepository<T>() where T : class
        {           
             return  new ForethoughtRepository<T>(_context);              
        }   

        //After inserting you have to save if you are to insert values in.
        public void Save()
        {
            _context.SaveChanges();
        }

        //It will be called automatically at the time of GC
        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}