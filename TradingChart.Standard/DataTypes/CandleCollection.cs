using System;
using System.Collections.Generic;
using System.Text;

namespace MagicalNuts.DataTypes
{
	public class CandleCollection : List<Candle>
	{
		public CandleCollection(List<Candle> candles)
		{
			Clear();
			AddRange(candles);
		}

		public CandleCollection Shift(int i)
		{
			return new CandleCollection(GetRange(i, Count - i));
		}
	}
}
