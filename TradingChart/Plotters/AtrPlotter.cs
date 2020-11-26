using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.Plotters
{
	/// <summary>
	/// ATRのプロッターを表します。
	/// </summary>
	public class AtrPlotter : IndicatorPlotter<Indicators.AtrIndicator>
	{
		/// <summary>
		/// Series
		/// </summary>
		private Series Series = null;

		/// <summary>
		/// AtrPlotterクラスの新しいインスタンスを初期化します。
		/// </summary>
		public AtrPlotter()
		{
			Series = new Series();
			Series.ChartType = SeriesChartType.Line;
			Series.YAxisType = AxisType.Secondary;
			Series.Color = Color.FromArgb(163, 9, 27);
		}

		/// <summary>
		/// プロッター名を取得します。
		/// </summary>
		public override string Name { get => "ATR"; }

		/// <summary>
		/// Seriesの配列を取得します。
		/// </summary>
		public override Series[] SeriesArray { get => new Series[] { Series }; }

		/// <summary>
		/// データをプロットします。
		/// </summary>
		/// <param name="candles">ロウソク足のリスト</param>
		public override void Plot(List<DataTypes.Candle> candles)
		{
			base.Plot(candles);

			// クリア
			Series.Points.Clear();

			// プロット
			for (int x = 0; x < candles.Count; x++)
			{
				double[] data = Indicator.GetValues(new Indicators.IndicatorArgs(GetCandlesForIndicator(x)));
				if (data == null) continue;

				Series.Points.Add(new DataPoint(x, data));
			}
		}

		/// <summary>
		/// ChartAreaを設定します。
		/// </summary>
		/// <param name="mainChartArea">主ChartArea</param>
		/// <returns>使用する従ChartAreaの配列</returns>
		public override SubChartArea[] SetChartArea(MainChartArea mainChartArea)
		{
			SubChartArea subChartArea = new SubChartArea();
			Series.ChartArea = subChartArea.Name;
			return new SubChartArea[] { subChartArea };
		}
	}
}
