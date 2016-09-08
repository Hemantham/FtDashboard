using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace DataEf.Context
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
   public class ForethoughtRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        private IQueryable<TEntity> _query;

        public ForethoughtRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _query = _dbSet;
        }

        //Get : Filter, pass the lamda expression (filter), incldude properties - comma delimitted list
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
           // IQueryable<TEntity> _query = _dbSet;

            if (filter != null)
            {
                _query = _query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                _query = _query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(_query).ToList();
            }
            else
            {
                return _query.ToList();
            }
        }

      

        public virtual IQueryable<TEntity> GetQuarable(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
           //IQueryable<TEntity> _query = _dbSet;

            if (filter != null)
            {
                _query = _query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                _query = _query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(_query);
            }
            return _query;
        }

        //Count the number of objects : For ex: Count of series in a chart etc
        public virtual int Count( Expression<Func<TEntity, bool>> filter = null )
        {
           // IQueryable<TEntity> query = _dbSet;

            return filter != null ? _query.Count(filter) : _query.ToList().Count;
        }

       //Max : call max on repository. Like the database. Max value for the passed in field for the whole repo.
        public virtual T Max<T>(
           Expression<Func<TEntity, T>> filter = null)
        {
          //  IQueryable<TEntity> _query = _dbSet;

            if (filter != null)
            {
                return   _query.Max(filter);
            }
            else
            {
                return default(T);
            }
        }

       //Filtering criteria to fetch only the very first record. Useful when you know that it will return only one rec 
        public virtual TEntity GetSingle(
            Expression<Func<TEntity, bool>> filter = null,            
            string includeProperties = "")
        {
         //  IQueryable<TEntity> _query = _dbSet;

            if (filter != null)
            {
                _query = _query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                _query = _query.Include(includeProperty);
            }
            return _query?.FirstOrDefault();
        }

        //Get by the ID
        public virtual TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }

       //Insert recs : if its a view without any joins you can insert. not used in this solution
        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }
        
       //Delete recs: by the IDs
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

       //Its the same as delete the ID. only the entity will have a key field and it will try to delete.
        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
            _context.SaveChanges();
        }

       //Update the rec.
        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public virtual ForethoughtRepository<TEntity> Include<TProperty>(Expression<Func<TEntity,TProperty>> expression)
        {
           _query = _query.Include(expression);
           return this;
        }
    }
}