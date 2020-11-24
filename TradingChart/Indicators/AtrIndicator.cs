using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicalNuts.Indicators
{
	public class AtrIndicator : IIndicator
	{
		public int Term { get; set; } = 10;

		public double[] GetData(IndicatorArgs args)
		{
			if (args.Candles.Count < Term + 1) return null;

			decimal sum = 0;
			for (int i = 0; i < Term; i++)
			{
				sum += new decimal[]
				{
					Math.Abs(args.Candles[i].High - args.Candles[i].Low),		// 当日高値 - 当日安値
					Math.Abs(args.Candles[i].High - args.Candles[i + 1].Close),	// 当日高値 - 前日終値
					Math.Abs(args.Candles[i].Low - args.Candles[i + 1].Close)	// 当日安値 - 前日終値
				}.Max();
			}
			return new double[] { (double)(sum / Term) };
		}
	}
}
