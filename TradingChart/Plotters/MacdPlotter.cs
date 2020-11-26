using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.Plotters
{
	public class MacdPlotter : IndicatorPlotter<Indicators.MacdIndicator>
	{
		private Series[] Series = null;

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

		public override string GetName()
		{
			return "MACD";
		}

		public override Series[] GetSeries()
		{
			return Series;
		}

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
				double[] data = Indicator.GetData(new Indicators.IndicatorArgs(GetCandlesForIndicator(x)));
				if (data == null) continue;

				Series[0].Points.Add(new DataPoint(x, data[0]));
				Series[1].Points.Add(new DataPoint(x, data[1]));

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
