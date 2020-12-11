using MathNet.Numerics.Statistics;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MagicalNuts.Indicators
{
	/// <summary>
	/// ボリンジャーバンドインジケーターを表します。
	/// </summary>
	public class BollingerBandIndicator : IIndicator
	{
		/// <summary>
		/// 標準偏差の倍率を設定または取得します。
		/// </summary>
		[Category("ボリンジャーバンド")]
		[DisplayName("標準偏差の倍率")]
		[Description("標準偏差の倍率を設定します。")]
		public double Deviation { get; set; } = 2.0;

		/// <summary>
		/// 移動平均インジケーターを設定または取得します。
		/// </summary>
		[Category("ボリンジャーバンド")]
		[DisplayName("移動平均")]
		[Description("移動平均を設定します。")]
		[TypeConverter(typeof(MovingAverageIndicatorConverter))]
		public MovingAverageIndicator MovingAverageIndicator { get; set; }

		/// <summary>
		/// BollingerBandIndicatorクラスの新しいインスタンスを初期化します。
		/// </summary>
		public BollingerBandIndicator()
		{
			MovingAverageIndicator = new MovingAverageIndicator();
		}

		/// <summary>
		/// 非同期で準備します。
		/// </summary>
		/// <returns>非同期タスク</returns>
		public async Task SetUpAsync() { }

		/// <summary>
		/// 値を取得します。
		/// </summary>
		/// <param name="args">インジケーター引数</param>
		/// <returns>値</returns>
		public double[] GetValues(IndicatorArgs args)
		{
			// 必要期間に満たない
			if (args.Candles.Count < MovingAverageIndicator.Period) return null;

			// 移動平均
			double ma = MovingAverageIndicator.GetValues(new IndicatorArgs(args.Candles))[0];

			// 標準偏差
			double dev = args.Candles.GetRange(0, MovingAverageIndicator.Period).Select(candle => (double)candle.Close)
				.PopulationStandardDeviation();

			return new double[] { ma, ma + dev * Deviation, ma - dev * Deviation };
		}
	}
}
