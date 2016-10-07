using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.API.API.Cache;

namespace Dashboard.Services.Cache
{
    public class MemCachProvider : CachingBase, ICacheProvider
    {
        private static MemCachProvider _memCachProvider;

        public MemCachProvider()
        {
            var x = 1;
        }
        public override void AddItem<T>(string key, T value)
        {
            base.AddItem(key, value);
        }

        public virtual T GetItem<T>(string key)
        {
            return base.GetItem<T>(key,false);
            //Remove default is true because it's Global Cache!
        }

        public override T GetItem<T>(string key, bool remove)
        {
            return base.GetItem<T>(key, remove);
        }

        public T GetItemAndAdd<T>(string key, Func<T> getFromRepo)
        {
            var cache = GetItem<T>(key);

            if (cache == null)
            {
                cache = getFromRepo();
                AddItem(key, cache);
            }
           
              return cache;
            
        }


        public static MemCachProvider Instance
        {
            get
            {
                if (_memCachProvider == null)
                {
                    _memCachProvider = new MemCachProvider();
                }
                return _memCachProvider;
            }
        }

        //class Nested
        //{
        //    // Explicit static constructor to tell C# compiler
        //    // not to mark type as beforefieldinit
        //    static Nested()
        //    {
        //    }
        //    internal static readonly MemCachProvider instance = new MemCachProvider();
        //}


    }
}
