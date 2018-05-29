using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Peak.Session;
using Microsoft.AspNetCore.Http;


namespace Peak.Web.Security{
  public class PxCaptchaGenerator {

    #region Private Method(s)

    private static string generateRandomCode(int length) {
      Random random = new Random();
      var mch = Enumerable.Range(49, 9).Select(c => (char)c).Union(Enumerable.Range(65, 26).Select(c => (char)c)).ToArray();
      string s = new String(Enumerable.Range(1, length).Select(a => mch[random.Next() % mch.Length]).ToArray());
      return s;
    }

    #endregion

    #region Public Method(s)

    /// <summary>
    /// Eğer gönderilen parametrede Text propertysine değer atanmamış ise gönderilen textlength parametresine göre random string oluşturur.
    /// Default textLength 8 karakterdir. Session'a CaptchaImageText adında yeni bir key oluşturup, oluşturduğu string değeri bu keye set eder.
    /// Geriye Image'in base64 stringi dönmektedir. Örnek : "data:image/png;base64, ........."
    /// </summary>
    /// <param name="textLength"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="fontFamily"></param>
    /// <returns></returns>
    public static string CreateCaptcha(PxCaptchaInfo info, int textLength = 8) {
      if (string.IsNullOrEmpty(info.Text)) {
        info.Text = generateRandomCode(textLength);
      }
      byte[] image = null;
      using (CaptchaImage img = new CaptchaImage(info)) {
        ImageConverter converter = new ImageConverter();
        image = (byte[])converter.ConvertTo(img.Image, typeof(byte[]));
      }
      PxSession session = PxSession.Get();
      session.Set<string>("CaptchaImageText", info.Text);
      return string.Format("data:image/png;base64,{0}", Convert.ToBase64String(image));
    }

    public static string CreateCaptcha() {
      return CreateCaptcha(new PxCaptchaInfo());
    }

    #endregion

  }
}
