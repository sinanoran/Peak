using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Caching {

  public enum PxEvictionReason {
    None = 0,
    //
    // Summary:
    //     Manually
    Removed = 1,
    //
    // Summary:
    //     Overwritten
    Replaced = 2,
    //
    // Summary:
    //     Timed out
    Expired = 3,
    //
    // Summary:
    //     Event
    TokenExpired = 4,
    //
    // Summary:
    //     GC, overflow
    Capacity = 5
  }

  //
  // Summary:
  //     Specifies how items are prioritized for preservation during a memory pressure
  //     triggered cleanup.
  public enum PxCacheItemPriority {
    Low = 0,
    Normal = 1,
    High = 2,
    NeverRemove = 3
  }
}
