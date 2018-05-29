using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Common.Enums
{
  /// <summary>
  /// Hata önceliği
  /// </summary>
  public enum ErrorPriority {
    /// <summary>
    /// 
    /// </summary>
    Undefined = 0,

    /// <summary>
    /// Direk kullanıcıya bağlı nedenlerden oluşan hatalar.
    /// Ör: 'Ad' alanı boş geçilemez.
    /// Bu durumda kullanıcı, hata mesajından sonra ikinci seferde 'Ad' alanını doldurup gönderir ve süreç devam eder.
    /// </summary>
    Low = 1,

    /// <summary>
    /// Direk kullanıcıya bağlı olmayan ama programın ayarlarından çözülebilecek hatalar.
    /// Ör: Günlük kur bilgisi eksik.
    /// Bu durumda kullanıcı ilgili kişiye/kişilere haber verir ve eksik kur bilgileri tamamlanır, ondan sonra süreç devam eder.
    /// </summary>
    Medium = 2,

    /// <summary>
    /// Hiç bir koşulda gelmemesi gereken, kullanıcının çözemeyeceği teknik hatalar.
    /// Ör: Programdaki kod halarına bağlı hatalar, Sitemdeki problemlere bağlı hatalar, vs.
    /// </summary>
    High = 3
  }
}
