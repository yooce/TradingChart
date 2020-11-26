﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MagicalNuts.Indicators
{
	/// <summary>
	/// ATRインジケーターを表します。
	/// </summary>
	public class AtrIndicator : IIndicator
	{
		/// <summary>
		/// 期間
		/// </summary>
		public int Period { get; set; } = 14;

		/// <summary>
		/// 前回のATR
		/// </summary>
		private double? PreviousAtr = null;

		/// <summary>
		/// 値を取得します。
		/// </summary>
		/// <param name="args">インジケーター引数</param>
		/// <returns>値</returns>
		public double[] GetValues(IndicatorArgs args)
		{
			// 必要期間に満たない
			if (args.Candles.Count < Period + 1) return null;

			// 真の値幅（TR）
			List<decimal> trs = new List<decimal>();
			for (int i = 0; i < Period; i++)
			{
				trs.Add(new decimal[]
				{
					Math.Abs(args.Candles[i].High - args.Candles[i].Low),		// 当日高値 - 当日安値
					Math.Abs(args.Candles[i].High - args.Candles[i + 1].Close),	// 当日高値 - 前日終値
					Math.Abs(args.Candles[i].Low - args.Candles[i + 1].Close)	// 当日安値 - 前日終値
				}.Max());
			}

			// ATR
			double atr = MovingAverageIndicator.GetMovingAverage(trs.Select(tr => (double)tr).ToArray(), MaMethod.Smma, PreviousAtr);

			// 次回のために覚えておく
			PreviousAtr = atr;

			return new double[] { atr };
		}
	}
}
