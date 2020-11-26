using System.Collections.Generic;

namespace MagicalNuts.Indicators
{
	/// <summary>
	/// MACDインジケーターを表します。
	/// </summary>
	public class MacdIndicator : IIndicator
	{
		/// <summary>
		/// 短期移動平均期間
		/// </summary>
		public int FastPeriod { get; set; } = 12;

		/// <summary>
		/// 長期移動平均期間
		/// </summary>
		public int SlowPeriod { get; set; } = 26;

		/// <summary>
		/// シグナル移動平均期間
		/// </summary>
		public int SignalPeriod { get; set; } = 9;

		/// <summary>
		/// 短期用移動平均インジケーター
		/// </summary>
		private MovingAverageIndicator FastMaIndicator = null;

		/// <summary>
		/// 長期用移動平均インジケーター
		/// </summary>
		private MovingAverageIndicator SlowMaIndicator = null;

		/// <summary>
		/// MACDのキュー
		/// </summary>
		private Queue<double> MacdQueue = null;

		/// <summary>
		/// 前回のMACDシグナル
		/// </summary>
		private double? PreviousSignal = null;

		/// <summary>
		/// MacdIndicatorの新しいインスタンスを初期化します。
		/// </summary>
		public MacdIndicator()
		{
			FastMaIndicator = new MovingAverageIndicator();
			FastMaIndicator.Period = FastPeriod;
			FastMaIndicator.MaMethod = MaMethod.Ema;
			SlowMaIndicator = new MovingAverageIndicator();
			SlowMaIndicator.Period = SlowPeriod;
			SlowMaIndicator.MaMethod = MaMethod.Ema;
		}

		/// <summary>
		/// 値を取得します。
		/// </summary>
		/// <param name="args">インジケーター引数</param>
		/// <returns>値</returns>
		public double[] GetValues(IndicatorArgs args)
		{
			// 必要期間に満たない
			if (args.Candles.Count < FastPeriod || args.Candles.Count < SlowPeriod) return null;

			// キュー作成
			if (MacdQueue == null) MacdQueue = new Queue<double>();

			// 移動平均
			double fast_ma = FastMaIndicator.GetValues(new IndicatorArgs(args.Candles))[0];
			double slow_ma = SlowMaIndicator.GetValues(new IndicatorArgs(args.Candles))[0];

			// MACD
			double macd = fast_ma - slow_ma;

			// キューに格納
			MacdQueue.Enqueue(macd);
			if (MacdQueue.Count > SignalPeriod) MacdQueue.Dequeue();

			// 必要期間に満たない
			if (MacdQueue.Count < SignalPeriod) return null;

			// シグナル
			double signal = MovingAverageIndicator.GetMovingAverage(MacdQueue.ToArray(), MaMethod.Sma, PreviousSignal);

			// 次回のために覚えておく
			PreviousSignal = signal;

			return new double[] { macd, signal };
		}
	}
}
