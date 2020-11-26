using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.Plotters
{
	/// <summary>
	/// ボリンジャーバンドのプロッターを表します。
	/// </summary>
	public class BollingerBandPlotter : IndicatorPlotter<Indicators.BollingerBandIndicator>
	{
		/// <summary>
		/// Series
		/// </summary>
		private Series[] Series = null;

		/// <summary>
		/// BollingerBandPlotterクラスの新しいインスタンスを初期化します。
		/// </summary>
		public BollingerBandPlotter()
		{
			Series = new Series[4];
			for (int i = 0; i < Series.Length; i++)
			{
				Series[i] = new Series();
				Series[i].YAxisType = AxisType.Secondary;
				switch (i)
				{
					case 0:
						Series[i].ChartType = SeriesChartType.Line;
						Series[i].Color = Color.FromArgb(144, 30, 38);
						break;
					case 1:
						Series[i].ChartType = SeriesChartType.Range;
						Series[i].Color = Color.FromArgb(10, 0, 133, 131);
						break;
					default:
						Series[i].ChartType = SeriesChartType.Line;
						Series[i].Color = Color.FromArgb(0, 133, 131);
						break;
				}
			}
		}

		/// <summary>
		/// プロッター名を取得します。
		/// </summary>
		public override string Name { get => "ボリンジャーバンド"; }

		/// <summary>
		/// Seriesの配列を取得します。
		/// </summary>
		public override Series[] SeriesArray { get => Series; }

		/// <summary>
		/// データをプロットします。
		/// </summary>
		/// <param name="candles">ロウソク足のリスト</param>
		public override void Plot(List<DataTypes.Candle> candles)
		{
			base.Plot(candles);

			// クリア
			foreach (Series series in Series)
			{
				series.Points.Clear();
			}

			// プロット
			for (int x = 0; x < candles.Count; x++)
			{
				double[] data = Indicator.GetValues(new Indicators.IndicatorArgs(GetCandlesForIndicator(x)));
				if (data == null) continue;

				Series[0].Points.Add(new DataPoint(x, data[0]));
				Series[1].Points.Add(new DataPoint(x, new double[] { data[1], data[2] }));
				Series[2].Points.Add(new DataPoint(x, data[1]));
				Series[3].Points.Add(new DataPoint(x, data[2]));
			}
		}

		/// <summary>
		/// ChartAreaを設定します。
		/// </summary>
		/// <param name="mainChartArea">主ChartArea</param>
		/// <returns>使用する従ChartAreaの配列</returns>
		public override SubChartArea[] SetChartArea(MainChartArea mainChartArea)
		{
			foreach (Series series in Series)
			{
				series.ChartArea = mainChartArea.Name;
			}
			return null;
		}
	}
}
