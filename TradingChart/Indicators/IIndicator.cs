namespace MagicalNuts.Indicators
{
	/// <summary>
	/// インジケーターのインターフェースを表します。
	/// </summary>
	public interface IIndicator
	{
		/// <summary>
		/// 値を取得します。
		/// </summary>
		/// <param name="args">インジケーター引数</param>
		/// <returns>値</returns>
		double[] GetValues(IndicatorArgs args);
	}
}
