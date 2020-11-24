using System.Collections.Generic;

namespace MagicalNuts.Indicators
{
	/// <summary>
	/// インジケーター引数を表します。
	/// </summary>
	public class IndicatorArgs
	{
		/// <summary>
		/// 直近の添え字が０、古いほど添え字が大きいロウソク足のリスト
		/// </summary>
		public List<DataTypes.Candle> Candles { get; set; }

		/// <summary>
		/// IndicatorArgsクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="candles">直近の添え字が０、古いほど添え字が大きいロウソク足のリスト</param>
		public IndicatorArgs(List<DataTypes.Candle> candles)
		{
			Candles = candles;
		}
	}
}
