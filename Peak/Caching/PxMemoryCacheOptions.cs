using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Caching
{
  public class PxMemoryCacheOptions {    

    //
    // Summary:
    //     Gets or sets an absolute expiration date for the cache entry.
    public DateTime? AbsoluteExpiration { get; set; }
    //
    // Summary:
    //     Gets or sets an absolute expiration time, relative to now.
    public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
   
    //
    // Summary:
    //     Gets or sets the callbacks will be fired after the cache entry is evicted from
    //     the cache.
    public PxCacheEvictionDelegate Callback{ get; }
    //
    // Summary:
    //     Gets or sets the priority for keeping the cache entry in the cache during a memory
    //     pressure triggered cleanup. The default is Microsoft.Extensions.Caching.Memory.CacheItemPriority.Normal.
    public PxCacheItemPriority Priority { get; set; }
  
  }
}
