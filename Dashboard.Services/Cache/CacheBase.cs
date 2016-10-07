using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Dashboard.API.API.Cache;

namespace Dashboard.Services.Cache
{
    public abstract class CachingBase 
    {
        protected CachingBase()
        {
          
        }

        protected MemoryCache Cache = new MemoryCache("CachingProvider");

        private static readonly object Padlock = new object();

        public virtual void AddItem<T>(string key, T value)
        {
            lock (Padlock)
            {
                Cache.Add(key, value, DateTimeOffset.MaxValue);
            }
        }

      
        public virtual void RemoveItem(string key)
        {
            lock (Padlock)
            {
                Cache.Remove(key);
            }
        }

        public virtual T GetItem<T>(string key, bool remove) 
        {
            lock (Padlock)
            {
                var res = Cache[key];

                if (res != null)
                {
                    if (remove)
                        Cache.Remove(key);
                }
                else
                {
                 
                }

                return (T) res ;
            }
        }
    }
}
