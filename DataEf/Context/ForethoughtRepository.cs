using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data;

namespace DashboardMaster.Domain.DataContext
{

   /// <summary>
   /// implementation of the repository pattern for DB Context
   /// auther:hemantha
   /// date:2014-3-22
   /// </summary>
   /// <typeparam name="TEntity"></typeparam>

   // Anything defined in the context layer is passed here
   public class ForethoughtRepository<TEntity> where TEntity : class
    {
        internal DbContext context;
        internal DbSet<TEntity> dbSet;

        public ForethoughtRepository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        //Get : Filter, pass the lamda expression (filter), incldude properties - comma delimitted list
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

      

        //H.M.2015.05.11------------------------------------------------------------------------
        public virtual IQueryable<TEntity> GetQuarable(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }
        //--------------------------------------------------------------------------------------

        //Count the number of objects : For ex: Count of series in a chart etc
        public virtual int Count( Expression<Func<TEntity, bool>> filter = null )
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                return  query.Count(filter);
            }            
            else
            {
                return query.ToList().Count;
            }
        }

       //Max : call max on repository. Like the database. Max value for the passed in field for the whole repo.
        public virtual T Max<T>(
           Expression<Func<TEntity, T>> filter = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                return   query.Max(filter);
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
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (query != null)
            {
                return query.FirstOrDefault();
            }
            
                return null;
           
        }

        //Get by the ID
        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

       //Insert recs : if its a view without any joins you can insert. not used in this solution
        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
            //H.M.2015.05.11-------------
            context.SaveChanges();
            //---------------------------
        }
        
       //Delete recs: by the IDs
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

       //Its the same as delete the ID. only the entity will have a key field and it will try to delete.
        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
            //H.M.2015.05.11-------------
            context.SaveChanges();
            //---------------------------
        }

       //Update the rec.
        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
            //H.M.2015.05.11-------------
            context.SaveChanges();
            //---------------------------
        }

       // IQueryable<T> Query<T>() { return context.GetTable<TEntity>(); }

    }
}