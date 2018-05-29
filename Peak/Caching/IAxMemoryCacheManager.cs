using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Caching {
  public interface IPxMemoryCacheManager {

    bool TryGet<T>(object key, out T value);
    T Get<T>(object key);
    T Set<T>(object key, T value);
    T Set<T>(object key, T value, DateTime absoluteExpiration);
    T Set<T>(object key, T value, TimeSpan absoluteExpirationRelativeToNow);
    T Set<T>(object key, T value, PxMemoryCacheOptions options);
    bool Contains(object key);
    void Remove(object key);
  }
}
