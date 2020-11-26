using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.Plotters
{
	/// <summary>
	/// インジケーターのプロッターを表します。
	/// </summary>
	/// <typeparam name="T">インジケーターの型を指定します。</typeparam>
	public abstract class IndicatorPlotter<T> : IPlotter where T : Indicators.IIndicator, new()
	{
		/// <summary>
		/// インジケーター
		/// </summary>
		public T Indicator { get; private set; }

		/// <summary>
		/// 逆順ロウソク足のリスト
		/// </summary>
		private List<DataTypes.Candle> ReversedCandles = null;

		/// <summary>
		/// IndicatorPlotterの新しいインスタンスを初期化します。
		/// </summary>
		public IndicatorPlotter()
		{
			Indicator = new T();
		}

		/// <summary>
		/// プロッター名を取得します。
		/// </summary>
		public abstract string Name { get; }

		/// <summary>
		/// ChartAreaを設定します。
		/// </summary>
		/// <param name="mainChartArea">主ChartArea</param>
		/// <returns>使用する従ChartAreaの配列</returns>
		public abstract SubChartArea[] SetChartArea(MainChartArea mainChartArea);

		/// <summary>
		/// Seriesの配列を取得します。
		/// </summary>
		public abstract Series[] SeriesArray { get; }

		/// <summary>
		/// データをプロットします。
		/// </summary>
		/// <param name="candles">ロウソク足のリスト</param>
		public virtual void Plot(List<DataTypes.Candle> candles)
		{
			ReversedCandles = new List<DataTypes.Candle>();
			ReversedCandles.AddRange(candles);
			ReversedCandles.Reverse();
		}

		/// <summary>
		/// インジケーター用のロウソク足のリストを取得します。
		/// </summary>
		/// <param name="x">x座標</param>
		/// <returns>インジケーター用のロウソク足のリスト</returns>
		protected List<DataTypes.Candle> GetCandlesForIndicator(int x)
		{
			return ReversedCandles.GetRange(ReversedCandles.Count - x - 1, x + 1);
		}
	}
}
