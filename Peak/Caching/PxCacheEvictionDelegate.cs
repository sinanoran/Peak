using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Caching
{
  public delegate void PxCacheEvictionDelegate(object key, object value, PxEvictionReason reason, object state);
}
