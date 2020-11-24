using System.Drawing;

namespace MagicalNuts
{
	/// <summary>
	/// TradingChartで使用するパレットを表します。
	/// </summary>
	public static class Palette
	{
		/// <summary>
		/// 価格上昇時の色
		/// </summary>
		public static readonly Color PriceUpColor = Color.FromArgb(0, 167, 154);

		/// <summary>
		/// 価格下落時の色
		/// </summary>
		public static readonly Color PriceDownColor = Color.FromArgb(254, 77, 84);

		/// <summary>
		/// グリッドの色
		/// </summary>
		public static readonly Color GridColor = Color.FromArgb(224, 236, 242);

		/// <summary>
		/// カーソルの色
		/// </summary>
		public static readonly Color CursorColor = Color.FromArgb(115, 134, 149);

		/// <summary>
		/// カーソルラベルの色
		/// </summary>
		public static readonly Color CursorLabelColor = Color.FromArgb(76, 82, 93);

		/// <summary>
		/// 分割線の色
		/// </summary>
		public static readonly Color SplitterColor = Color.Silver;

		/// <summary>
		/// スクロールバーの色
		/// </summary>
		public static readonly Color ScrollBarColor = Color.FromArgb(127, Color.Silver);
	}
}
