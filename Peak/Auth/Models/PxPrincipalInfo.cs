using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peak.Configuration;
using System.ComponentModel;
using System.Globalization;
using System.Security.Principal;

namespace Peak.Auth.Models {

  /// <summary>
  /// 
  /// </summary>
  public class PxPrincipalInfo : INotifyPropertyChanged, IPrincipal {

    #region Private Member(s)

    private int _userId;
    private string _userName;
    private string _cultureCode;
    private string _name;
    private string _middleName;
    private string _surname;
    private string _fullName;
    private string _phoneNumber;
    private string _emailAddress;
    private byte[] _profileImage;
    private string _companyName;
    private decimal _companyId;


    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    protected void OnPropertyChanged(string pName) {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) {
        handler(this, new PropertyChangedEventArgs(pName));
      }
    }


    #region Constructor(s)

    /// <summary>
    /// 
    /// </summary>
    public PxPrincipalInfo() {
      this.Credentials = new PxCredentialInfo();
      this.Authentication = new PxAuthenticationInfo();
      this.Authorization = new PxAuthorizationInfo();

      this.CultureCode = this.getCulture();
    }

    #endregion

    #region Private Method(s)

    private void setFullName() {
      bool isSpaceLock = false;
      _fullName = null;

      if (!string.IsNullOrEmpty(Name)) {
        _fullName = Name;
        isSpaceLock = true;
      }

      if (!string.IsNullOrEmpty(MiddleName)) {
        if (isSpaceLock) {
          _fullName += " ";
        }
        _fullName += MiddleName;
        isSpaceLock = true;
      }

      if (!string.IsNullOrEmpty(Surname)) {
        if (isSpaceLock) {
          _fullName += " ";
        }
        _fullName += Surname;
      }

    }
    private string getCulture() {
      //if (CultureInfo.CurrentCulture != null && !string.IsNullOrEmpty(CultureInfo.CurrentCulture.Name)) {
      //  return CultureInfo.CurrentCulture.Name;
      //}
      //else {
        return PxConfigurationManager.PxConfig.Localization.DefaultCulture;
      //}      
    }

    #endregion

    #region Public Property(s)

    /// <summary>
    /// 
    /// </summary>
    public PxCredentialInfo Credentials { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public PxAuthenticationInfo Authentication { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public PxAuthorizationInfo Authorization { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int UserId {
      get {
        return _userId;
      }
      set {
        _userId = value;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public string CultureCode {
      get {
        return _cultureCode;
      }
      set {
        _cultureCode = value;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public string Surname {
      get {
        return _surname;
      }
      set {
        _surname = value;
        setFullName();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public string Name {
      get {
        return _name;
      }
      set {
        _name = value;
        setFullName();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public string MiddleName {
      get {
        return _middleName;
      }
      set {
        _middleName = value;
        setFullName();
      }
    }


    /// <summary>
    /// 
    /// </summary>
    public string CompanyName {
      get {
        return _companyName;
      }
      set {
        _companyName = value;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public decimal CompanyId {
      get {
        return _companyId;
      }
      set {
        _companyId = value;
      }
    }
    /// <summary>
    /// 
    /// </summary>
    public string FullName {
      get {
        return _fullName;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public string PhoneNumber {
      get {
        return _phoneNumber;
      }
      set {
        _phoneNumber = value;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public string EmailAddress {
      get {
        return _emailAddress;
      }
      set {
        _emailAddress = value;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public byte[] ProfileImage {
      get {
        return _profileImage;
      }
      set {
        _profileImage = value;
      }
    }

    public string UserName {
      get {
        return _userName;
      }

      set {
        _userName = value;
      }
    }

    public IIdentity Identity {
      get {
        return this.Authentication;
      }
    }

    public void Clear() {
      this.Credentials.Clear();
      this.Authentication.Clear();
      this.Authorization.Clear();

      this._userId = 0;
      this._name = null;
      this._surname = null;
      this._middleName = null;
      this._phoneNumber = null;
      this._emailAddress = null;
      this._profileImage = null;
      this._companyName = null;
    }

    public bool IsInRole(string role) {
      throw new NotImplementedException();
    }

    #endregion

  }
}
