using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicalNuts.Indicators
{
	public class MacdIndicator : IIndicator
	{
		public int FastPeriod { get; set; } = 12;
		public int SlowPeriod { get; set; } = 26;
		public int SignalPeriod { get; set; } = 9;

		private MovingAverageIndicator FastMaIndicator = null;
		private MovingAverageIndicator SlowMaIndicator = null;

		public MacdIndicator()
		{
			FastMaIndicator = new MovingAverageIndicator();
			FastMaIndicator.Period = FastPeriod;
			FastMaIndicator.MaMethod = MaMethod.Ema;
			SlowMaIndicator = new MovingAverageIndicator();
			SlowMaIndicator.Period = SlowPeriod;
			SlowMaIndicator.MaMethod = MaMethod.Ema;
		}

		public double[] GetData(IndicatorArgs args)
		{
			if (args.Candles.Count < FastPeriod + SignalPeriod || args.Candles.Count < SlowPeriod + SignalPeriod) return null;

			List<double> macds = new List<double>();
			for (int i = 0; i < SignalPeriod; i++)
			{
				macds.Add(FastMaIndicator.GetData(new IndicatorArgs(args.Candles.GetRange(i, args.Candles.Count - i)))[0]
					- SlowMaIndicator.GetData(new IndicatorArgs(args.Candles.GetRange(i, args.Candles.Count - i)))[0]);
			}

			return new double[] { macds[0], MovingAverageIndicator.GetMovingAverage(macds.ToArray(), MaMethod.Ema, macds.Last() ) };
		}
	}
}
