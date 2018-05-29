using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Common {
  /// <summary>
  /// Singleton sınıflar için kullanılmaktadır.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public abstract class SingletonBase<T> where T : new() {

    internal const string CTOR_OBSOLETE_MSG = "Use 'Instance' property instead of creating new instance.";

    private static class Creator<TSingleton> where TSingleton : new() {
      internal static readonly TSingleton instance = new TSingleton();
      static Creator() { }
    }

    /// <summary>
    /// 
    /// </summary>
    public static T Instance { get { return Creator<T>.instance; } }
  }
}
