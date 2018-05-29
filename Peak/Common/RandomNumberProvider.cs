using System;
using System.Security.Cryptography;

namespace Peak.Common
{
  /// <summary>
  /// Random sayı oluşturur.
  /// </summary>
  public class RandomNumberProvider {
    #region Private Members

    private const int BUFFER_SIZE = 1024;
    private byte[] randomBuffer;
    private int bufferOffset;
    private RNGCryptoServiceProvider rngProvider;
    #endregion

    #region Constructor

    /// <summary>
    /// 
    /// </summary>
    public RandomNumberProvider() {
      randomBuffer = new byte[BUFFER_SIZE];
      rngProvider = new RNGCryptoServiceProvider();
      bufferOffset = randomBuffer.Length;
    }

    #endregion

    #region Private Methods
    private void fillBuffer() {
      rngProvider.GetBytes(randomBuffer);
      bufferOffset = 0;
    }

    #endregion Private Methods

    #region Public Methods

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int Next() {
      if (bufferOffset >= randomBuffer.Length) {
        fillBuffer();
      }
      int val = BitConverter.ToInt32(randomBuffer, bufferOffset) & 0x7fffffff;
      bufferOffset += sizeof(int);
      return val;
    }

    /// <summary>
    /// Max Value'ya göre random sayı oluşturur.
    /// </summary>
    /// <param name="maxValue"></param>
    /// <returns></returns>
    public int Next(int maxValue) {
      return Next() % maxValue;
    }

    /// <summary>
    /// Gönderilen Min ve Max değerleri arasında random sayı oluşturur.
    /// </summary>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <returns></returns>
    public int Next(int minValue, int maxValue) {
      if (maxValue < minValue) {
        throw new ArgumentOutOfRangeException("MaxValue must be greater than or equal to minValue");
      }
      int range = maxValue - minValue;
      return minValue + Next(range);
    }
    #endregion
  }
}
