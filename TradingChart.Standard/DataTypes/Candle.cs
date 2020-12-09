using System;

namespace MagicalNuts.DataTypes
{
	/// <summary>
	/// ロウソク足を表します。
	/// </summary>
	public class Candle
	{
		/// <summary>
		/// ロウソク足の開始日時
		/// </summary>
		public DateTime DateTime { get; set; }

		/// <summary>
		/// 始値
		/// </summary>
		public decimal Open { get; set; }

		/// <summary>
		/// 高値
		/// </summary>
		public decimal High { get; set; }

		/// <summary>
		/// 安値
		/// </summary>
		public decimal Low { get; set; }

		/// <summary>
		/// 終値
		/// </summary>
		public decimal Close { get; set; }

		/// <summary>
		/// 出来高
		/// </summary>
		public long Volume { get; set; }

		/// <summary>
		/// ソート用に２つのロウソク足を比較します。
		/// </summary>
		/// <param name="a">ロウソク足</param>
		/// <param name="b">ロウソク足</param>
		/// <returns>aの方が古ければ-1、同じなら0、aの方が新しければ1</returns>
		public static int Compare(Candle a, Candle b)
		{
			if (a.DateTime < b.DateTime) return -1;
			if (a.DateTime == b.DateTime) return 0;
			return 1;
		}
	}
}
