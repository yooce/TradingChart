using MagicalNuts.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicalNuts.Indicators
{
	public class MovingAverageIndicator : IIndicator
	{
		public int Term { get; set; } = 25;

		public double[] GetData(IndicatorArgs args)
		{
			// 必要期間に満たない
			if (args.Candles.Count < Term) return null;

			return new double[] { (double)args.Candles.GetRange(0, Term).Average(candle => candle.Close) };
		}
	}
}
