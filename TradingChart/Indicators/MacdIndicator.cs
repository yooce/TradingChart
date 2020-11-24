using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicalNuts.Indicators
{
	public class MacdIndicator : IIndicator
	{
		public int ShortTerm { get; set; } = 12;
		public int LongTerm { get; set; } = 26;
		public int SignalTerm { get; set; } = 9;

		private MovingAverageIndicator ShortMaIndicator = null;
		private MovingAverageIndicator LongMaIndicator = null;

		public MacdIndicator()
		{
			ShortMaIndicator = new MovingAverageIndicator();
			ShortMaIndicator.Term = ShortTerm;
			LongMaIndicator = new MovingAverageIndicator();
			LongMaIndicator.Term = LongTerm;
		}

		public double[] GetData(IndicatorArgs args)
		{
			if (args.Candles.Count < ShortTerm + SignalTerm || args.Candles.Count < LongTerm + SignalTerm) return null;

			List<double> macds = new List<double>();
			for (int i = 0; i < SignalTerm; i++)
			{
				macds.Add(ShortMaIndicator.GetData(new IndicatorArgs(args.Candles.GetRange(i, args.Candles.Count - i)))[0]
					- LongMaIndicator.GetData(new IndicatorArgs(args.Candles.GetRange(i, args.Candles.Count - i)))[0]);
			}

			return new double[] { macds[0], macds.Average() };
		}
	}
}
