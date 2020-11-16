using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicalNuts.Indicators
{
	public class IndicatorArgs
	{
		public List<DataTypes.Candle> Candles { get; set; }

		public IndicatorArgs(List<DataTypes.Candle> candles)
		{
			Candles = candles;
		}
	}
}
