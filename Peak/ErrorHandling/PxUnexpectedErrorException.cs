using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peak.Common.Enums;
using Peak.Localization;

namespace Peak.ErrorHandling
{
  /// <summary>
  /// Tanımsız hataları kapsulleyen hata sınıfı.
  /// </summary>
  public class PxUnexpectedErrorException : PxException {

    #region Private Member(s)

    private const string ERROR_CODE = "999.9999";

    private class UnexpectedErrorMessageProvider : IPxErrorMessageProvider {

      private string _localizedStringCode = "UnexpectedError";
      private string _innerErrorMessage;
      public UnexpectedErrorMessageProvider(string innerErrorMessage) {
        _innerErrorMessage = innerErrorMessage;
      }

      #region IErrorMessageProvider Members

      public string GetMessage() {
        string msg = PxLocalizationManager.Get(_localizedStringCode);
        if (msg != null) {
          msg= string.Format(msg, _innerErrorMessage);
          return msg;
        }
        return ERROR_CODE;
      }
      #endregion
    }

    #endregion

    #region Constructor(s)

    /// <summary>
    /// 
    /// </summary>
    /// <param name="innerEx"></param>
    public PxUnexpectedErrorException(System.Exception innerEx) : base(innerEx, ERROR_CODE, ErrorPriority.High, new UnexpectedErrorMessageProvider(innerEx.Message)) { }

    #endregion

  }
}
