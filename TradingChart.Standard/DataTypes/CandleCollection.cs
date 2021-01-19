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

		public double Open(int i)
		{
			return (double)this[i].Open;
		}

		public double High(int i)
		{
			return (double)this[i].High;
		}

		public double Low(int i)
		{
			return (double)this[i].Low;
		}

		public double Close(int i)
		{
			return (double)this[i].Close;
		}

		public long Volume(int i)
		{
			return this[i].Volume;
		}

		public double TradingValue(int i)
		{
			return (double)this[i].TradingValue;
		}

		public double Price(PriceType pt, int i)
		{
			double price = 0.0;
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
