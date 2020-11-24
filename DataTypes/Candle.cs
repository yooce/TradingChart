using System;

namespace MagicalNuts.DataTypes
{
	public class Candle
	{
		public string Code { get; set; }
		public DateTime DateTime { get; set; }
		public decimal Open { get; set; }
		public decimal High { get; set; }
		public decimal Low { get; set; }
		public decimal Close { get; set; }
		public long Volume { get; set; }

		public static int Compare(Candle a, Candle b)
		{
			if (a.DateTime < b.DateTime) return -1;
			if (a.DateTime == b.DateTime) return 0;
			return 1;
		}
	}
}
