namespace Peak.ErrorHandling {
  /// <summary>
  /// Peak hataları için istenilen dilde mesaj üretir.
  /// </summary>
  public interface IPxErrorMessageProvider {
    /// <summary>
    /// Hata mesajını oturumun dilinde verir.
    /// </summary>
    /// <returns></returns>
    string GetMessage();
  }
}