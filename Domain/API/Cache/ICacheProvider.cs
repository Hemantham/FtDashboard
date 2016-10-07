using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.API.API.Cache
{
    public interface ICacheProvider
    {
        void AddItem<T>(string key, T value);
        T GetItem<T>(string key);
        T GetItemAndAdd<T>(string key, Func<T> getFromRepo);
    }
}
