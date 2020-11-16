using MagicalNuts.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.Indicators
{
	public class CandleIndicator : IIndicator
	{
		public double[] GetData(IndicatorArgs args)
		{
			return new double[4] {
				(double)args.Candles[0].High, (double)args.Candles[0].Low, (double)args.Candles[0].Open, (double)args.Candles[0].Close };
		}
	}
}
