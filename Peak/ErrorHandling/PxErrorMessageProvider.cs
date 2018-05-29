using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peak.Localization;
using Peak.Session;

namespace Peak.ErrorHandling {
  /// <summary>
  /// Hataların çoklu dil desteği vermesi için kullanılan default IPxErrorMessageProvider implementasyonudur.
  /// </summary>
  public class PxErrorMessageProvider : IPxErrorMessageProvider {

    #region Private Member(s)    
    private object[] messageParameters;
    private string _localizedStringCode;

    #endregion

    #region Constructor(s)

    /// <summary>
    /// 
    /// </summary>
    /// <param name="localizedStringCode"></param>
    /// <param name="messageParameters"></param>
    public PxErrorMessageProvider(string localizedStringCode, params object[] messageParameters) {
      this._localizedStringCode = localizedStringCode;
      this.messageParameters = messageParameters;
    }

    #endregion

    #region IErrorMessageProvider Members    

    /// <summary>
    /// Hata mesajını oturumun dilinde getirir.
    /// </summary>
    /// <param name="cultureCode"></param>
    /// <returns></returns>
    public string GetMessage() {
      string message = PxLocalizationManager.Get(_localizedStringCode);
      if (message != null && messageParameters != null && messageParameters.Length > 0) {
        message = string.Format(message, messageParameters);
      }
      return message;
    }

    #endregion
  }

}
