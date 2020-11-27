using MathNet.Numerics.Statistics;
using System.ComponentModel;
using System.Linq;

namespace MagicalNuts.Indicators
{
	public class BollingerBandIndicatorProperties
	{
		/// <summary>
		/// 対象期間を設定または取得します。
		/// </summary>
		[Category("ボリンジャーバンド")]
		[Description("対象期間を設定します。")]
		[DefaultValue(25)]
		public int Period { get; set; } = 25;

		/// <summary>
		/// 標準偏差の倍率を設定または取得します。
		/// </summary>
		[Category("ボリンジャーバンド")]
		[Description("標準偏差の倍率を設定します。")]
		[DefaultValue(2.0)]
		public double Deviation { get; set; } = 2.0;
	}

	/// <summary>
	/// ボリンジャーバンドインジケーターを表します。
	/// </summary>
	public class BollingerBandIndicator : IIndicator
	{
		/// <summary>
		/// ボリンジャーバンドインジケーターのプロパティを表します。
		/// </summary>
		public BollingerBandIndicatorProperties Properties { get; set; }

		/// <summary>
		/// 移動平均インジケーター
		/// </summary>
		private MovingAverageIndicator MovingAverageIndicator = null;

		/// <summary>
		/// BollingerBandIndicatorクラスの新しいインスタンスを初期化します。
		/// </summary>
		public BollingerBandIndicator()
		{
			Properties = new BollingerBandIndicatorProperties();
			MovingAverageIndicator = new MovingAverageIndicator();
			MovingAverageIndicator.Properties.Period = Properties.Period;
		}

		/// <summary>
		/// 値を取得します。
		/// </summary>
		/// <param name="args">インジケーター引数</param>
		/// <returns>値</returns>
		public double[] GetValues(IndicatorArgs args)
		{
			// 必要期間に満たない
			if (args.Candles.Count < Properties.Period) return null;

			// 移動平均
			double ma = MovingAverageIndicator.GetValues(new IndicatorArgs(args.Candles))[0];

			// 標準偏差
			double dev = args.Candles.GetRange(0, Properties.Period).Select(candle => (double)candle.Close).PopulationStandardDeviation();

			return new double[] { ma, ma + dev * Properties.Deviation, ma - dev * Properties.Deviation };
		}
	}
}
