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
	public class AtrIndicator : IIndicator
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
		private decimal? PreviousAtr = null;

		/// <summary>
		/// 非同期で準備します。
		/// </summary>
		/// <returns>非同期タスク</returns>
		public async Task SetUpAsync()
		{
			PreviousAtr = null;
		}

		/// <summary>
		/// 値を取得します。
		/// </summary>
		/// <param name="candles">ロウソク足のコレクション</param>
		/// <returns>値</returns>
		public virtual decimal[] GetValues(DataTypes.CandleCollection candles)
		{
			// 必要期間に満たない
			if (candles.Count < Period + 1) return null;

			// 真の値幅（TR）
			List<decimal> trs = new List<decimal>();
			for (int i = 0; i < Period; i++)
			{
				trs.Add(new decimal[]
				{
					Math.Abs(candles[i].High - candles[i].Low),			// 当日高値 - 当日安値
					Math.Abs(candles[i].High - candles[i + 1].Close),	// 当日高値 - 前日終値
					Math.Abs(candles[i].Low - candles[i + 1].Close)		// 当日安値 - 前日終値
				}.Max());
			}

			// ATR
			decimal atr = MovingAverageIndicator.GetMovingAverage(trs.ToArray(), MaMethod.Smma, PreviousAtr);

			// 次回のために覚えておく
			PreviousAtr = atr;

			return new decimal[] { atr };
		}
	}
}
