namespace MagicalNuts.Indicators
{
	/// <summary>
	/// ロウソク足インジケーターを表します。
	/// </summary>
	public class CandleIndicator : IIndicator
	{
		/// <summary>
		/// 値を取得します。
		/// </summary>
		/// <param name="args">インジケーター引数</param>
		/// <returns>値</returns>
		public double[] GetValues(IndicatorArgs args)
		{
			return new double[4] {
				(double)args.Candles[0].High, (double)args.Candles[0].Low, (double)args.Candles[0].Open, (double)args.Candles[0].Close };
		}
	}
}
