﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MagicalNuts.Indicators
{
	/// <summary>
	/// MACDインジケーターを表します。
	/// </summary>
	public class MacdIndicator : IndicatorBase
	{
		/// <summary>
		/// シグナルの移動平均期間を設定または取得します。
		/// </summary>
		[Category("MACD")]
		[DisplayName("シグナルの移動平均期間")]
		[Description("シグナルの移動平均期間を設定します。")]
		public int SignalPeriod { get; set; } = 9;

		/// <summary>
		/// シグナルの移動平均計算方法を設定または取得します。
		/// </summary>
		[Category("MACD")]
		[DisplayName("シグナルの移動平均計算方法")]
		[Description("シグナルの移動平均計算方法を設定します。")]
		public MaMethod SignalMaMethod { get; set; } = MaMethod.Sma;

		/// <summary>
		/// 短期用移動平均インジケーターを設定または取得します。
		/// </summary>
		[Category("MACD")]
		[DisplayName("短期用移動平均")]
		[Description("短期移動平均を設定します。")]
		[TypeConverter(typeof(MovingAverageIndicatorConverter))]
		public MovingAverageIndicator FastMaIndicator { get; set; }

		/// <summary>
		/// 長期用移動平均インジケーターを設定または取得します。
		/// </summary>
		[Category("MACD")]
		[DisplayName("長期用移動平均")]
		[Description("長期移動平均を設定します。")]
		[TypeConverter(typeof(MovingAverageIndicatorConverter))]
		public MovingAverageIndicator SlowMaIndicator { get; set; }

		/// <summary>
		/// MACDのキュー
		/// </summary>
		[Browsable(false)]
		private Queue<double> MacdQueue = null;

		/// <summary>
		/// 前回のMACDシグナル
		/// </summary>
		[Browsable(false)]
		private double? PreviousSignal = null;

		/// <summary>
		/// MacdIndicatorの新しいインスタンスを初期化します。
		/// </summary>
		public MacdIndicator()
		{
			FastMaIndicator = new MovingAverageIndicator();
			FastMaIndicator.Period = 12;
			FastMaIndicator.MaMethod = MaMethod.Ema;
			SlowMaIndicator = new MovingAverageIndicator();
			SlowMaIndicator.Period = 26;
			SlowMaIndicator.MaMethod = MaMethod.Ema;
		}

		/// <summary>
		/// 非同期で準備します。
		/// </summary>
		/// <returns>非同期タスク</returns>
		public override async Task SetUpAsync()
		{
			MacdQueue = null;
			PreviousSignal = null;
		}

		/// <summary>
		/// 値を取得します。
		/// </summary>
		/// <param name="args">インジケーター引数</param>
		/// <returns>値</returns>
		public override double[] GetValues(DataTypes.CandleCollection candles)
		{
			// 必要期間に満たない
			if (candles.Count < FastMaIndicator.Period || candles.Count < SlowMaIndicator.Period) return null;

			// キュー作成
			if (MacdQueue == null) MacdQueue = new Queue<double>();

			// 移動平均
			double fast_ma = FastMaIndicator.GetValues(candles)[0];
			double slow_ma = SlowMaIndicator.GetValues(candles)[0];

			// MACD
			double macd = fast_ma - slow_ma;

			// キューに格納
			MacdQueue.Enqueue(macd);
			if (MacdQueue.Count > SignalPeriod) MacdQueue.Dequeue();

			// 必要期間に満たない
			if (MacdQueue.Count < SignalPeriod) return null;

			// シグナル
			double signal = MovingAverageIndicator.GetMovingAverage(MacdQueue.ToArray(), SignalMaMethod, PreviousSignal);

			// 次回のために覚えておく
			PreviousSignal = signal;

			return new double[] { macd, signal };
		}
	}
}
