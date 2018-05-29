using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


namespace Peak.Web.Security {
  public class CaptchaImage : IDisposable {

    #region Private Member(s)

    private readonly PxCaptchaInfo _info;
        
    private Bitmap image;

    private Random random = new Random();

    #endregion

    #region Constructor(s)
    public CaptchaImage(PxCaptchaInfo info) {
      if (info == null) {
        throw new ArgumentNullException("PxCaptchaInfo");
      }
      _info = info;            
      this.generateImage();
    }   
    #endregion

    #region Private Method(s)    

    private void generateImage() {

      if (string.IsNullOrEmpty(Info.Text)) {
        throw new ArgumentNullException("Text");
      }

      if (Info.Width <= 0) {
        throw new ArgumentOutOfRangeException("Width", Info.Width, "Argument out of range, must be greater than zero.");
      }
      if (Info.Height <= 0) {
        throw new ArgumentOutOfRangeException("Height", Info.Height, "Argument out of range, must be greater than zero.");
      }

      try {      
        new Font(Info.FontFamily, 12F).Dispose();        
      }
      catch {
        this.Info.FontFamily = System.Drawing.FontFamily.GenericSerif.Name;
      }

      Bitmap bitmap = new Bitmap(Info.Width, Info.Height, PixelFormat.Format32bppArgb);
      Graphics g = Graphics.FromImage(bitmap);
      g.SmoothingMode = SmoothingMode.AntiAlias;
      Rectangle rect = new Rectangle(0, 0, Info.Width, Info.Height);

      HatchBrush hatchBrush = new HatchBrush(HatchStyle.SmallConfetti,Info.Color, Color.White);
      g.FillRectangle(hatchBrush, rect);

      SizeF size;

      float fontSize = rect.Height + 1;

      Font font;

      do {

        fontSize--;

        font = new Font(Info.FontFamily, fontSize, FontStyle.Bold);

        size = g.MeasureString(Info.Text, font);

      } while (size.Width > rect.Width);

      StringFormat format = new StringFormat();
      format.Alignment = StringAlignment.Center;
      format.LineAlignment = StringAlignment.Center;

      GraphicsPath path = new GraphicsPath();

      path.AddString(Info.Text, font.FontFamily, (int)font.Style, font.Size, rect, format);

      float v = 4F;

      PointF[] points =  { new PointF(this.random.Next(rect.Width) / v, this.random.Next(rect.Height) / v),
                                 new PointF(rect.Width - this.random.Next(rect.Width) / v, this.random.Next(rect.Height) / v),
                                 new PointF(this.random.Next(rect.Width) / v, rect.Height - this.random.Next(rect.Height) / v),
                                 new PointF(rect.Width - this.random.Next(rect.Width) / v, rect.Height - this.random.Next(rect.Height) / v)
                          };

      Matrix matrix = new Matrix();
      matrix.Translate(0F, 0F);
      path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);

      hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, Info.Color, Info.Color);
      g.FillPath(hatchBrush, path);

      int m = Math.Max(rect.Width, rect.Height);

      for (int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++) {
        int x = this.random.Next(rect.Width);
        int y = this.random.Next(rect.Height);
        int w = this.random.Next(m / 50);
        int h = this.random.Next(m / 50);
        g.FillEllipse(hatchBrush, x, y, w, h);
      }

      font.Dispose();
      hatchBrush.Dispose();
      g.Dispose();

      this.image = bitmap;

    }

    #endregion

    #region Property(s)  

    public Bitmap Image {
      get { return this.image; }
    }

    public PxCaptchaInfo Info {
      get {
        return _info;
      }
    }

    #endregion

    #region IDisposable Member(s)
    public void Dispose() {
      GC.SuppressFinalize(this);
      this.Dispose(true);
    }

    protected virtual void Dispose(bool disposing) {
      if (disposing) {
        this.image.Dispose();
      }
    }

    #endregion
  }
}
