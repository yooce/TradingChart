using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicalNuts.Indicators
{
	public class AtrIndicator : IIndicator
	{
		public int Period { get; set; } = 14;

		public double[] GetData(IndicatorArgs args)
		{
			if (args.Candles.Count < Period + 1) return null;

			decimal sum = 0;
			for (int i = 0; i < Period; i++)
			{
				sum += new decimal[]
				{
					Math.Abs(args.Candles[i].High - args.Candles[i].Low),		// 当日高値 - 当日安値
					Math.Abs(args.Candles[i].High - args.Candles[i + 1].Close),	// 当日高値 - 前日終値
					Math.Abs(args.Candles[i].Low - args.Candles[i + 1].Close)	// 当日安値 - 前日終値
				}.Max();
			}
			return new double[] { (double)(sum / Period) };
		}
	}
}
