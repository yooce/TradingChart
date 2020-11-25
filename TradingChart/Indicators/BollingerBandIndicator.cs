using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicalNuts.Indicators
{
	public class BollingerBandIndicator : IIndicator
	{
		public int Period { get; set; } = 25;
		public double Deviation { get; set; } = 2.0;

		private MovingAverageIndicator MovingAverageIndicator = null;

		public BollingerBandIndicator()
		{
			MovingAverageIndicator = new MovingAverageIndicator();
			MovingAverageIndicator.Period = Period;
		}

		public double[] GetData(IndicatorArgs args)
		{
			if (args.Candles.Count < Period) return null;

			// 移動平均
			double ma = MovingAverageIndicator.GetData(new IndicatorArgs(args.Candles))[0];

			// 標準偏差
			List<double> closes = new List<double>();
			for (int i = 0; i < Period; i++)
			{
				closes.Add((double)args.Candles[i].Close);
			}
			double dev = closes.PopulationStandardDeviation();

			return new double[] { ma, ma + dev * Deviation, ma - dev * Deviation };
		}
	}
}
