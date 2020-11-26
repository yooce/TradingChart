using MathNet.Numerics.Statistics;
using System.Linq;

namespace MagicalNuts.Indicators
{
	/// <summary>
	/// ボリンジャーバンドインジケーターを表します。
	/// </summary>
	public class BollingerBandIndicator : IIndicator
	{
		/// <summary>
		/// 期間
		/// </summary>
		public int Period { get; set; } = 25;

		/// <summary>
		/// 偏差倍率
		/// </summary>
		public double Deviation { get; set; } = 2.0;

		/// <summary>
		/// 移動平均インジケーター
		/// </summary>
		private MovingAverageIndicator MovingAverageIndicator = null;

		/// <summary>
		/// BollingerBandIndicatorクラスの新しいインスタンスを初期化します。
		/// </summary>
		public BollingerBandIndicator()
		{
			MovingAverageIndicator = new MovingAverageIndicator();
			MovingAverageIndicator.Period = Period;
		}

		/// <summary>
		/// 値を取得します。
		/// </summary>
		/// <param name="args">インジケーター引数</param>
		/// <returns>値</returns>
		public double[] GetData(IndicatorArgs args)
		{
			// 必要期間に満たない
			if (args.Candles.Count < Period) return null;

			// 移動平均
			double ma = MovingAverageIndicator.GetData(new IndicatorArgs(args.Candles))[0];

			// 標準偏差
			double dev = args.Candles.GetRange(0, Period).Select(candle => (double)candle.Close).PopulationStandardDeviation();

			return new double[] { ma, ma + dev * Deviation, ma - dev * Deviation };
		}
	}
}
