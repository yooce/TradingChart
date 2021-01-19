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

		public DateTime DateTime(int i)
		{
			return this[i].DateTime;
		}

		public decimal Open(int i)
		{
			return this[i].Open;
		}

		public decimal High(int i)
		{
			return this[i].High;
		}

		public decimal Low(int i)
		{
			return this[i].Low;
		}

		public decimal Close(int i)
		{
			return this[i].Close;
		}

		public long Volume(int i)
		{
			return this[i].Volume;
		}

		public decimal TradingValue(int i)
		{
			return this[i].TradingValue;
		}

		public decimal Price(PriceType pt, int i)
		{
			decimal price = 0;
			switch (pt)
			{
				case PriceType.Open:
					price = Open(i);
					break;
				case PriceType.High:
					price = High(i);
					break;
				case PriceType.Low:
					price = Low(i);
					break;
				case PriceType.Close:
					price = Close(i);
					break;
			}
			return price;
		}

		public CandleCollection Shift(int i)
		{
			return new CandleCollection(GetRange(i, Count - i));
		}
	}
}
