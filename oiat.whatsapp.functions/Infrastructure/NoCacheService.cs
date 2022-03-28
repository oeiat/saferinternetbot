using mbit.common.cache;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace oiat.whatsapp.functions.Infrastructure
{
    public class NoCacheService : ICacheService
    {
        public T GetItem<T>(Func<T> func, params object[] keys) where T : class
        {
            return func();
        }

        public async Task<T> GetItemAsync<T>(Func<Task<T>> func, params object[] keys) where T : class
        {
            return await func();
        }

        public T GetItemWithExpiration<T>(Func<T> func, int expirationMinutes, params object[] keys) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T> GetItemWithExpirationAsync<T>(Func<Task<T>> func, int expirationMinutes, params object[] keys) where T : class
        {
            throw new NotImplementedException();
        }

        public bool HasItem(params object[] keys) => false;

        public void InvalidateAll()
        {
        }

        public void InvalidateItem(params object[] keys)
        {
        }

        public void InvalidateItemBySingleKey(object key, int maxDepth = 3)
        {
            throw new NotImplementedException();
        }

        public void InvalidateItemIfContained(object key)
        {
            throw new NotImplementedException();
        }
    }
}
