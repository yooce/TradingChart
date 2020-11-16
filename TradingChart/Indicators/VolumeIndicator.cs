using MagicalNuts.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicalNuts.Indicators
{
	public class VolumeIndicator : IIndicator
	{
		public double[] GetData(IndicatorArgs args)
		{
			return new double[] { args.Candles[0].Volume };
		}
	}
}
