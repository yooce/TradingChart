using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.Plotters
{
	/// <summary>
	/// MACDのプロッターを表します。
	/// </summary>
	public class MacdPlotter : IndicatorPlotter<Indicators.MacdIndicator>
	{
		/// <summary>
		/// Series
		/// </summary>
		private Series[] Series = null;

		/// <summary>
		/// MacdPlotterの新しいインスタンスを初期化します。
		/// </summary>
		public MacdPlotter()
		{
			Series = new Series[3];
			for (int i = 0; i < Series.Length; i++)
			{
				Series[i] = new Series();
				Series[i].YAxisType = AxisType.Secondary;
				switch (i)
				{
					case 0:
						Series[i].ChartType = SeriesChartType.Line;
						Series[i].Color = Color.FromArgb(0, 143, 250);
						break;
					case 1:
						Series[i].ChartType = SeriesChartType.Line;
						Series[i].Color = Color.FromArgb(255, 103, 36);
						break;
					case 2:
						Series[i].ChartType = SeriesChartType.Column;
						break;
				}
			}
		}

		/// <summary>
		/// プロッター名を取得します。
		/// </summary>
		public override string Name { get => "MACD"; }

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
			DataPoint prevDp = null;
			for (int x = 0; x < candles.Count; x++)
			{
				double[] data = Indicator.GetValues(new Indicators.IndicatorArgs(GetCandlesForIndicator(x)));
				if (data == null) continue;

				// MACD
				Series[0].Points.Add(new DataPoint(x, data[0]));

				// MACDシグナル
				Series[1].Points.Add(new DataPoint(x, data[1]));

				// MACDオシレーター
				double y = data[0] - data[1];
				DataPoint dp = new DataPoint(x, y);
				if (y >= 0)
				{
					if (prevDp != null && prevDp.YValues[0] > y) dp.Color = Color.FromArgb(169, 224, 219);
					else dp.Color = Palette.PriceUpColor;
				}
				else
				{
					if (prevDp != null && prevDp.YValues[0] < y) dp.Color = Color.FromArgb(255, 204, 210);
					else dp.Color = Palette.PriceDownColor;
				}
				Series[2].Points.Add(dp);

				prevDp = dp;
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
			foreach (Series series in Series)
			{
				series.ChartArea = subChartArea.Name;
			}
			return new SubChartArea[] { subChartArea };
		}
	}
}
