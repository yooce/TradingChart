using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.Plotters
{
	/// <summary>
	/// プロッターのインターフェースを表します。
	/// </summary>
	public interface IPlotter
	{
		/// <summary>
		/// プロッター名を取得します。
		/// </summary>
		/// <returns>プロッター名</returns>
		string GetName();

		/// <summary>
		/// ChartAreaを設定します。
		/// </summary>
		/// <param name="mainChartArea">主ChartArea</param>
		/// <returns>使用する従ChartAreaの配列</returns>
		SubChartArea[] SetChartArea(MainChartArea mainChartArea);

		/// <summary>
		/// Seriesを取得します。
		/// </summary>
		/// <returns>Seriesの配列</returns>
		Series[] GetSeries();

		/// <summary>
		/// データをプロットします。
		/// </summary>
		/// <param name="candles">ロウソク足のリスト</param>
		void Plot(List<DataTypes.Candle> candles);
	}
}
