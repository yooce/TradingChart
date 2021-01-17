using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MagicalNuts.Indicators
{
	/// <summary>
	/// ATRインジケーターを表します。
	/// </summary>
	public class AtrIndicator : IndicatorBase
	{
		/// <summary>
		/// 期間を設定または取得します。
		/// </summary>
		[Category("ATR")]
		[DisplayName("期間")]
		[Description("期間を設定します。")]
		public int Period { get; set; } = 14;

		/// <summary>
		/// 前回のATR
		/// </summary>
		[Browsable(false)]
		private double? PreviousAtr = null;

		/// <summary>
		/// 非同期で準備します。
		/// </summary>
		/// <returns>非同期タスク</returns>
		public override async Task SetUpAsync()
		{
			PreviousAtr = null;
		}

		/// <summary>
		/// 値を取得します。
		/// </summary>
		/// <param name="args">インジケーター引数</param>
		/// <returns>値</returns>
		public override double[] GetValues()
		{
			// 必要期間に満たない
			if (Candles.Count < Period + 1) return null;

			// 真の値幅（TR）
			List<double> trs = new List<double>();
			for (int i = 0; i < Period; i++)
			{
				trs.Add(new double[]
				{
					Math.Abs(High(i) - Low(i)),		// 当日高値 - 当日安値
					Math.Abs(High(i) - Close(i + 1)),	// 当日高値 - 前日終値
					Math.Abs(Low(i) - Close(i + 1))	// 当日安値 - 前日終値
				}.Max());
			}

			// ATR
			double atr = MovingAverageIndicator.GetMovingAverage(trs.ToArray(), MaMethod.Smma, PreviousAtr);

			// 次回のために覚えておく
			PreviousAtr = atr;

			return new double[] { atr };
		}
	}
}
