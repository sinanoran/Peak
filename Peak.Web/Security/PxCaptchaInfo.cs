using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Peak.Web.Security{

  /// <summary>
  /// Default değerleri login ekranındaki kontrollerden alınmıştır.  
  /// </summary>
  public class PxCaptchaInfo {

    #region Private Member(s)

    private string _text;
    private int _width = 290;
    private int _height = 43;
    private string _fontFamily = System.Drawing.FontFamily.GenericSerif.Name;
    private Color _color = ColorTranslator.FromHtml("#1BA39C");

    #endregion

    #region Public Property(s)

    public int Height {
      get { return _height; }
      set { _height = value; }
    }

    public string Text {
      get { return _text; }
      set { _text = value; }
    }

    public int Width {
      get { return _width; }
      set { _width = value; }
    }

    public string FontFamily {
      get { return _fontFamily; }
      set { _fontFamily = value; }
    }

    public Color Color {
      get { return _color; }
      set { _color = value; }
    }

    #endregion
  }
}
