namespace MagicalNuts.Indicators
{
	/// <summary>
	/// 出来高インジケーターを表します。
	/// </summary>
	public class VolumeIndicator : IIndicator
	{
		/// <summary>
		/// 値を取得します。
		/// </summary>
		/// <param name="args">インジケーター引数</param>
		/// <returns>値</returns>
		public double[] GetValues(IndicatorArgs args)
		{
			return new double[] { args.Candles[0].Volume };
		}
	}
}
