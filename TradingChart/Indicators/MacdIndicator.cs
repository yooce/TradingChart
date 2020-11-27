using System.Collections.Generic;
using System.ComponentModel;

namespace MagicalNuts.Indicators
{
	public class MacdIndicatorProperties
	{
		/// <summary>
		/// 短期移動平均期間を設定または取得します。
		/// </summary>
		[Category("MACD")]
		[Description("短期移動平均期間を設定します。")]
		[DefaultValue(12)]
		public int FastPeriod { get; set; } = 12;

		/// <summary>
		/// 長期移動平均期間を設定または取得します。
		/// </summary>
		[Category("MACD")]
		[Description("長期移動平均期間を設定します。")]
		[DefaultValue(26)]
		public int SlowPeriod { get; set; } = 26;

		/// <summary>
		/// シグナル移動平均期間を設定または取得します。
		/// </summary>
		[Category("MACD")]
		[Description("シグナル移動平均期間を設定します。")]
		[DefaultValue(9)]
		public int SignalPeriod { get; set; } = 9;

		/// <summary>
		/// MACDの移動平均計算方法を設定または取得します。
		/// </summary>
		[Category("MACD")]
		[Description("MACDの移動平均計算方法を設定します。")]
		[DefaultValue(MaMethod.Ema)]
		public MaMethod MacdMaMethod { get; set; } = MaMethod.Ema;

		/// <summary>
		/// シグナルの移動平均計算方法を設定または取得します。
		/// </summary>
		[Category("MACD")]
		[Description("シグナルの移動平均計算方法を設定します。")]
		[DefaultValue(MaMethod.Ema)]
		public MaMethod SignalMaMethod { get; set; } = MaMethod.Ema;
	}

	/// <summary>
	/// MACDインジケーターを表します。
	/// </summary>
	public class MacdIndicator : IIndicator
	{
		/// <summary>
		/// MACDインジケーターのプロパティを取得または設定します。
		/// </summary>
		public MacdIndicatorProperties Properties { get; set; }

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
			Properties = new MacdIndicatorProperties();
			FastMaIndicator = new MovingAverageIndicator();
			FastMaIndicator.Properties.Period = Properties.FastPeriod;
			FastMaIndicator.Properties.MaMethod = Properties.MacdMaMethod;
			SlowMaIndicator = new MovingAverageIndicator();
			SlowMaIndicator.Properties.Period = Properties.SlowPeriod;
			SlowMaIndicator.Properties.MaMethod = Properties.MacdMaMethod;
		}

		/// <summary>
		/// 値を取得します。
		/// </summary>
		/// <param name="args">インジケーター引数</param>
		/// <returns>値</returns>
		public double[] GetValues(IndicatorArgs args)
		{
			// 必要期間に満たない
			if (args.Candles.Count < Properties.FastPeriod || args.Candles.Count < Properties.SlowPeriod) return null;

			// キュー作成
			if (MacdQueue == null) MacdQueue = new Queue<double>();

			// 移動平均
			double fast_ma = FastMaIndicator.GetValues(new IndicatorArgs(args.Candles))[0];
			double slow_ma = SlowMaIndicator.GetValues(new IndicatorArgs(args.Candles))[0];

			// MACD
			double macd = fast_ma - slow_ma;

			// キューに格納
			MacdQueue.Enqueue(macd);
			if (MacdQueue.Count > Properties.SignalPeriod) MacdQueue.Dequeue();

			// 必要期間に満たない
			if (MacdQueue.Count < Properties.SignalPeriod) return null;

			// シグナル
			double signal = MovingAverageIndicator.GetMovingAverage(MacdQueue.ToArray(), Properties.SignalMaMethod, PreviousSignal);

			// 次回のために覚えておく
			PreviousSignal = signal;

			return new double[] { macd, signal };
		}
	}
}
