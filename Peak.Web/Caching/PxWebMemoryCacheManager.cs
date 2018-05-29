using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peak.Caching;
using Peak.Common;
using Microsoft.Extensions.Caching.Memory;

namespace Peak.Web.Caching {
  public class PxWebMemoryCacheManager : SingletonBase<PxWebMemoryCacheManager>, IPxMemoryCacheManager {

    #region Private Member(s)

    private IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());

    #endregion

    //#region Internal Property(s)


    ///// <summary>
    ///// Startup'da Configure Services'de setlenir.
    ///// </summary>
    //internal IMemoryCache MemoryCache {
    //  get {
    //    return _memoryCache;
    //  }

    //  set {
    //    _memoryCache = value;
    //  }
    //}

    //#endregion

    #region Public Method(s)

    public bool Contains(object key) {
      object result;
      return _memoryCache.TryGetValue(key, out result);
    }

    public object this[object key] {
      get { return _memoryCache.Get(key); }
    }
    
    public bool TryGet<T>(object key,out T value) {
      return _memoryCache.TryGetValue<T>(key, out value);
    }
            
    public T Get<T>(object key) {      
      return _memoryCache.Get<T>(key);
    }

    public T Set<T>(object key, T value) {
      return _memoryCache.Set<T>(key, value);
    }

    public T Set<T>(object key, T value, DateTime absoluteExpiration) {
      return _memoryCache.Set<T>(key, value, new DateTimeOffset(absoluteExpiration));
    }

    public T Set<T>(object key, T value, TimeSpan absoluteExpirationRelativeToNow) {
      return _memoryCache.Set<T>(key, value, absoluteExpirationRelativeToNow);
    }

    public T Set<T>(object key, T value, PxMemoryCacheOptions options) {
      MemoryCacheEntryOptions entryOption = new MemoryCacheEntryOptions()
      {
        AbsoluteExpiration = options.AbsoluteExpiration.HasValue ? new DateTimeOffset?((DateTime)options.AbsoluteExpiration) : null,
        AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow,
        Priority = (CacheItemPriority)options.Priority
      };
      entryOption.RegisterPostEvictionCallback((cacheKey, cacheValue, reason, substate) =>
      {
        options.Callback(cacheKey, cacheValue, (PxEvictionReason)reason, substate);
      });
      return _memoryCache.Set<T>(key, value,entryOption);
    }

    public void Remove(object key) {
       _memoryCache.Remove(key);
    }
    #endregion
  }

}
